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
    public partial class dismissed_employees : Form
    {
        DataBase db = new DataBase();

        int selectedRow;

        int k = 0;
        int j;
        String m;

        public dismissed_employees()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void dismissed_employees_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
            dataGridView1.Columns[10].Visible = false;
            dataGridView1.ClearSelection();
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("idstaff", "id");
            dataGridView1.Columns.Add("name", "ФИО");
            dataGridView1.Columns.Add("work_experience", "Опыт работы");
            dataGridView1.Columns.Add("id_positions", "Должность");
            dataGridView1.Columns.Add("factor_of_utility", "Ставка");
            dataGridView1.Columns.Add("graphic", "График работы");
            dataGridView1.Columns.Add("number_of_working_days", "Количество рабочих дней");
            dataGridView1.Columns.Add("date_of_admission", "Дата начала работы");
            dataGridView1.Columns.Add("date_of_dismissal", "Дата увольнения");
            dataGridView1.Columns.Add("worked_hours", "Часов отработано");
            dataGridView1.Columns.Add("IsNew", String.Empty);
            dataGridView1.Columns[1].Width = 190;
            dataGridView1.Columns[3].Width = 180;
            dataGridView1.Columns[5].Width = 150;
            dataGridView1.Columns[6].Width = 130;
            dataGridView1.Columns[7].Width = 130;
        }

        private void ReadSingleRow(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetInt32(2), record.GetString(3), record.GetString(4), record.GetString(5), record.GetInt32(6), record.GetDateTime(7), record.GetDateTime(8), record.GetString(9), RowState.ModifiedNew);

        }

        private void RefreshDataGrid(DataGridView dwg)
        {
            dwg.Rows.Clear();
            MySqlCommand command = new MySqlCommand("select idstaff, name, work_experience, positions.title, factor_of_utility, concat_ws(\" до \",work_schedule_from, work_schedule_to) as 'graphic', number_of_working_days, date_of_admission, date_of_dismissal, staff_accounts.worked_hours from staff\r\njoin positions on staff.id_positions = positions.idpositions \r\njoin staff_accounts on staff.idstaff = staff_accounts.idstaff_accounts\r\nwhere date_of_dismissal is not null", db.getConnection());
            db.openConnection();

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dwg, reader);
            }
            reader.Close();
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
            dataGridView1.ClearSelection();

            string myConnectionString = "Database=semestrovaya;Data Source=127.0.0.1;User Id=root;Password=1337";

            DataTable table_completed_orders = new DataTable();

            MySqlConnection connection_completed_orders = new MySqlConnection(myConnectionString);
            {
                MySqlCommand command = new MySqlCommand("select idorders, title from orders\r\njoin staff_to_completed_orders on staff_to_completed_orders.id_orders = orders.idorders\r\njoin staff on staff_to_completed_orders.id_staff = staff.idstaff join staff_accounts on staff_accounts.idstaff_accounts = staff.idstaff where staff.idstaff = @id", connection_completed_orders);
                command.Parameters.Add("@id", MySqlDbType.VarChar).Value = 0;
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(table_completed_orders);
            }
            listBox1.DataSource = table_completed_orders;
            listBox1.DisplayMember = "title";
            listBox1.ValueMember = "idorders";
            listBox1.Refresh();

            textBox1.Text = "";
        }

        private void Search(DataGridView dgw)
        {

            dgw.Rows.Clear();
            MySqlCommand command = new MySqlCommand("select idstaff, name, work_experience, positions.title, factor_of_utility, concat_ws(\" до \",work_schedule_from, work_schedule_to) as 'graphic', number_of_working_days, date_of_admission, date_of_dismissal, staff_accounts.worked_hours from staff\r\njoin positions on staff.id_positions = positions.idpositions \r\njoin staff_accounts on staff.idstaff = staff_accounts.idstaff_accounts\r\n where staff.name like concat ('%', @title, '%') and date_of_dismissal is not null", db.getConnection());
            command.Parameters.Add("@title", MySqlDbType.VarChar).Value = textBox2.Text;

            db.openConnection();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();

        }

        private void button_search_Click(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            String i, r, orders_count;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                i = row.Cells[0].Value.ToString();
                r = row.Cells[1].Value.ToString();

                j = Convert.ToInt32(i);
                m = r;
                string myConnectionString = "Database=semestrovaya;Data Source=127.0.0.1;User Id=root;Password=1337";

                DataTable table_completed_orders = new DataTable();

                MySqlConnection connection_completed_orders = new MySqlConnection(myConnectionString);
                {
                    MySqlCommand command = new MySqlCommand("select idorders, title from orders\r\njoin staff_to_completed_orders on staff_to_completed_orders.id_orders = orders.idorders\r\njoin staff on staff_to_completed_orders.id_staff = staff.idstaff join staff_accounts on staff_accounts.idstaff_accounts = staff.idstaff where staff.idstaff = @id", connection_completed_orders);
                    command.Parameters.Add("@id", MySqlDbType.VarChar).Value = j;
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    adapter.Fill(table_completed_orders);
                }

                listBox1.DataSource = table_completed_orders;
                listBox1.DisplayMember = "title";
                listBox1.ValueMember = "idorders";
                listBox1.Refresh();

                MySqlConnection orders_count_connection = new MySqlConnection(myConnectionString);
                MySqlCommand command_max = new MySqlCommand("select count(*) from staff_to_completed_orders\r\njoin staff on staff_to_completed_orders.id_staff = staff.idstaff\r\njoin orders on staff_to_completed_orders.id_orders = orders.idorders\r\nwhere staff.idstaff = @id", orders_count_connection);
                command_max.Parameters.Add("@id", MySqlDbType.VarChar).Value = j;
                orders_count_connection.Open();

                orders_count = command_max.ExecuteScalar().ToString();
                textBox1.Text = orders_count;
                orders_count_connection.Close();


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (m != null)
            {
                db.openConnection();
                MySqlCommand command = new MySqlCommand("update staff \r\nset date_of_dismissal = null where idstaff = @idstaff", db.getConnection());
                command.Parameters.Add("@idstaff", MySqlDbType.VarChar).Value = j;
                command.ExecuteNonQuery();

                MessageBox.Show("Сотрудник '" + m + "' был восстановлен\n\n\t\t" + DateTime.Now);
                RefreshDataGrid(dataGridView1);
                db.closeConnection();
                dataGridView1.RefreshEdit();
                m = null;
                dataGridView1.ClearSelection();

                string myConnectionString = "Database=semestrovaya;Data Source=127.0.0.1;User Id=root;Password=1337";

                DataTable table_completed_orders = new DataTable();

                MySqlConnection connection_completed_orders = new MySqlConnection(myConnectionString);
                {
                    MySqlCommand command2 = new MySqlCommand("select idorders, title from orders\r\njoin staff_to_completed_orders on staff_to_completed_orders.id_orders = orders.idorders\r\njoin staff on staff_to_completed_orders.id_staff = staff.idstaff join staff_accounts on staff_accounts.idstaff_accounts = staff.idstaff where staff.idstaff = @id", connection_completed_orders);
                    command2.Parameters.Add("@id", MySqlDbType.VarChar).Value = 0;
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command2);
                    adapter.Fill(table_completed_orders);
                }
                listBox1.DataSource = table_completed_orders;
                listBox1.DisplayMember = "title";
                listBox1.ValueMember = "idorders";
                listBox1.Refresh();

                textBox1.Text = "";
            }
            else
                MessageBox.Show("Ни один сотрудник не выбран");
        }
    }
}
