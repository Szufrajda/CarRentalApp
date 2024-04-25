using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class AddEditRentalRecord : Form
    {
        private bool isEditMode;
        private readonly CarRentalEntities _db; //install connection with databae
        public AddEditRentalRecord()
        {
            InitializeComponent();
            lblTitle.Text = "Add New Rental";
            this.Text = "Add New Rental";
            isEditMode = false;
            _db = new CarRentalEntities();
        }

        public AddEditRentalRecord(CarRentalRecord recordToEdit)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Rental Record";
            this.Text = "Edit Rental Record";

            if (recordToEdit == null)
            {
                MessageBox.Show("Please ensure that you selected a valid record to edit");
                Close();
            }
            else
            {
                isEditMode = true;
                _db = new CarRentalEntities();
                PopulateFields(recordToEdit);
            }
        }

        private void PopulateFields(CarRentalRecord recordToEdit)
        {
            tbCustomerName.Text = recordToEdit.CustomerName;
            dtRented.Value = (DateTime)recordToEdit.DateRented;
            dtReturned.Value = (DateTime)recordToEdit.DateReturned;
            tbCost.Text = recordToEdit.Cost.ToString();
            lblRecordId.Text = recordToEdit.id.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                string customerName = tbCustomerName.Text;
                var dateOut = dtRented.Value;
                var dateIn = dtReturned.Value;
                double cost = Convert.ToDouble(tbCost.Text);

                var carType = cbTypeOfCar.Text;

                var isValid = true;
                var errorMessage = "";

                if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(carType))
                {
                    isValid = false;
                    errorMessage += "ERROR: Please enter missing data.\n\r";
                }

                if (dateOut > dateIn)
                {
                    isValid = false;
                    errorMessage += "ERROR: Illegal date selection.\n\r";
                }


                if (isValid)
                {
                    var rentalRecord = new CarRentalRecord();
                    if (isEditMode)
                    {
                        var id = int.Parse(lblRecordId.Text);
                        rentalRecord = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);

                    }
                    rentalRecord.CustomerName = customerName;
                    rentalRecord.DateRented = dateOut;
                    rentalRecord.DateReturned = dateIn;
                    rentalRecord.Cost = (decimal)cost;
                    rentalRecord.TypeOfCarId = (int)cbTypeOfCar.SelectedValue;

                    if (!isEditMode)
                    {
                        _db.CarRentalRecords.Add(rentalRecord);
                        _db.SaveChanges();

                        MessageBox.Show($"Customer Name: {customerName}\n\r" +
                            $"Date Rented: {dateOut}\n\r" +
                            $"Date Returned: {dateIn}\n\r" +
                            $"Cost: {cost}\n\r" +
                            $"Car Type: {carType}\n\r" +
                            $"THANK YOU FOR YOUR RENTING!");
                        Close();
                    }
                    else
                    {
                        rentalRecord = new CarRentalRecord();
                        rentalRecord.CustomerName = customerName;
                        rentalRecord.DateRented = dateOut;
                        rentalRecord.DateReturned = dateIn;
                        rentalRecord.Cost = (decimal)cost;
                        rentalRecord.TypeOfCarId = (int)cbTypeOfCar.SelectedValue;


                        // ADDING TO THE DATABSE NEW DATA
                        _db.CarRentalRecords.Add(rentalRecord);
                        _db.SaveChanges();


                        MessageBox.Show($"Customer Name: {customerName}\n\r" +
                            $"Date Rented: {dateOut}\n\r" +
                            $"Date Returned: {dateIn}\n\r" +
                            $"Cost: {cost}\n\r" +
                            $"Car Type: {carType}\n\r" +
                            $"THANK YOU FOR YOUR RENTING!");
                    }
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // SELECT * FROM TypeOfCars
            //var cars = carRentalEntities.TypesOfCars.ToList(); // convert to list types of cars from database

            var cars = _db.TypesOfCars
                .Select(q => new
                {
                    Id = q.id,
                    Name = q.Make + " " + q.Model
                }).ToList();
            
            cbTypeOfCar.DisplayMember = "Name"; // text what user see
            cbTypeOfCar.ValueMember = "id";
            cbTypeOfCar.DataSource = cars; // connection to the database
        }

        //private void button1_Click1(object sender, EventArgs e)
        //{
        //    MainWindow mainWindow = new MainWindow(); // initialize objext of class MainWindow
        //    mainWindow.Show();
        //}
    }
}
