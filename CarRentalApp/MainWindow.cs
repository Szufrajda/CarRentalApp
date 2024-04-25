using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class MainWindow : Form
    {

        private Login _login;
        public string _roleName;
        public User _user;

        public MainWindow()
        {
            InitializeComponent();
        }


        public MainWindow(Login login, User user)
        {
            InitializeComponent();
            _login = login;
            _user = user;
            _roleName = user.UserRoles.FirstOrDefault().Role.SchortName;
        }

        private void addRentalRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var addRentalRecord = new AddEditRentalRecord(); // initialize object of class AddRentalRecord
            addRentalRecord.ShowDialog(); // allows to open independly only one window
            addRentalRecord.MdiParent = this; // MDI - represent of main window

        }

        private void manageVehicleListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // if it's already open scan the open forms and leave the only one form from the List ->  ManageVehicleListing
            var OpenForms = Application.OpenForms.Cast<Form>(); // list of forms 
            var isOpen = OpenForms.Any(q => q.Name == "ManageVehicleListing");

            // if it's not open just launch the Mdichild
            if (!isOpen)
            {
                var vehicleListing = new ManageVehicleListing();
                vehicleListing.MdiParent = this;

                vehicleListing.Show(); // launches new window INSIDE parent window
            }
        }

        private void viewArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("ManageRentalRecords"))
            {
                var manageRentalRecords = new ManageRentalRecords();
                manageRentalRecords.MdiParent = this;
                manageRentalRecords.Show();
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            _login.Close();
        }

        private void manageUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("ManageRentalRecords"))
            {
                var manageUsers = new ManageUsers();
                manageUsers.MdiParent = this;
                manageUsers.Show();
            }

        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            if(_user.Password == Utils.DefaultHashedPassowrd())
            {
                var resetPassword = new ResetPassword(_user);
                resetPassword.ShowDialog();

            }
            var username = _user.Username;
            toolStripStatusLabel1.Text = $"Logged in as: {username}";
            if (_roleName != "admin")
            {
                manageUsersToolStripMenuItem.Visible = false;
            }
        }
    }
}
