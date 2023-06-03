using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace semestrovaya_2
{

    public partial class add_employee : Form
    {
        DataBase db = new DataBase();
        public add_employee()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            textBox_phone.Text = "+7 XXX XXX XX XX";
            textBox_phone.ForeColor = Color.DimGray;

            string myConnectionString = "Database=semestrovaya;Data Source=127.0.0.1;User Id=root;Password=1337";


            DataTable table_posts = new DataTable();

            MySqlConnection connection_posts = new MySqlConnection(myConnectionString);
            {
                MySqlCommand command = new MySqlCommand("select idpositions, title from positions", connection_posts);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(table_posts);
            }
            comboBox_post.DataSource = table_posts;
            comboBox_post.DisplayMember = "title";
            comboBox_post.ValueMember = "idpositions";
        }

        private void add_employee_Load(object sender, EventArgs e)
        {
            textBox_phone.MaxLength = 11;
            textBox_count_days.MaxLength = 2;
            textBox_work_exp.MaxLength = 2;
            textBox_email.MaxLength = 100;
            comboBox_city.MaxLength = 100;
            textBox_name.MaxLength = 100;
            comboBox_post.Text = "";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string myConnectionString = "Database=semestrovaya;Data Source=127.0.0.1;User Id=root;Password=1337";

            ///////////////////////////////////////////////
            string employees_count; int accounts_count;
            ///////////////////////////////////////////////
            String name = textBox_name.Text;
            String work_experience = textBox_work_exp.Text;
            String phone = textBox_phone.Text;
            String email = textBox_email.Text;
            String work_from = maskedTextBox_work_from.Text;
            String work_to = maskedTextBox_work_to.Text;
            String work_days = textBox_count_days.Text;
            String city = comboBox_city.Text;



            MySqlCommand cmd = new MySqlCommand("insert into staff (name, work_experience, id_positions, work_schedule_from, work_schedule_to, number_of_working_days, date_of_admission) values \r\n(@name, @work_experience, @position, @work_from, @work_to, @work_days, now())", db.getConnection());
            cmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
            cmd.Parameters.Add("@work_experience", MySqlDbType.VarChar).Value = work_experience;
            cmd.Parameters.Add("@position", MySqlDbType.VarChar).Value = comboBox_post.SelectedValue;
            cmd.Parameters.Add("@work_from", MySqlDbType.VarChar).Value = work_from;
            cmd.Parameters.Add("@work_to", MySqlDbType.VarChar).Value = work_to;
            cmd.Parameters.Add("@work_days", MySqlDbType.VarChar).Value = work_days;

            // --------------------------------------------------------------------------------------- //
           /* MySqlConnection employees_connection = new MySqlConnection(myConnectionString);

            MySqlCommand command = new MySqlCommand("select max(idstaff) from staff", employees_connection);
            employees_connection.Open();

            employees_count = command.ExecuteScalar().ToString();
            accounts_count = Convert.ToInt32(employees_count);

            employees_connection.Close();*/
            
            Random rnd = new Random();

            string password = GetPass();
            int login = rnd.Next(10000000, 99999999);

            // --------------------------------------------------------------------------------------- //

            db.openConnection();
            if (textBox_count_days.TextLength < 1 || textBox_work_exp.TextLength < 1 || textBox_email.TextLength < 9 || comboBox_city.Text == "" || textBox_phone.TextLength < 11 || textBox_phone.Text == "+7 XXX XXX XX XX" || textBox_name.TextLength < 1 || maskedTextBox_work_from.Text == "  :" || maskedTextBox_work_to.Text == "  :")
            {
                MessageBox.Show("1.Введите ФИО сотрудника.\r\n" +
                    "2.Выберите должность.\r\nДругие ошибки выделены ниже:\r\n\n" +
                    "3.Выберите город\r\n" +
                    "4.Введите существующий адрес эл.почты\r\n" +
                    "5.Введите номер телефона\r\n" +
                    "6.Введите опыт работы\r\n" +
                    "7.Введите количество рабочих дней\r\n" +
                    "8.Введите график работы", "Несоответствие форме добавления сотрудника\r\n");
            }
            else if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Сотрудник '" + name + "' успешно добавлен.", "Добавление сотрудника...");
                employee_report frm_login = new employee_report();

                DataTable table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();

                MySqlConnection employees_connection = new MySqlConnection(myConnectionString);
                MySqlCommand command_max = new MySqlCommand("select max(idstaff) from staff", employees_connection);
                employees_connection.Open();

                employees_count = command_max.ExecuteScalar().ToString();
                accounts_count = Convert.ToInt32(employees_count);

                employees_connection.Close();


                MySqlCommand cmd_2 = new MySqlCommand("insert into staff_accounts (idstaff_accounts, login, password, city, phone, email) values\r\n(@idstaff_account, @login, @password, @city, @phone, @email)", db.getConnection());
                cmd_2.Parameters.Add("@idstaff_account", MySqlDbType.VarChar).Value = accounts_count;
                cmd_2.Parameters.Add("@login", MySqlDbType.VarChar).Value = login;
                cmd_2.Parameters.Add("@password", MySqlDbType.VarChar).Value = password;
                cmd_2.Parameters.Add("@city", MySqlDbType.VarChar).Value = city;
                cmd_2.Parameters.Add("@phone", MySqlDbType.VarChar).Value = phone;
                cmd_2.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;

                adapter.SelectCommand = cmd_2;
                adapter.Fill(table);

                db.closeConnection();

                this.Hide();
                frm_login.ShowDialog();
            }
            else
            {
                MessageBox.Show("Произошла ошибка при добавлении сотрудника.", "Ошибка при добавлении...");
            }

        }


        public string GetPass()
        {
            int[] arr = new int[12];
            Random rnd = new Random();
            string Password = "";

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = rnd.Next(33,90);
                Password += (char)arr[i];
            }
            return Password;
        }


        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox_phone_Enter(object sender, EventArgs e)
        {
            if (textBox_phone.Text == "+7 XXX XXX XX XX")
                textBox_phone.Text = "";
        }

        private void textBox_phone_Leave(object sender, EventArgs e)
        {
            if (textBox_phone.Text == "")
            {
                textBox_phone.Text = "+7 XXX XXX XX XX";
                textBox_phone.ForeColor = Color.DimGray;
            }
        }

        private void textBox_phone_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox_phone_MouseEnter(object sender, EventArgs e)
        {
            textBox_phone.ForeColor = Color.DarkSlateGray;
        }

        private void textBox_phone_MouseLeave(object sender, EventArgs e)
        {
            if (textBox_phone.Text == "" || textBox_phone.Text == "+7 XXX XXX XX XX")
                textBox_phone.ForeColor = Color.Gray;
        }

        private void textBox_email_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && !Char.IsLetter(number) && number != 45 && number != 46 && number != 64)
            {
                e.Handled = true;
            }
        }

        private void textBox_phone_MouseHover(object sender, EventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(textBox_phone, "Минимальная длина номера телефона 11 цифр");
        }

        private void textBox_work_exp_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox_count_days_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox_name_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (number != 8 && !Char.IsLetter(number) && number != 32)
            {
                e.Handled = true;
            }
        }

        private void comboBox_city_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (number != 8 && !Char.IsLetter(number))
            {
                e.Handled = true;
            }
        }
    }
}
