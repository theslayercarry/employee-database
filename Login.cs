using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace semestrovaya_2
{
    public partial class Login : Form
    {

        DataBase db = new DataBase();
        public Login()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void textBox_login1_TextChanged(object sender, EventArgs e)
        {

        }
        public static String loginUser;
        public static String passUser;
        private void Login_Load(object sender, EventArgs e)
        {
            textBox_password1.PasswordChar = '●';
            pictureBox3.Visible = false;
            textBox_login1.MaxLength = 50;
            textBox_password1.MaxLength = 50;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            loginUser = textBox_login1.Text;
            passUser = textBox_password1.Text;

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("select `login`, `password` from staff_accounts where `login` = @uL and `password` = @uP", db.getConnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginUser;
            command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = passUser;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count == 1)
            {
                MessageBox.Show("Загрузка данных пользователя...", "Успешная авторизация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                staff_account frm1 = new staff_account();
                this.Hide();
                frm1.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Пожалуйста, проверьте свой пароль и имя аккаунта и попробуйте снова.", "Ошибка при входе", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            employee_report frm1 = new employee_report();
            this.Hide();
            frm1.ShowDialog();
            this.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            textBox_password1.UseSystemPasswordChar = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = true;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            textBox_password1.UseSystemPasswordChar = true;
            pictureBox3.Visible = true;
            pictureBox4.Visible = false;
        }

        private void checkBox_remember_me_MouseHover(object sender, EventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(checkBox_remember_me, "При следующем запуске приложения вам не нужно будет вводить пароль или подтверждать вход в аккаунт");
        }

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(pictureBox3, "Скрыть пароль");
        }

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(pictureBox4, "Показать пароль");
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
