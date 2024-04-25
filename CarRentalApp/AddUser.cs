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
    public partial class AddUser : Form
    {

        private readonly CarRentalEntities _db;
        private ManageUsers _manageUsers;

        public AddUser(ManageUsers manageUsers)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            _manageUsers = manageUsers;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            var roles = _db.Roles.ToList();
            cbRole.DataSource = roles;
            cbRole.ValueMember = "id";
            cbRole.DisplayMember = "Name";

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var username = tbUsername.Text;
                var roleId = (int)cbRole.SelectedValue;

                var password = Utils.DefaultHashedPassowrd();
                var user = new User
                {
                    Username = username,
                    Password = password,
                    isActive = true,
                };

                _db.Users.Add(user);
                _db.SaveChanges();

                var userid = user.id;
                var userRole = new UserRole
                {
                    RoleId = roleId,
                    UserId = userid
                };

                _db.UserRoles.Add(userRole);
                _db.SaveChanges();

                MessageBox.Show($"New user {user.Username} added successfully!");
                _manageUsers.PopulateGrid();
                Close();

            }
            catch (Exception)
            {
                MessageBox.Show("An Error has occured");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
