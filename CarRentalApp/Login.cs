using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class Login : Form
    {

        private readonly CarRentalEntities _db;

        public Login()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {

                SHA256 sHA256 = SHA256.Create();

                var username = tbUsername.Text.Trim();
                var password = tbPassword.Text;

                var hashed_password = Utils.HashPassowrd(password);


                var user = _db.Users.FirstOrDefault(q => q.Username == username && 
                q.Password == hashed_password && q.isActive == true);

                if (user == null) 
                {
                    MessageBox.Show("Please provide valid credentials");
                }
                else
                {
                    // Ctrl + Shift + B  -> compile after changes in database

                    //var role = user.UserRoles.FirstOrDefault();
                    //var roleSchortName = role.Role.SchortName;
                    //var mainWindow = new MainWindow(this, roleSchortName);
                    //mainWindow.Show();
                    //Hide();


                    var mainWindow = new MainWindow(this, user);
                    mainWindow.Show();
                    Hide();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong. Please try again");
            }
        }
    }
}
