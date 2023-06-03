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
    public partial class closing_orders : Form
    {
        DataBase db = new DataBase();
        public closing_orders()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            string myConnectionString = "Database=semestrovaya;Data Source=127.0.0.1;User Id=root;Password=1337";


            DataTable available_orders = new DataTable();

            MySqlConnection connection_active_orders = new MySqlConnection(myConnectionString);
            {
                MySqlCommand command = new MySqlCommand("select idorders, title from orders\r\njoin staff_to_taken_orders on staff_to_taken_orders.id_orders = orders.idorders\r\njoin staff on staff_to_taken_orders.id_staff = staff.idstaff \r\njoin staff_accounts on staff_accounts.idstaff_accounts = staff.idstaff \r\nwhere not exists (select 1 from staff_to_completed_orders where staff_to_completed_orders.id_orders = staff_to_taken_orders.id_orders and staff_to_completed_orders.id_staff = staff_to_taken_orders.id_staff)\r\nand staff_accounts.login = @uL and staff_accounts.password = @uP", connection_active_orders);
                command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = Login.loginUser;
                command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = Login.passUser;
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(available_orders);
            }
            comboBox1.DataSource = available_orders;
            comboBox1.DisplayMember = "title";
            comboBox1.ValueMember = "idorders";
        }

        private void closing_orders_Load(object sender, EventArgs e)
        {
            comboBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (check_records())
            {
                return;
            }
            var id_staff = staff_account.idstaff_int;
            var id_order = comboBox1.SelectedValue;

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("insert into staff_to_completed_orders (id_staff, id_orders, time_of_completion) values (@idstaff, @idorder, now())", db.getConnection());
            command.Parameters.Add("@idstaff", MySqlDbType.Int32).Value = id_staff;
            command.Parameters.Add("@idorder", MySqlDbType.Int32).Value = id_order;


            db.openConnection();
            if(comboBox1.Text == "")
            {
                MessageBox.Show("Ни один заказ не выбран.");
            }
            else if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Отчёт о выполнении заказа был успешно отправлен.\r\n\n\t" + DateTime.Now, "Обратная связь с заказчиком", MessageBoxButtons.OK, MessageBoxIcon.Information);

                db.closeConnection();
                /*adapter.SelectCommand = command;
                adapter.Fill(table);*/
            }
            else
            {
                MessageBox.Show("Произошла ошибка при отправлении отчёта.", "Обратная связь с заказчиком", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private Boolean check_records()
        {
            var id_order = comboBox1.SelectedValue;
            var id_staff = staff_account.idstaff_int;

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("select * from staff_to_completed_orders where\r\nid_staff = @idstaff and id_orders = @idorder", db.getConnection());
            command.Parameters.Add("@idstaff", MySqlDbType.Int32).Value = id_staff;
            command.Parameters.Add("@idorder", MySqlDbType.Int32).Value = id_order;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Вы уже отправили отчёт по данному заказу");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
