using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
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
    enum RowState
    {
        Existed,
        New,
        Modified,
        ModifiedNew,
        Deleted
    }
    public partial class employee_report : Form
    {
        DataBase db = new DataBase();
        int selectedRow;

        int k = 0;
        int j;
        String m;

        public employee_report()
        {
            InitializeComponent();
            radioButton_zp.Checked = true;
            StartPosition = FormStartPosition.CenterScreen; 
        }

        private void employee_report_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
            dataGridView1.Columns[11].Visible = false;
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
            dataGridView1.Columns.Add("cash", "Счёт RUB");
            dataGridView1.Columns.Add("worked_hours", "Часов отработано");
            dataGridView1.Columns.Add("bool", "Начисление заработной платы");
            dataGridView1.Columns.Add("IsNew", String.Empty);
            dataGridView1.Columns[1].Width = 190;
            dataGridView1.Columns[3].Width = 180;
            dataGridView1.Columns[5].Width = 150;
            dataGridView1.Columns[6].Width = 130;
            dataGridView1.Columns[7].Width = 130;
        }

        private void ReadSingleRow(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetInt32(2), record.GetString(3), record.GetString(4), record.GetString(5), record.GetInt32(6), record.GetDateTime(7), record.GetString(8), record.GetString(9), record.GetBoolean(10), RowState.ModifiedNew);

        }

        private void RefreshDataGrid(DataGridView dwg)
        {
            dwg.Rows.Clear();
            MySqlCommand command = new MySqlCommand("select idstaff, name, work_experience, positions.title, factor_of_utility, concat_ws(\" до \",work_schedule_from, work_schedule_to) as 'graphic', number_of_working_days, date_of_admission, staff_accounts.cash, staff_accounts.worked_hours, staff_accounts.payment_of_wages from staff\r\njoin positions on staff.id_positions = positions.idpositions \r\njoin staff_accounts on staff.idstaff = staff_accounts.idstaff_accounts\r\nwhere date_of_dismissal is null", db.getConnection());
            db.openConnection();

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dwg, reader);
            }
            reader.Close();
        }

        private void Search(DataGridView dgw)
        {

            dgw.Rows.Clear();
            MySqlCommand command = new MySqlCommand("select idstaff, name, work_experience, positions.title, factor_of_utility, concat_ws(\" до \",work_schedule_from, work_schedule_to) as 'graphic', number_of_working_days, date_of_admission, staff_accounts.cash, staff_accounts.worked_hours, staff_accounts.payment_of_wages from staff\r\njoin positions on staff.id_positions = positions.idpositions join staff_accounts on staff.idstaff = staff_accounts.idstaff_accounts where staff.name like concat ('%', @title, '%') and date_of_dismissal is null", db.getConnection());
            command.Parameters.Add("@title", MySqlDbType.VarChar).Value = textBox2.Text;

            db.openConnection();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void button_search_Click_1(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void button_refresh_Click_1(object sender, EventArgs e)
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

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            add_employee frm1 = new add_employee();
            this.Hide();
            frm1.ShowDialog();
            this.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dismissed_employees frm1 = new dismissed_employees();
            this.Hide();
            frm1.ShowDialog();
            this.Show();
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

        private void button1_Click(object sender, EventArgs e)
        {
            if(checkBox_remember_me.Checked)
            {
                if(radioButton_zp.Checked)
                {
                    db.openConnection();
                    MySqlCommand command = new MySqlCommand("update staff_accounts \r\njoin staff on staff_accounts.idstaff_accounts = staff.idstaff\r\njoin positions on staff.id_positions = positions.idpositions\r\nset cash = cash + ((staff_accounts.worked_hours / ((hour((staff.work_schedule_to) - (work_schedule_from)) + minute((staff.work_schedule_to) - (work_schedule_from)) / 60) * staff.number_of_working_days)) * \r\npositions.salary * staff.factor_of_utility),\r\npayment_of_wages = 1 \r\nwhere payment_of_wages = 0", db.getConnection());

                    command.ExecuteNonQuery();
                   
                    MessageBox.Show("Начисление заработной платы всем сотрудникам");
                    RefreshDataGrid(dataGridView1);
                    db.closeConnection();

                }
                if (radioButton_orders.Checked)
                {
                    DataTable table = new DataTable();
                    MySqlDataAdapter adapter = new MySqlDataAdapter();

                    db.openConnection();
                    MySqlCommand command = new MySqlCommand("update staff_accounts\r\njoin staff_to_completed_orders on staff_accounts.idstaff_accounts = staff_to_completed_orders.id_staff\r\nset orders_cash = orders_cash +\r\n(select sum(orders.salary) from staff_to_completed_orders\r\njoin staff on staff.idstaff = staff_to_completed_orders.id_staff\r\njoin orders on staff_to_completed_orders.id_orders = orders.idorders\r\nwhere staff_to_completed_orders.id_staff = staff_accounts.idstaff_accounts and not exists (select 1 from paid_orders \r\nwhere paid_orders.id_orders = staff_to_completed_orders.id_orders and paid_orders.id_staff = staff_to_completed_orders.id_staff)),\r\ncash = cash + orders_cash,\r\norders_cash = 0\r\nwhere not exists (select 1 from paid_orders \r\nwhere paid_orders.id_orders = staff_to_completed_orders.id_orders and paid_orders.id_staff = staff_to_completed_orders.id_staff)", db.getConnection());
                    MySqlCommand command_2 = new MySqlCommand("INSERT INTO paid_orders(id_orders, id_staff, time_of_paid) SELECT id_orders, id_staff, now() FROM staff_to_completed_orders \r\nwhere not exists (select 1 from paid_orders \r\nwhere paid_orders.id_orders = staff_to_completed_orders.id_orders and paid_orders.id_staff = staff_to_completed_orders.id_staff)", db.getConnection());

                    command.ExecuteNonQuery();
                    
                    adapter.SelectCommand = command_2;
                    adapter.Fill(table);

                    MessageBox.Show("Начисление выплат за заказы всем сотрудникам");
                    RefreshDataGrid(dataGridView1);
                    db.closeConnection();
                    

                }
                if (radioButton_orders_and_zp.Checked)
                {
                    DataTable table = new DataTable();
                    MySqlDataAdapter adapter = new MySqlDataAdapter();

                    db.openConnection();
                    MySqlCommand command = new MySqlCommand("update staff_accounts \r\njoin staff on staff_accounts.idstaff_accounts = staff.idstaff\r\njoin positions on staff.id_positions = positions.idpositions\r\nset cash = cash + ((staff_accounts.worked_hours / ((hour((staff.work_schedule_to) - (work_schedule_from)) + minute((staff.work_schedule_to) - (work_schedule_from)) / 60) * staff.number_of_working_days)) * \r\npositions.salary * staff.factor_of_utility),\r\npayment_of_wages = 1 \r\nwhere payment_of_wages = 0", db.getConnection());
                    MySqlCommand command_1 = new MySqlCommand("update staff_accounts\r\njoin staff_to_completed_orders on staff_accounts.idstaff_accounts = staff_to_completed_orders.id_staff\r\nset orders_cash = orders_cash +\r\n(select sum(orders.salary) from staff_to_completed_orders\r\njoin staff on staff.idstaff = staff_to_completed_orders.id_staff\r\njoin orders on staff_to_completed_orders.id_orders = orders.idorders\r\nwhere staff_to_completed_orders.id_staff = staff_accounts.idstaff_accounts and not exists (select 1 from paid_orders \r\nwhere paid_orders.id_orders = staff_to_completed_orders.id_orders and paid_orders.id_staff = staff_to_completed_orders.id_staff)),\r\ncash = cash + orders_cash,\r\norders_cash = 0\r\nwhere not exists (select 1 from paid_orders \r\nwhere paid_orders.id_orders = staff_to_completed_orders.id_orders and paid_orders.id_staff = staff_to_completed_orders.id_staff)", db.getConnection());
                    MySqlCommand command_2 = new MySqlCommand("INSERT INTO paid_orders(id_orders, id_staff, time_of_paid) SELECT id_orders, id_staff, now() FROM staff_to_completed_orders \r\nwhere not exists (select 1 from paid_orders \r\nwhere paid_orders.id_orders = staff_to_completed_orders.id_orders and paid_orders.id_staff = staff_to_completed_orders.id_staff)", db.getConnection());


                    command.ExecuteNonQuery();
                    command_1.ExecuteNonQuery();
                    adapter.SelectCommand = command_2;
                    adapter.Fill(table);

                    MessageBox.Show("Начисление заработной платы и выплат за заказы всем сотрудникам");
                    RefreshDataGrid(dataGridView1);
                    db.closeConnection();

                }
            }
            else
            {
                if (radioButton_zp.Checked)
                {
                    db.openConnection();
                    MySqlCommand command = new MySqlCommand("update staff_accounts \r\njoin staff on staff_accounts.idstaff_accounts = staff.idstaff\r\njoin positions on staff.id_positions = positions.idpositions\r\nset cash = cash + ((staff_accounts.worked_hours / ((hour((staff.work_schedule_to) - (work_schedule_from)) + minute((staff.work_schedule_to) - (work_schedule_from)) / 60) * staff.number_of_working_days)) * \r\npositions.salary * staff.factor_of_utility),\r\npayment_of_wages = 1 \r\nwhere payment_of_wages = 0 and staff.idstaff = @idstaff", db.getConnection());
                    command.Parameters.Add("@idstaff", MySqlDbType.VarChar).Value = j;
                    command.ExecuteNonQuery();

                    MessageBox.Show("Начисление заработной платы сотруднику:\n" + m);
                    RefreshDataGrid(dataGridView1);
                    db.closeConnection();

                }
                if (radioButton_orders.Checked)
                {
                    DataTable table = new DataTable();
                    MySqlDataAdapter adapter = new MySqlDataAdapter();

                    db.openConnection();
                    MySqlCommand command = new MySqlCommand("update staff_accounts\r\njoin staff_to_completed_orders on staff_accounts.idstaff_accounts = staff_to_completed_orders.id_staff\r\nset orders_cash = orders_cash +\r\n(select sum(orders.salary) from staff_to_completed_orders\r\njoin staff on staff.idstaff = staff_to_completed_orders.id_staff\r\njoin orders on staff_to_completed_orders.id_orders = orders.idorders\r\nwhere staff_to_completed_orders.id_staff = staff_accounts.idstaff_accounts and not exists (select 1 from paid_orders \r\nwhere paid_orders.id_orders = staff_to_completed_orders.id_orders and paid_orders.id_staff = staff_to_completed_orders.id_staff)),\r\ncash = cash + orders_cash,\r\norders_cash = 0\r\nwhere not exists (select 1 from paid_orders \r\nwhere paid_orders.id_orders = staff_to_completed_orders.id_orders and paid_orders.id_staff = staff_to_completed_orders.id_staff) and staff_accounts.idstaff_accounts = @idstaff", db.getConnection());
                    command.Parameters.Add("@idstaff", MySqlDbType.VarChar).Value = j;
                    MySqlCommand command_2 = new MySqlCommand("INSERT INTO paid_orders(id_orders, id_staff, time_of_paid) SELECT id_orders, id_staff, now() FROM staff_to_completed_orders \r\nwhere not exists (select 1 from paid_orders \r\nwhere paid_orders.id_orders = staff_to_completed_orders.id_orders and paid_orders.id_staff = staff_to_completed_orders.id_staff) and staff_to_completed_orders.id_staff = @idstaff", db.getConnection());
                    command_2.Parameters.Add("@idstaff", MySqlDbType.VarChar).Value = j;

                    command.ExecuteNonQuery();

                    adapter.SelectCommand = command_2;
                    adapter.Fill(table);

                    MessageBox.Show("Начисление выплат за заказы сотруднику:\n" + m);
                    RefreshDataGrid(dataGridView1);
                    db.closeConnection();
                }
                if (radioButton_orders_and_zp.Checked)
                {
                    DataTable table = new DataTable();
                    MySqlDataAdapter adapter = new MySqlDataAdapter();

                    db.openConnection();
                    MySqlCommand command = new MySqlCommand("update staff_accounts \r\njoin staff on staff_accounts.idstaff_accounts = staff.idstaff\r\njoin positions on staff.id_positions = positions.idpositions\r\nset cash = cash + ((staff_accounts.worked_hours / ((hour((staff.work_schedule_to) - (work_schedule_from)) + minute((staff.work_schedule_to) - (work_schedule_from)) / 60) * staff.number_of_working_days)) * \r\npositions.salary * staff.factor_of_utility),\r\npayment_of_wages = 1 \r\nwhere payment_of_wages = 0 and staff.idstaff = @idstaff", db.getConnection());
                    command.Parameters.Add("@idstaff", MySqlDbType.VarChar).Value = j;
                    MySqlCommand command_1 = new MySqlCommand("update staff_accounts\r\njoin staff_to_completed_orders on staff_accounts.idstaff_accounts = staff_to_completed_orders.id_staff\r\nset orders_cash = orders_cash +\r\n(select sum(orders.salary) from staff_to_completed_orders\r\njoin staff on staff.idstaff = staff_to_completed_orders.id_staff\r\njoin orders on staff_to_completed_orders.id_orders = orders.idorders\r\nwhere staff_to_completed_orders.id_staff = staff_accounts.idstaff_accounts and not exists (select 1 from paid_orders \r\nwhere paid_orders.id_orders = staff_to_completed_orders.id_orders and paid_orders.id_staff = staff_to_completed_orders.id_staff)),\r\ncash = cash + orders_cash,\r\norders_cash = 0\r\nwhere not exists (select 1 from paid_orders \r\nwhere paid_orders.id_orders = staff_to_completed_orders.id_orders and paid_orders.id_staff = staff_to_completed_orders.id_staff) and staff_accounts.idstaff_accounts = @idstaff", db.getConnection());
                    command_1.Parameters.Add("@idstaff", MySqlDbType.VarChar).Value = j;
                    MySqlCommand command_2 = new MySqlCommand("INSERT INTO paid_orders(id_orders, id_staff, time_of_paid) SELECT id_orders, id_staff, now() FROM staff_to_completed_orders \r\nwhere not exists (select 1 from paid_orders \r\nwhere paid_orders.id_orders = staff_to_completed_orders.id_orders and paid_orders.id_staff = staff_to_completed_orders.id_staff) and staff_to_completed_orders.id_staff = @idstaff", db.getConnection());
                    command_2.Parameters.Add("@idstaff", MySqlDbType.VarChar).Value = j;


                    command.ExecuteNonQuery();
                    command_1.ExecuteNonQuery();
                    adapter.SelectCommand = command_2;
                    adapter.Fill(table);

                    MessageBox.Show("Начисление заработной платы и выплат сотруднику:\n" + m);
                    RefreshDataGrid(dataGridView1);
                    db.closeConnection();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (m != null)
            {
                db.openConnection();
                MySqlCommand command = new MySqlCommand("update staff \r\nset date_of_dismissal = now() where idstaff = @idstaff", db.getConnection());
                command.Parameters.Add("@idstaff", MySqlDbType.VarChar).Value = j;
                command.ExecuteNonQuery();

                MessageBox.Show("Сотрудник '" + m + "' был уволен\n\n\t\t" + DateTime.Now);
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

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void checkBox_remember_me_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox_remember_me.Checked)
            {
                MessageBox.Show("Выбраны все сотрудники");
            }
            
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

        }

        private void dataGridView1_RowLeave(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
