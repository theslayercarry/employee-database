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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace semestrovaya_2
{
    
    public partial class staff_account : Form
    {
        int i = 0;
        int j = 0;

        public static int idstaff_int;

        DataBase db = new DataBase();
        public staff_account()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            string myConnectionString = "Database=semestrovaya;Data Source=127.0.0.1;User Id=root;Password=1337";


            DataTable table_active_orders = new DataTable();
            DataTable table_completed_orders = new DataTable();

            MySqlConnection connection_active_orders = new MySqlConnection(myConnectionString);
            {
                MySqlCommand command = new MySqlCommand("select idorders, title from orders\r\njoin staff_to_taken_orders on staff_to_taken_orders.id_orders = orders.idorders\r\njoin staff on staff_to_taken_orders.id_staff = staff.idstaff \r\njoin staff_accounts on staff_accounts.idstaff_accounts = staff.idstaff \r\nwhere not exists (select 1 from staff_to_completed_orders where staff_to_completed_orders.id_orders = staff_to_taken_orders.id_orders and staff_to_completed_orders.id_staff = staff_to_taken_orders.id_staff)\r\n and staff_accounts.login = @uL and staff_accounts.password = @uP", connection_active_orders);
                command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = Login.loginUser;
                command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = Login.passUser;
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(table_active_orders);
            }
            listBox_active_orders.DataSource = table_active_orders;
            listBox_active_orders.DisplayMember = "title";
            listBox_active_orders.ValueMember = "idorders";
        

            MySqlConnection connection_completed_orders = new MySqlConnection(myConnectionString);
            {
                MySqlCommand command = new MySqlCommand("select idorders, title from orders\r\njoin staff_to_completed_orders on staff_to_completed_orders.id_orders = orders.idorders\r\njoin staff on staff_to_completed_orders.id_staff = staff.idstaff join staff_accounts on staff_accounts.idstaff_accounts = staff.idstaff where staff_accounts.login = @uL and staff_accounts.password = @uP", connection_completed_orders);
                command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = Login.loginUser;
                command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = Login.passUser;
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(table_completed_orders);
            }

            listBox_completed_orders.DataSource = table_completed_orders;
            listBox_completed_orders.DisplayMember = "title";
            listBox_completed_orders.ValueMember = "idorders";
        }
        private void staff_account_Load(object sender, EventArgs e)
        {
            CreateColumns1();
            RefreshDataGrid1(dataGridView1);
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ClearSelection();
            CreateColumns2();
            RefreshDataGrid2(dataGridView2);
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.ClearSelection();
            CreateColumns3();
            RefreshDataGrid3(dataGridView3);
            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView3.ClearSelection();
            CreateColumns4();
            RefreshDataGrid4(dataGridView4);
            dataGridView4.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView4.ClearSelection();
            CreateColumns5();
            RefreshDataGrid5(dataGridView5);
            dataGridView5.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView5.ClearSelection();
            CreateColumns8();
            RefreshDataGrid8(dataGridView6);
            dataGridView6.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView6.ClearSelection();
            CreateColumns9();
            RefreshDataGrid9(dataGridView9);
            dataGridView9.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView9.ClearSelection();

            pictureBox_close_more.Visible = false;
            panel_more.Visible = false;



            string myConnectionString = "Database=semestrovaya;Data Source=127.0.0.1;User Id=root;Password=1337";

            MySqlConnection orders_staff_connection = new MySqlConnection(myConnectionString);
            MySqlCommand command_id_staff = new MySqlCommand("select idstaff from staff\r\njoin staff_accounts on staff_accounts.idstaff_accounts = staff.idstaff\r\nwhere staff_accounts.login = @uL and staff_accounts.password = @uP", orders_staff_connection);
            command_id_staff.Parameters.Add("@uL", MySqlDbType.VarChar).Value = Login.loginUser;
            command_id_staff.Parameters.Add("@uP", MySqlDbType.VarChar).Value = Login.passUser;
            orders_staff_connection.Open();

            String idstaff;

            idstaff = command_id_staff.ExecuteScalar().ToString();
            idstaff_int = Convert.ToInt32(idstaff);
            orders_staff_connection.Close();

        }
        private void CreateColumns1()
        {
            dataGridView1.Columns.Add("name", "ФИО");
            dataGridView1.Columns[0].Width = 412;
        }
        private void CreateColumns2()
        {
            dataGridView2.Columns.Add("position", "Должность");
            dataGridView2.Columns[0].Width = 336;
        }
        private void CreateColumns3()
        {
            dataGridView3.Columns.Add("city", "Город");
            dataGridView3.Columns[0].Width = 336;
        }
        private void CreateColumns4()
        {
            dataGridView4.Columns.Add("phone", "Номер телефона");
            dataGridView4.Columns[0].Width = 300;
        }
        private void CreateColumns5()
        {
            dataGridView5.Columns.Add("email", "Почта");
            dataGridView5.Columns[0].Width = 300;
        }

        private void CreateColumns6()
        {
            dataGridView8.Columns.Add("description", "Описание");
            dataGridView8.Columns.Add("taking_time", "Время взятия заказа");
            dataGridView8.Columns[0].Width = 329;
        }
        private void CreateColumns7()
        {
            dataGridView7.Columns.Add("description", "Описание");
            dataGridView7.Columns.Add("completed_time","Время завершения заказа");
            dataGridView7.Columns[0].Width = 329;
        }

        private void CreateColumns8()
        {
            dataGridView6.Columns.Add("cash", "Счёт");
            dataGridView6.Columns[0].Width = 154;
        }

        private void CreateColumns9()
        {
            dataGridView9.Columns.Add("worked hours", "Часы");
            dataGridView9.Columns[0].Width = 93;
        }

        private void ReadSingleRow1(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetString(0));

        }
        private void ReadSingleRow2(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetString(0));

        }
        private void ReadSingleRow3(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetString(0));

        }
        private void ReadSingleRow4(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetString(0));

        }
        private void ReadSingleRow5(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetString(0));

        }

        private void ReadSingleRow6(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetString(0), record.GetString(1));

        }
        private void ReadSingleRow7(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetString(0), record.GetString(1));

        }

        private void ReadSingleRow8(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetString(0));

        }

        private void ReadSingleRow9(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetString(0));

        }

        private void RefreshDataGrid1(DataGridView dwg)
        {
            dwg.Rows.Clear();
            MySqlCommand command = new MySqlCommand("select name from staff\r\njoin staff_accounts on staff_accounts.idstaff_accounts = staff.idstaff\r\nwhere staff_accounts.login = @login and staff_accounts.password = @password", db.getConnection());
            command.Parameters.Add("@login", MySqlDbType.VarChar).Value = Login.loginUser;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = Login.passUser;
            db.openConnection();

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow1(dwg, reader);
            }
            reader.Close();
        }

        private void RefreshDataGrid2(DataGridView dwg)
        {
            dwg.Rows.Clear();
            MySqlCommand command = new MySqlCommand("select positions.title from staff\r\njoin staff_accounts on staff_accounts.idstaff_accounts = staff.idstaff\r\njoin positions on staff.id_positions = positions.idpositions where staff_accounts.login = @login and staff_accounts.password = @password", db.getConnection());
            command.Parameters.Add("@login", MySqlDbType.VarChar).Value = Login.loginUser;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = Login.passUser;
            db.openConnection();

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow2(dwg, reader);
            }
            reader.Close();
        }
        private void RefreshDataGrid3(DataGridView dwg)
        {
            dwg.Rows.Clear();
            MySqlCommand command = new MySqlCommand("select staff_accounts.city from staff\r\njoin staff_accounts on staff_accounts.idstaff_accounts = staff.idstaff\r\nwhere staff_accounts.login = @login and staff_accounts.password = @password", db.getConnection());
            command.Parameters.Add("@login", MySqlDbType.VarChar).Value = Login.loginUser;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = Login.passUser;
            db.openConnection();

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow3(dwg, reader);
            }
            reader.Close();
        }
        private void RefreshDataGrid4(DataGridView dwg)
        {
            dwg.Rows.Clear();
            MySqlCommand command = new MySqlCommand("select staff_accounts.phone from staff\r\njoin staff_accounts on staff_accounts.idstaff_accounts = staff.idstaff\r\nwhere staff_accounts.login = @login and staff_accounts.password = @password", db.getConnection());
            command.Parameters.Add("@login", MySqlDbType.VarChar).Value = Login.loginUser;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = Login.passUser;
            db.openConnection();

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow4(dwg, reader);
            }
            reader.Close();
        }
        private void RefreshDataGrid5(DataGridView dwg)
        {
            dwg.Rows.Clear();
            MySqlCommand command = new MySqlCommand("select staff_accounts.email from staff\r\njoin staff_accounts on staff_accounts.idstaff_accounts = staff.idstaff\r\nwhere staff_accounts.login = @login and staff_accounts.password = @password", db.getConnection());
            command.Parameters.Add("@login", MySqlDbType.VarChar).Value = Login.loginUser;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = Login.passUser;
            db.openConnection();

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow5(dwg, reader);
            }
            reader.Close();
        }

        private void RefreshDataGrid6(DataGridView dwg)
        {
            dwg.Rows.Clear();
            MySqlCommand command = new MySqlCommand("select orders.description, time_of_taking from staff_to_taken_orders\r\njoin orders on staff_to_taken_orders.id_orders = orders.idorders\r\njoin staff_accounts on staff_to_taken_orders.id_staff = staff_accounts.idstaff_accounts\r\nwhere orders.idorders = @idorder and staff_accounts.login = @login and staff_accounts.password = @password", db.getConnection());
            command.Parameters.Add("@idorder", MySqlDbType.VarChar).Value = listBox_active_orders.SelectedValue;
            command.Parameters.Add("@login", MySqlDbType.VarChar).Value = Login.loginUser;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = Login.passUser;
            db.openConnection();

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow6(dwg, reader);
            }
            reader.Close();
        }

        private void RefreshDataGrid7(DataGridView dwg)
        {
            dwg.Rows.Clear();
            MySqlCommand command = new MySqlCommand("select orders.description, time_of_completion from staff_to_completed_orders\r\njoin orders on staff_to_completed_orders.id_orders = orders.idorders\r\njoin staff_accounts on staff_to_completed_orders.id_staff = staff_accounts.idstaff_accounts\r\nwhere orders.idorders = @idorder and staff_accounts.login = @login and staff_accounts.password = @password", db.getConnection());
            command.Parameters.Add("@idorder", MySqlDbType.VarChar).Value = listBox_completed_orders.SelectedValue;
            command.Parameters.Add("@login", MySqlDbType.VarChar).Value = Login.loginUser;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = Login.passUser;
            db.openConnection();

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow7(dwg, reader);
            }
            reader.Close();
        }

        private void RefreshDataGrid8(DataGridView dwg)
        {
            dwg.Rows.Clear();
            MySqlCommand command = new MySqlCommand("select staff_accounts.cash from staff_accounts\r\njoin staff on staff_accounts.idstaff_accounts = staff.idstaff\r\nwhere staff_accounts.login = @login and staff_accounts.password = @password", db.getConnection());
            command.Parameters.Add("@login", MySqlDbType.VarChar).Value = Login.loginUser;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = Login.passUser;
            db.openConnection();

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow8(dwg, reader);
            }
            reader.Close();
        }

        private void RefreshDataGrid9(DataGridView dwg)
        {
            dwg.Rows.Clear();
            MySqlCommand command = new MySqlCommand("select staff_accounts.worked_hours from staff_accounts\r\njoin staff on staff_accounts.idstaff_accounts = staff.idstaff\r\nwhere staff_accounts.login = @login and staff_accounts.password = @password", db.getConnection());
            command.Parameters.Add("@login", MySqlDbType.VarChar).Value = Login.loginUser;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = Login.passUser;
            db.openConnection();

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow9(dwg, reader);
            }
            reader.Close();
        }


        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ((DataGridView)sender).SelectedCells[0].Selected = false;
            }
            catch { }
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ((DataGridView)sender).SelectedCells[0].Selected = false;
            }
            catch { }
        }

        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ((DataGridView)sender).SelectedCells[0].Selected = false;
            }
            catch { }
        }

        private void dataGridView4_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ((DataGridView)sender).SelectedCells[0].Selected = false;
            }
            catch { }
        }

        private void dataGridView5_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ((DataGridView)sender).SelectedCells[0].Selected = false;
            }
            catch { }
        }

        private void listBox_completed_orders_SelectedIndexChanged(object sender, EventArgs e)
        {
 
        }

        private void listBox_active_orders_MouseClick(object sender, MouseEventArgs e)
        {
            if (i++ == 0)
            {
                CreateColumns6();
            }
            RefreshDataGrid6(dataGridView8);
            dataGridView8.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView8.ClearSelection();
        }

        private void listBox_completed_orders_MouseClick(object sender, MouseEventArgs e)
        {
            if (j++ == 0)
            {
                CreateColumns7();
            }
            RefreshDataGrid7(dataGridView7);
            dataGridView7.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView7.ClearSelection();
        }

        private void listBox_active_orders_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView8_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ((DataGridView)sender).SelectedCells[0].Selected = false;
            }
            catch { }
        }

        private void dataGridView7_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ((DataGridView)sender).SelectedCells[0].Selected = false;
            }
            catch { }
        }

        private void dataGridView6_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ((DataGridView)sender).SelectedCells[0].Selected = false;
            }
            catch { }
        }

        private void dataGridView9_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ((DataGridView)sender).SelectedCells[0].Selected = false;
            }
            catch { }
        }

        private void label6_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void label5_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label_taking_order_Click(object sender, EventArgs e)
        {
            taking_orders frm1 = new taking_orders();
            this.Hide();
            frm1.ShowDialog();

            string myConnectionString = "Database=semestrovaya;Data Source=127.0.0.1;User Id=root;Password=1337";


            DataTable table_active_orders = new DataTable();

            MySqlConnection connection_active_orders = new MySqlConnection(myConnectionString);
            {
                MySqlCommand command = new MySqlCommand("select idorders, title from orders\r\njoin staff_to_taken_orders on staff_to_taken_orders.id_orders = orders.idorders\r\njoin staff on staff_to_taken_orders.id_staff = staff.idstaff \r\njoin staff_accounts on staff_accounts.idstaff_accounts = staff.idstaff \r\nwhere not exists (select 1 from staff_to_completed_orders where staff_to_completed_orders.id_orders = staff_to_taken_orders.id_orders and staff_to_completed_orders.id_staff = staff_to_taken_orders.id_staff)\r\n and staff_accounts.login = @uL and staff_accounts.password = @uP", connection_active_orders);
                command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = Login.loginUser;
                command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = Login.passUser;
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(table_active_orders);
            }
            listBox_active_orders.DataSource = table_active_orders;
            listBox_active_orders.DisplayMember = "title";
            listBox_active_orders.ValueMember = "idorders";

            dataGridView8.Rows.Clear();

            this.Show();

        }

        private void label_closing_order_Click(object sender, EventArgs e)
        {
            closing_orders frm1 = new closing_orders();
            this.Hide();
            frm1.ShowDialog();

            string myConnectionString = "Database=semestrovaya;Data Source=127.0.0.1;User Id=root;Password=1337";


            DataTable table_active_orders = new DataTable();
            DataTable table_completed_orders = new DataTable();

            MySqlConnection connection_active_orders = new MySqlConnection(myConnectionString);
            {
                MySqlCommand command = new MySqlCommand("select idorders, title from orders\r\njoin staff_to_taken_orders on staff_to_taken_orders.id_orders = orders.idorders\r\njoin staff on staff_to_taken_orders.id_staff = staff.idstaff \r\njoin staff_accounts on staff_accounts.idstaff_accounts = staff.idstaff \r\nwhere not exists (select 1 from staff_to_completed_orders where staff_to_completed_orders.id_orders = staff_to_taken_orders.id_orders and staff_to_completed_orders.id_staff = staff_to_taken_orders.id_staff)\r\n and staff_accounts.login = @uL and staff_accounts.password = @uP", connection_active_orders);
                command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = Login.loginUser;
                command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = Login.passUser;
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(table_active_orders);
            }
            listBox_active_orders.DataSource = table_active_orders;
            listBox_active_orders.DisplayMember = "title";
            listBox_active_orders.ValueMember = "idorders";


            MySqlConnection connection_completed_orders = new MySqlConnection(myConnectionString);
            {
                MySqlCommand command = new MySqlCommand("select idorders, title from orders\r\njoin staff_to_completed_orders on staff_to_completed_orders.id_orders = orders.idorders\r\njoin staff on staff_to_completed_orders.id_staff = staff.idstaff join staff_accounts on staff_accounts.idstaff_accounts = staff.idstaff where staff_accounts.login = @uL and staff_accounts.password = @uP", connection_completed_orders);
                command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = Login.loginUser;
                command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = Login.passUser;
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(table_completed_orders);
            }

            listBox_completed_orders.DataSource = table_completed_orders;
            listBox_completed_orders.DisplayMember = "title";
            listBox_completed_orders.ValueMember = "idorders";

            dataGridView8.Rows.Clear();
            dataGridView7.Rows.Clear();

            this.Show();
        }

        private void staff_account_MouseClick(object sender, MouseEventArgs e)
        {
            panel_more.Visible = false;
            pictureBox_close_more.Visible = false;
            pictureBox_open_more.Visible = true;
        }

        private void pictureBox_close_more_Click(object sender, EventArgs e)
        {
            pictureBox_close_more.Visible = false;
            pictureBox_open_more.Visible = true;
            panel_more.Visible = false;
        }

        private void pictureBox_open_more_Click(object sender, EventArgs e)
        {
            pictureBox_open_more.Visible = false;
            pictureBox_close_more.Visible = true;
            panel_more.Visible = true;
        }
    }
}
