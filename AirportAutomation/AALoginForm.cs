using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;
using System.Data.Common;

namespace AirportAutomation
{
    public partial class AALoginForm : Form
    {
        public AALoginForm()
        {
            InitializeComponent();

            Globals.Connection.Open();
        }

        private void btnAdminLogin_Click(object sender, EventArgs e)
        {
            string username = txtAdminUsername.Text;
            string password = txtAdminPassword.Text;

            MySqlCommand cmd = new MySqlCommand($"select * from admins where username = '{username}' and password = '{password}';", Globals.Connection);
            var result = cmd.ExecuteReader();
            if (!result.HasRows)
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                result.Close();
                return;
            }
            result.Close();

            Globals.ConnectedAdminUsername = username;
            Globals.ConnectedAdminPassword = password;

            AAMasterAdminPanel panel = new AAMasterAdminPanel();
            if (panel.IsDisposed)
            {
                MessageBox.Show("Application internal error, Form is disposed before it is initialized!");
                Close();
                return;
            }

            panel.Show();

            Hide();
        }
    }
}
