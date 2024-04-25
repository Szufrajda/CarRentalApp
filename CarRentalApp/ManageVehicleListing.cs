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
    public partial class ManageVehicleListing : Form
    {

        private readonly CarRentalEntities _db;

        public ManageVehicleListing()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void ManageVehicleListing_Load(object sender, EventArgs e)
        {

            try {
                //Simple Refresh Option
                PopulateGrid();
            } catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }


            // SELECT * FROM TypeOfCars
            //var cars = _db.TypesOfCars.ToList();

            // SELECT (COLUMN) as {Name of Object} FROM TypesOfCars
            //var cars = _db.TypesOfCars
            //    .Select(q => new 
            //    { 
            //        Make = q.Make, 
            //        Model = q.Model, 
            //        VIN = q.VIN, 
            //        Year = q.Year, 
            //        LicensePlateNumber = q.LicensePlateNumber,
            //        q.id
            //    })
            //    .ToList(); 

            //gvVehicleList.DataSource = cars;

            //gvVehicleList.Columns[4].HeaderText = "License Plate Number";
            //gvVehicleList.Columns[5].Visible = false;


            //gvVehicleList.Columns[0].HeaderText = "ID"; // first column with header ID
            //gvVehicleList.Columns[1].HeaderText = "NAME"; 
        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            AddEditVehicle addEditVehicle = new AddEditVehicle(this);
            addEditVehicle.ShowDialog();
            addEditVehicle.MdiParent = this.MdiParent;

        }

        private void btnEditCar_Click(object sender, EventArgs e)
        {
            try
            {
                // Get ID of Selected row
                var id = (int)gvVehicleList.SelectedRows[0].Cells["id"].Value;

                // Query Database for record
                var car = _db.TypesOfCars.FirstOrDefault(q => q.id == id);

                // Launch AddEditVehicle window with Data
                var addEditVehicle = new AddEditVehicle(car, this);
                addEditVehicle.MdiParent = this.MdiParent;
                addEditVehicle.Show();
            } catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }



        private void btnDeleteCar_Click(object sender, EventArgs e)
        {
            try
            {
                // Get ID of Selected row
                var id = (int)gvVehicleList.SelectedRows[0].Cells["id"].Value;

                // Query Database for record
                var car = _db.TypesOfCars.FirstOrDefault(q => q.id == id);

                DialogResult dr = MessageBox.Show("Are You sure you want to DELETE this record?", 
                    "Delete", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    // DELETE Vehicle FROM table
                    _db.TypesOfCars.Remove(car);
                    _db.SaveChanges();
                }
                PopulateGrid();

            } catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }



        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //Simple Refresh Option
            PopulateGrid();
        }


        // Function that can be called anytime we need a grid refresh
        public void PopulateGrid()
        {
            // Select a custom model collection of cars from database
            var cars = _db.TypesOfCars
                .Select(q => new
                {
                    Make = q.Make,
                    Model = q.Model,
                    VIN = q.VIN,
                    Year = q.Year,
                    LicensePlateNumber = q.LicensePlateNumber,
                    q.id
                })
                .ToList();


            gvVehicleList.DataSource = cars;
            gvVehicleList.Columns[4].HeaderText = "License Plate Number";
            //Hide the column for ID. Changed from the hard coded column value to the name, 
            // to make it more dynamic. 

            gvVehicleList.Columns["id"].Visible = false;
        }
    }
}
