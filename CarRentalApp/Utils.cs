﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    internal class Utils
    {
        public static bool FormIsOpen(string name)
        {
            // Check if window is already open
            var OpenForms = Application.OpenForms.Cast<Form>();
            var isOpen = OpenForms.Any(q => q.Name == name);
            return isOpen;
        }


        public static string HashPassowrd(string password)
        {
            SHA256 sha = SHA256.Create();

            // Convert the input string to a byte array and compute the hash
            byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes(password));


            // Create a new StringBuilder to collect bytes
            // and create a string
            StringBuilder sBuilder = new StringBuilder();


            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static string DefaultHashedPassowrd()
        {
            SHA256 sha = SHA256.Create();

            // Convert the input string to a byte array and compute the hash
            byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes("Password@123"));


            // Create a new StringBuilder to collect bytes
            // and create a string
            StringBuilder sBuilder = new StringBuilder();


            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
