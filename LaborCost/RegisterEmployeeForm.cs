using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaborCost
{
    public partial class RegisterEmployeeForm : Form
    {
        public RegisterEmployeeForm()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 1;
            this.dateTimePicker1.MaxDate = DateTime.Now.Date;
        }

        

        private void buttonSaveEmployee_Click(object sender, EventArgs e)
        {
            int timeSpan = 0;
            try
            {
                Employee employee = new Employee();

                employee.FirstName = this.textBoxFirstName.Text;
                employee.LastName = this.textBoxLastName.Text;
                employee.Email = this.textBoxEmail.Text;
                employee.PhoneNumber = this.textBoxPhoneNumber.Text;

                //dateTimePicker1.Format = DateTimePickerFormat.Custom;
                employee.BirthDay = dateTimePicker1.Value.Date;
                timeSpan = DateTime.Now.Date.Year - employee.BirthDay.Year;
              
                
                employee.Adress = this.textBoxAdress.Text;
                employee.Username = this.textBoxUserName.Text;
                employee.Password = this.textBoxPassword.Text;

                if (this.comboBox1.SelectedItem.Equals("Shift Manager"))
                {
                    employee.Role = 2;
                }
                if (this.comboBox1.SelectedItem.Equals("Employee"))
                {
                    employee.Role = 1;
                }
              
                PostgreSqlConnector connector = new PostgreSqlConnector();

                connector.Insert_Employee(employee);

                MessageBox.Show("Uspjesno ste registrirali zaposlenika!", "Registracija zaposlenika");

                this.Close();
            }
            catch (Exception)
            {
                if (timeSpan <= 18)
                {
                    MessageBox.Show("You can't register employee that is under 18 years old!", "Checking age");
                }

              
            }
            





        }

        private void RegisterEmployeeForm_Load(object sender, EventArgs e)
        {

        }
    }
}
