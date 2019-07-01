using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Assignment3
{
    public partial class Form1 : Form
    {
        List<Customer> customerList = new List<Customer>();
        int index = 0;
        public Form1()
        {
            InitializeComponent();
        }
        Model1 mod = new Model1();

        private void Form1_Load(object sender, EventArgs e)
        {
            using (Model1 mod = new Model1())
            {
                this.customerList = mod.Customers.ToList();
                show();
            }

        }
        String error;
        private void show()
        {

            this.firstName1.Text = this.customerList[index].FirstName;
            this.lastName1.Text = this.customerList[index].LastName;
            this.streetNumber1.Text = this.customerList[index].StreetNumber;
            this.street1.Text = this.customerList[index].Street;
            this.city1.Text = this.customerList[index].City;
            this.province1.Text = this.customerList[index].Province;
            this.country1.Text = this.customerList[index].Country;
            this.postalCode1.Text = this.customerList[index].PostalCode;
            this.phoneNumber1.Text = this.customerList[index].PhoneNumber;
            this.emailAddress1.Text = this.customerList[index].Email;
            validations();
        }

        private void previous_Click(object sender, EventArgs e)
        {
            nextClick.Text = "";
            if (this.index > 0)
            {
                this.index--;
                show();
            }
            else
            {
                this.error = "No previous record";
                previousClick.Text = error;
            }

        }

        private void next_Click(object sender, EventArgs e)
        {
            previousClick.Text = "";
            if (this.index < this.customerList.Count - 1)
            {
                this.index++;
                show();
            }
            else
            {
                this.error = "Last record";
                nextClick.Text = error;
            }
        }

        private void validations()
        {
            var postalCodeValidation = Regex.Match(this.customerList[index].PostalCode, @"^([ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ])\ {0,1}(\d[ABCEGHJKLMNPRSTVWXYZ]\d)$");
            var phoneValidation = Regex.Match(this.customerList[index].PhoneNumber, @"^[0-9]{10}$");
            var emailValidation = Regex.Match(this.customerList[index].Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            if (postalCodeValidation.Success && phoneValidation.Success && emailValidation.Success)
            {
                this.error = "Everything is fine";
                this.status1.Text = error;
            }
            else {
                if (!postalCodeValidation.Success)
                {
                    this.error = "Postal Code is not valid";
                    this.status1.Text = error;
                }

                if (!phoneValidation.Success)
                {
                    this.error = "Phone Number is not valid";
                    this.status1.Text = error;
                }
                if (!emailValidation.Success)
                {
                    this.error = "Email is not valid";
                    this.status1.Text = error;
                }
            }

        }
    }
    
}
