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

            try
            {
                var conForm = new AAConnectPanel();
                conForm.Show();
                Globals.Connection.Open();
                conForm.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Sunucu ile bağlantı kurulamadı! İnternet bağlantınızın olduğuna emin olun ya da başka bir bağlantı deneyin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Environment.Exit(1);
            }
        }

        private void btnAdminLogin_Click(object sender, EventArgs e)
        {
            string username = txtAdminUsername.Text;
            string password = txtAdminPassword.Text;

            MySqlCommand cmd = new MySqlCommand($"select * from admins where username = '{username}' and password = '{password}';", Globals.Connection);
            var result = cmd.ExecuteReader();
            if (!result.HasRows)
            {
                result.Close();
                cmd = new MySqlCommand($"Select * from airport_admins where username = '{username}' and password = '{password}';", Globals.Connection);
                var rd = cmd.ExecuteReader();
                if (!rd.HasRows)
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    rd.Close();
                    return;
                }
                else
                {
                    rd.Close();
                    cmd = new MySqlCommand($"Select adminID from airport_admins where username = '{username}' and password = '{password}';", Globals.Connection);
                    rd = cmd.ExecuteReader();
                    rd.Read();
                    Globals.ConnectedAdminID = rd.GetInt32(0);
                    Globals.ConnectedAdminUsername = username;
                    Globals.ConnectedAdminPassword = password;
                    rd.Close();
                    cmd = new MySqlCommand($"Select airportID from airports where adminID = {Globals.ConnectedAdminID};", Globals.Connection);
                    var rd2 = cmd.ExecuteReader();
                    rd2.Read();
                    Globals.ConnectedAdminAirportID = rd2.GetInt32(0);
                    rd2.Close();
                    AAAirportAdminPanel panel = new AAAirportAdminPanel();
                    if (panel.IsDisposed)
                    {
                        MessageBox.Show("Application internal error, Form is disposed before it is initialized!");
                        Close();
                        return;
                    }

                    panel.Show();

                    Hide();
                }
                result.Close();
                return;
            }
            else
            {
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

        private void btnEmployeeLogin_Click(object sender, EventArgs e)
        {
            string username = txtEmployeeUsername.Text;
            string password = txtEmployeePassword.Text;

            MySqlCommand cmd = new MySqlCommand($"select * from staff where username = '{username}' and password = '{password}';", Globals.Connection);
            var result = cmd.ExecuteReader();
            if (!result.HasRows)
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                result.Close();
                return;
            }
            result.Close();

            AAStaffPanel panel = new AAStaffPanel();
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
