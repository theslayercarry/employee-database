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
    public partial class taking_orders : Form
    {
        DataBase db = new DataBase();

        int i = 0;
        int j = 0;
        public taking_orders()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            string myConnectionString = "Database=semestrovaya;Data Source=127.0.0.1;User Id=root;Password=1337";


            DataTable available_orders = new DataTable();

            MySqlConnection connection_active_orders = new MySqlConnection(myConnectionString);
            {
                MySqlCommand command = new MySqlCommand("select idorders, title from orders\r\nwhere not exists (select 1 from staff_to_taken_orders\r\nwhere staff_to_taken_orders.id_orders = orders.idorders)", connection_active_orders);
                command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = Login.loginUser;
                command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = Login.passUser;
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(available_orders);
            }
            listBox1.DataSource = available_orders;
            listBox1.DisplayMember = "title";
            listBox1.ValueMember = "idorders";

        }

        private void taking_orders_Load(object sender, EventArgs e)
        {
            string myConnectionString = "Database=semestrovaya;Data Source=127.0.0.1;User Id=root;Password=1337";

            MySqlConnection orders_count_connection = new MySqlConnection(myConnectionString);
            MySqlCommand command_orders_count = new MySqlCommand("select count(*) from orders\r\nwhere not exists (select 1 from staff_to_taken_orders\r\nwhere staff_to_taken_orders.id_orders = orders.idorders)", orders_count_connection);
            orders_count_connection.Open();

            String orders_count;

            orders_count = command_orders_count.ExecuteScalar().ToString();
            orders_count_connection.Close();

            label3.Text = "Заказы (" + orders_count + "):";

        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (i++ == 0)
            {
                CreateColumns_description();
            }
            RefreshDataGrid_description(dataGridView_description);
            dataGridView_description.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_description.ClearSelection();

            if (j++ == 0)
            {
                CreateColumns_requirements();
            }
            RefreshDataGrid_requirements(dataGridView_requirements);
            dataGridView_requirements.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_requirements.ClearSelection();

            string myConnectionString = "Database=semestrovaya;Data Source=127.0.0.1;User Id=root;Password=1337";

            MySqlConnection salary_order_connection = new MySqlConnection(myConnectionString);

            MySqlCommand command_salary_order = new MySqlCommand("select salary from orders where idorders = @idorder", salary_order_connection);
            command_salary_order.Parameters.Add("@idorder", MySqlDbType.VarChar).Value = listBox1.SelectedValue;
            salary_order_connection.Open();

            String orders_salary;

            orders_salary = command_salary_order.ExecuteScalar().ToString();
            salary_order_connection.Close();

            textBox1.Text = orders_salary;

        }

        private void CreateColumns_description()
        {
            dataGridView_description.Columns.Add("description", "Описание");
            dataGridView_description.Columns.Add("IsNew", String.Empty);
            dataGridView_description.Columns[0].Width = 469;

            dataGridView_description.Columns[1].Visible = false;
        }

        private void CreateColumns_requirements()
        {
            dataGridView_requirements.Columns.Add("requirements", "Требования");
            dataGridView_requirements.Columns.Add("IsNew", String.Empty);
            dataGridView_requirements.Columns[0].Width = 469;

            dataGridView_requirements.Columns[1].Visible = false;
        }

        private void ReadSingleRow_description(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetString(0));

        }

        private void ReadSingleRow_requirements(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetString(0));

        }

        private void RefreshDataGrid_description(DataGridView dwg)
        {
            dwg.Rows.Clear();
            MySqlCommand command = new MySqlCommand("select description from orders where idorders = @idorder", db.getConnection());
            command.Parameters.Add("@idorder", MySqlDbType.VarChar).Value = listBox1.SelectedValue;
            db.openConnection();

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow_description(dwg, reader);
            }
            reader.Close();
        }

        private void RefreshDataGrid_requirements(DataGridView dwg)
        {
            dwg.Rows.Clear();
            MySqlCommand command = new MySqlCommand("select requirements from orders where idorders = @idorder", db.getConnection());
            command.Parameters.Add("@idorder", MySqlDbType.VarChar).Value = listBox1.SelectedValue;
            db.openConnection();

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow_requirements(dwg, reader);
            }
            reader.Close();
        }

        private void dataGridView_description_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ((DataGridView)sender).SelectedCells[0].Selected = false;
            }
            catch { }
        }

        private void dataGridView_requirements_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ((DataGridView)sender).SelectedCells[0].Selected = false;
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (check_records())
            {
                return;
            }
            var id_staff = staff_account.idstaff_int;
            var id_order = listBox1.SelectedValue;

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("insert into staff_to_taken_orders (id_staff, id_orders, time_of_taking) values (@idstaff, @idorder, now())", db.getConnection());
            command.Parameters.Add("@idstaff", MySqlDbType.Int32).Value = id_staff;
            command.Parameters.Add("@idorder", MySqlDbType.Int32).Value = id_order;


            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Заказ успешно добавлен на ваш аккаунт.\r\n\n\t" + DateTime.Now, "Добавление заказа", MessageBoxButtons.OK, MessageBoxIcon.Information);

                db.closeConnection();
                /*adapter.SelectCommand = command;
                adapter.Fill(table);*/
            }
            else
            {
                MessageBox.Show("Произошла ошибка при добавлении заказа на аккаунт.", "Ошибка при добавлении заказа", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            dataGridView_description.Rows.Clear();
            dataGridView_requirements.Rows.Clear();
            textBox1.Text = "";
        }

        private Boolean check_records()
        {
            var id_order = listBox1.SelectedValue;
            var id_staff = staff_account.idstaff_int;

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("select * from staff_to_taken_orders where\r\nid_staff = @idstaff and id_orders = @idorder", db.getConnection());
            command.Parameters.Add("@idstaff", MySqlDbType.Int32).Value = id_staff;
            command.Parameters.Add("@idorder", MySqlDbType.Int32).Value = id_order;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Данный заказ уже добавлен на ваш аккаунт");
                return true;
            }
            else
            {
                return false;
            }
        }

        private void taking_orders_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
