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
    public partial class ManageUsers : Form
    {

        private readonly CarRentalEntities _db;
        

        public ManageUsers()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();

        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddUser"))
            {
                var addUser = new AddUser(this);
                addUser.MdiParent = this.MdiParent;
                addUser.Show();
            }
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                // Get ID of Selected row
                var id = (int)gvUserList.SelectedRows[0].Cells["id"].Value;

                // Changing user's password inside database
                var user = _db.Users.FirstOrDefault(q => q.id == id);
                var hashed_password = Utils.DefaultHashedPassowrd();

                user.Password = hashed_password;
                _db.SaveChanges();

                MessageBox.Show($"{user.Username}'s password has been reset!\n\r" + "New Password: 'Password@123' ");


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnDeactivateUser_Click(object sender, EventArgs e)
        {
            try
            {
                // Get ID of Selected row
                var id = (int)gvUserList.SelectedRows[0].Cells["id"].Value;

                // Deactivating user's account inside database
                var user = _db.Users.FirstOrDefault(q => q.id == id);
                user.isActive = user.isActive == true ? false : true;
                _db.SaveChanges();

                MessageBox.Show($"{user.Username}'s activity status has changed!");
                PopulateGrid();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        public void PopulateGrid()
        {
            var users = _db.Users.Select(q => new
            {
                q.id,
                q.Username,
                q.UserRoles.FirstOrDefault().Role.Name,
                q.isActive
            })
                .ToList();
            gvUserList.DataSource = users;
            gvUserList.Columns["Username"].HeaderText = "Username";
            gvUserList.Columns["Name"].HeaderText = "Role name";
            gvUserList.Columns["isActive"].HeaderText = "Active";

            gvUserList.Columns["id"].Visible = false;
        }


    }
}
