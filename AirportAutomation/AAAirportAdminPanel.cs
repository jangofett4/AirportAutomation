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
namespace AirportAutomation
{
    public partial class AAAirportAdminPanel : Form
    {
        public AAAirportAdminPanel()
        {
            MySqlCommand cmd = new MySqlCommand($"select * from airport_admins where username = '{ Globals.ConnectedAdminUsername }' and password = '{ Globals.ConnectedAdminPassword }';", Globals.Connection);
            var result = cmd.ExecuteReader();
            if (!result.HasRows)
            {
                MessageBox.Show("Bağlı yönetici yok!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                result.Close();
                Close();
                return;
            }
            result.Close();
      
            InitializeComponent();
        }
        private void RefreshData(object sender, EventArgs e)
        {
            var src = contextRefresh.SourceControl;

            if (src == gridAirlines) RefreshAirlines();
            else if (src == gridPilots) RefreshPilots();
            else if (src == gridPlaneTypes) RefreshPlaneTypes();
            else if (src == gridPlanes) RefreshPlanes();
            else if (src == gridPlaneModels) RefreshPlaneModels();
            else if (src == gridStaff) RefreshStaff();
            else if (src == gridAirports) RefreshAirports();
        }
        #region "Refresh"
        public void RefreshAirlines()
        {
            gridAirlines.Rows.Clear();

            MySqlCommand cmd = new MySqlCommand("select * from airlines", Globals.Connection);
            var result = cmd.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {

                    int id = result.GetInt32(0);
                    string name = result.GetString(1);
                    gridAirlines.Rows.Add(id, name);
                }
            }
            result.Close();
        }

        public void RefreshPilots()
        {
            RefreshAirlines();

            gridPilots.Rows.Clear();

            MySqlCommand cmd = new MySqlCommand("select * from pilotGridView", Globals.Connection);
            var result = cmd.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    int id = result.GetInt32(0);
                    string name = result.GetString(1);
                    string surname = result.GetString(2);
                    string tc = result.GetString(3);
                    int aid = result.GetInt32(4);
                    string aname = result.GetString(5);

                    gridPilots.Rows.Add(id, tc, name, surname, aname, aid);
                }
            }
            result.Close();
        }

        public void RefreshAirports()
        {
            gridAirports.Rows.Clear();

            MySqlCommand cmd = new MySqlCommand("select * from airportGridView", Globals.Connection);
            var result = cmd.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    int id = result.GetInt32(0);
                    string name = result.GetString(1);
                    string cityid = result.GetString(2);
                    string adminid = result.GetString(3);
                    string adminname = result.GetString("admin_name");
                    string cityname = result.GetString(5);
                    gridAirports.Rows.Add(id, name, cityname, adminname, adminid, cityid);
                }
            }
            result.Close();
        }

        public void RefreshPlaneTypes()
        {
            gridPlaneTypes.Rows.Clear();

            MySqlCommand cmd = new MySqlCommand("select * from types", Globals.Connection);
            var result = cmd.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    int id = result.GetInt32(0);
                    string name = result.GetString(1);
                    gridPlaneTypes.Rows.Add(id, name);
                }
            }
            result.Close();
        }

        public void RefreshPlaneModels()
        {
            gridPlaneModels.Rows.Clear();

            MySqlCommand cmd = new MySqlCommand("select * from modelGridView", Globals.Connection);
            var result = cmd.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    int id = result.GetInt32(0);
                    string name = result.GetString(1);
                    int cap = result.GetInt32(2);
                    int type = result.GetInt32(3);
                    string typename = result.GetString(4);

                    gridPlaneModels.Rows.Add(id, name, cap, typename, type);
                }
            }
            result.Close();
        }

        public void RefreshPlanes()
        {
            gridPlanes.Rows.Clear();

            MySqlCommand cmd = new MySqlCommand("select * from planeGridView", Globals.Connection);
            var result = cmd.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    string id = result.GetString(0);
                    string modelName = result.GetString(1);
                    int model = result.GetInt32(2);
                    gridPlanes.Rows.Add(id, modelName, model);
                }
            }
            result.Close();
        }


        public void RefreshStaff()
        {
            gridStaff.Rows.Clear();

            MySqlCommand cmd = new MySqlCommand("select * from staffGridView", Globals.Connection);
            var result = cmd.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    int id = result.GetInt32(0);
                    string name = result.GetString(1);
                    string surname = result.GetString(2);
                    string username = result.GetString(3);
                    string password = result.GetString(4);
                    string tc = result.GetString(5);
                    int airport = result.GetInt32(6);
                    string airportName = result.GetString(7);

                    gridStaff.Rows.Add(id, tc, name, surname, airportName, airport, username, password);
                }
            }
            result.Close();
        }

        public void RefreshFlights()
        {
            gridFlights.Rows.Clear();

            MySqlCommand cmd = new MySqlCommand("select * from flightGridView", Globals.Connection);
            var result = cmd.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    object[] objs = {
                        result.GetInt32(0),
                        result.GetInt32(1),
                        result.GetString(2),
                        result.GetInt32(4),
                        result.GetString(5),
                        result.GetInt32(7),
                        result.GetString(8),
                        result.GetInt32(9),
                        result.GetString(10),
                        result.GetInt32(11),
                        result.GetString(12),
                        result.GetString(13),
                        result.GetDateTime(14),
                        result.GetDateTime(15)
                    };

                    gridFlights.Rows.Add(objs);
                }
            }
            result.Close();
        }

        #endregion
        #region "Havayolları"

        private void AddAirline(object sender, EventArgs e)
        {
            string airline = txtAirlineName.Text.Trim();
            if (string.IsNullOrWhiteSpace(airline))
            {
                MessageBox.Show("Havayolu adı alanı boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MySqlCommand cmd = new MySqlCommand($"insert into airlines (airlineName) values ('{ airline }')", Globals.Connection);
            try
            {
                int rows = cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                MessageBox.Show($"Havayolu ({ airline }) zaten sistemde kayıtlı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            cmd = new MySqlCommand("select max(airlineID) from airlines", Globals.Connection);

            var rd = cmd.ExecuteReader();
            rd.Read();

            var id = rd.GetInt32(0);
            txtAirlineID.Text = id.ToString();

            rd.Close();

            MessageBox.Show($"Havayolu ({ airline }) eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            gridAirlines.Rows.Add(id, airline);
        }

        private void DeleteAirline(object sender, EventArgs e)
        {
            var id = txtAirlineID.Text;
            if (string.IsNullOrWhiteSpace(id) || gridAirlines.RowCount == 1)
            {
                MessageBox.Show("Listeden kayıt seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show($"Seçilen havayolu silinecek.\nHavayoluna bağlı bütün bilgiler (uçuşlar, pilotlar) silinecek.\nDikkat bu işlem geri alınamaz!", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                MySqlCommand cmd = new MySqlCommand($"delete from airlines where airlineID = { id }", Globals.Connection);
                cmd.ExecuteNonQuery();
                foreach (DataGridViewRow r in gridAirlines.Rows)
                {
                    if (r.Cells[0].Value.ToString() == id)
                    {
                        gridAirlines.Rows.Remove(r);
                        txtAirlineID.Text = "";
                        return;
                    }
                }
            }
        }
        private void SelectAirlines(object sender, EventArgs e)
        {
            if (gridAirlines.SelectedRows.Count < 1) return;
            var row = gridAirlines.SelectedRows[0].Index;
            if (row < 0) return;
            var r = gridAirlines.Rows[row];
            if (row >= gridAirlines.RowCount - 1) return;

            var idstr = r.Cells[0].Value.ToString();
            var namestr = r.Cells[1].Value.ToString();

            txtAirlineID.Text = idstr;
            txtAirlineName.Text = namestr;

            txtPilotAirlineID.Text = idstr;
            txtPilotAirlineName.Text = namestr;

            txtFlightAirlineID.Text = idstr;
            txtFlightAirlineName.Text = namestr;
        }

        private void UpdateAirline(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAirlineID.Text))
            {
                MessageBox.Show("Düzenlenecek havayolunu seçin!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var id = int.Parse(txtAirlineID.Text);

            MySqlCommand cmd = new MySqlCommand($"update airlines set airlineName = '{ txtAirlineName.Text }' where airlineID = { id }", Globals.Connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Düzenleme başarısız! Aynı isme sahip başka bir havayolu daha mevcut. { ex.Message }", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (DataGridViewRow r in gridAirlines.Rows)
            {
                if ((int)r.Cells[0].Value == id)
                {
                    r.Cells[1].Value = txtAirlineName.Text;
                    return;
                }
            }
        }
        #endregion
        #region"Pilotlar"
        private void AddPilot(object sender, EventArgs e)
        {
            string name = txtPilotName.Text.Trim();
            string surname = txtPilotSurname.Text.Trim();
            string tc = txtPilotTC.Text.Trim();
            string aidstr = txtPilotAirlineID.Text;

            if (string.IsNullOrWhiteSpace(aidstr))
            {
                MessageBox.Show("Havayolu alanı boş bırakılamaz! Havayolu sekmesinden havayolu seçiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int aid = int.Parse(aidstr);

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
            {
                MessageBox.Show("İsim ve soy isim alanları boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!tc.IsTCValid())
            {
                MessageBox.Show("TC Kimlik numarası alanı 11 karakter ve sadece sayılardan oluşmalıdır!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MySqlCommand cmd = new MySqlCommand($"insert into pilots (name, surname, tc, airlineID) values ('{ name }', '{ surname }', '{ tc }', { aidstr })", Globals.Connection);
            try
            {
                int rows = cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                MessageBox.Show($"Aynı TC Kimlik numarasına sahip başka bir pilot ({ tc }) zaten sistemde kayıtlı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            cmd = new MySqlCommand("select max(pilotID) from pilots", Globals.Connection);

            var rd = cmd.ExecuteReader();
            rd.Read();

            var id = rd.GetInt32(0);
            txtPilotID.Text = id.ToString();

            rd.Close();

            MessageBox.Show($"Pilot ({ name + " " + surname }) eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            gridPilots.Rows.Add(id, tc, name, surname, txtPilotAirlineName.Text, aidstr);
        }

        private void DeletePilot(object sender, EventArgs e)
        {
            var id = txtPilotID.Text;
            if (string.IsNullOrWhiteSpace(id) || gridPilots.RowCount == 1)
            {
                MessageBox.Show("Listeden kayıt seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show($"Seçilen pilot  silinecek.\nPilota bağlı bütün bilgiler (uçuşlar vb) silinecek.\nDikkat bu işlem geri alınamaz!", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                MySqlCommand cmd = new MySqlCommand($"delete from pilots where pilotID = { id }", Globals.Connection);
                cmd.ExecuteNonQuery();
                foreach (DataGridViewRow r in gridPilots.Rows)
                {
                    if (r.Cells[0].Value.ToString() == id)
                    {
                        gridPilots.Rows.Remove(r);
                        return;
                    }
                }
            }
        }

        private void UpdatePilot(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPilotID.Text))
            {
                MessageBox.Show("Düzenlenecek pilotu seçin!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var id = int.Parse(txtPilotID.Text);

            MySqlCommand cmd = new MySqlCommand($"update pilots set name = '{ txtPilotName.Text }',surname ='{ txtPilotSurname.Text}',tc='{txtPilotTC.Text}',airlineID=={txtPilotAirlineID.Text} where pilotID = { id }", Globals.Connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Düzenleme başarısız! Aynı tc sahip başka bir pilot daha mevcut. { ex.Message }", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (DataGridViewRow r in gridPilots.Rows)
            {
                if ((int)r.Cells[0].Value == id)
                {
                    r.Cells[1].Value = txtPilotName.Text;
                    r.Cells[2].Value = txtPilotSurname.Text;
                    r.Cells[3].Value = txtPilotTC.Text;
                    r.Cells[4].Value = txtPilotAirlineID.Text;
                    RefreshPilots();
                    return;
                }
            }
        }
        private void SelectPilot(object sender, EventArgs e)
        {
            if (gridPilots.SelectedRows.Count < 1) return;
            var row = gridPilots.SelectedRows[0].Index;
            if (row < 0) return;
            var r = gridPilots.Rows[row];
            if (row >= gridPilots.RowCount - 1) return;

            var idstr = r.Cells[0].Value.ToString();
            var tc = r.Cells[1].Value.ToString();
            var name = r.Cells[2].Value.ToString();
            var surname = r.Cells[3].Value.ToString();
            var airline = r.Cells[4].Value.ToString();
            var airlineid = r.Cells[5].Value.ToString();
            var namestr = name + " " + surname;

            txtPilotID.Text = idstr;
            txtPilotName.Text = name;
            txtPilotSurname.Text = surname;
            txtPilotTC.Text = tc;
            txtPilotAirlineID.Text = airlineid;
            txtPilotAirlineName.Text = airline;

            if (SwitchCopilot)
            {
                txtFlightCopilotID.Text = idstr;
                txtFlightCopilotName.Text = namestr;
            }
            else
            {
                txtFlightPilotID.Text = idstr;
                txtFlightPilotName.Text = namestr;
            }
        }
        #endregion
        private bool SwitchLanding = false;
        private void SwitchSelectTakeoff(object sender, EventArgs e)
        {
            txtFlightLandingAirportID.BackColor = SystemColors.Control;
            txtFlightLandingAirportName.BackColor = SystemColors.Control;
            SwitchLanding = false;

            txtFlightTakeoffAirportID.BackColor = Color.LightGreen;
            txtFlightTakeoffAirportName.BackColor = Color.LightGreen;
        }

        private void SwitchSelectLanding(object sender, EventArgs e)
        {
            txtFlightLandingAirportID.BackColor = Color.LightGreen;
            txtFlightLandingAirportName.BackColor = Color.LightGreen;
            SwitchLanding = true;

            txtFlightTakeoffAirportID.BackColor = SystemColors.Control;
            txtFlightTakeoffAirportName.BackColor = SystemColors.Control;
        }

        private bool SwitchCopilot = false;
        private void SwitchSelectPilot(object sender, EventArgs e)
        {
            txtFlightPilotID.BackColor = Color.LightGreen;
            txtFlightPilotName.BackColor = Color.LightGreen;
            SwitchCopilot = false;

            txtFlightCopilotID.BackColor = SystemColors.Control;
            txtFlightCopilotName.BackColor = SystemColors.Control;
        }

        private void SwitchSelectCopilot(object sender, EventArgs e)
        {
            txtFlightPilotID.BackColor = SystemColors.Control;
            txtFlightPilotName.BackColor = SystemColors.Control;
            SwitchCopilot = true;

            txtFlightCopilotID.BackColor = Color.LightGreen;
            txtFlightCopilotName.BackColor = Color.LightGreen;
        }
        #region"Çalışanlar"
        private void AddStaff(object sender, EventArgs e)
        {
            string name = txtStaffName.Text.Trim();
            string surname = txtStaffSurname.Text.Trim();
            string username = txtStaffUsername.Text.Trim();
            string password = txtStaffPassword.Text.Trim();
            string tc = txtStaffTC.Text.Trim();
            string airport = txtStaffAirportID.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("İsim / Soy isim / Kullanıcı adı / Şifre alanları boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrWhiteSpace(airport))
            {
                MessageBox.Show("Havalimanı alanı boş bırakılamaz! İlgili sekmeden havalimanı seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!tc.IsTCValid())
            {
                MessageBox.Show("TC Kimlik numarası alanı 11 karakter ve sadece sayılardan oluşmalıdır!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MySqlCommand cmd = new MySqlCommand($"insert into staff (name, surname, username, password, tc, airportID) values ('{ name }', '{ surname }', '{ username }', '{ password }', '{ tc }', { airport })", Globals.Connection);
            try
            {
                int rows = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Bu TC kimlik numarasına ya da kullanıcı adına sahip başka bir çalışan zaten sistemde kayıtlı! { ex.Message }", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            cmd = new MySqlCommand("select max(staffID) from staff", Globals.Connection);
            var rd = cmd.ExecuteReader();
            rd.Read();
            var id = rd.GetInt32(0);
            txtStaffID.Text = id.ToString();

            rd.Close();

            MessageBox.Show($"Çalışan ({ name + " " + surname }) eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            gridStaff.Rows.Add(id, tc, name, surname, txtStaffAirportName.Text, airport, username, password);
        }
        private void SelectStaff(object sender, EventArgs e)
        {
            if (gridStaff.SelectedRows.Count < 1) return;
            var row = gridStaff.SelectedRows[0].Index;
            if (row < 0) return;
            var r = gridStaff.Rows[row];
            if (row >= gridStaff.RowCount - 1) return;

            var idstr = r.Cells[0].Value.ToString();
            var tc = r.Cells[1].Value.ToString();
            var name = r.Cells[2].Value.ToString();
            var surname = r.Cells[3].Value.ToString();
            var airport = r.Cells[4].Value.ToString();
            var airportid = r.Cells[5].Value.ToString();
            var username = r.Cells[6].Value.ToString();
            var password = r.Cells[7].Value.ToString();

            txtStaffID.Text = idstr;
            txtStaffTC.Text = tc;
            txtStaffName.Text = name;
            txtStaffSurname.Text = surname;
            txtStaffAirportID.Text = airportid;
            txtStaffAirportName.Text = airport;
            txtStaffUsername.Text = username;
            txtStaffPassword.Text = password;
        }
        private void DeleteStaff(object sender, EventArgs e)
        {
            var id = txtStaffID.Text;
            if (string.IsNullOrWhiteSpace(id) || gridStaff.RowCount == 1)
            {
                MessageBox.Show("Listeden kayıt seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show($"Seçilen eleman silinecek.\nDikkat bu işlem geri alınamaz!", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                MySqlCommand cmd = new MySqlCommand($"delete from staff where staffID = { id }", Globals.Connection);
                cmd.ExecuteNonQuery();
                foreach (DataGridViewRow r in gridStaff.Rows)
                {
                    if (r.Cells[0].Value.ToString() == id)
                    {
                        gridStaff.Rows.Remove(r);
                        return;
                    }
                }
            }
        }
        private void UpdateStaff(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStaffID.Text))
            {
                MessageBox.Show("Düzenlenecek elemanı seçin!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string name = txtStaffName.Text.Trim();
            string surname = txtStaffSurname.Text.Trim();
            string username = txtStaffUsername.Text.Trim();
            string password = txtStaffPassword.Text.Trim();
            string tc = txtStaffTC.Text.Trim();
            string airport = txtStaffAirportID.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("İsim / Soy isim / Kullanıcı adı / Şifre alanları boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!tc.IsTCValid())
            {
                MessageBox.Show("TC Kimlik numarası alanı 11 karakter ve sadece sayılardan oluşmalıdır!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var id = txtStaffID.Text;

            MySqlCommand cmd = new MySqlCommand($"update staff set name = '{ name }', surname ='{ surname }', username = '{ username }', password = '{ password }', tc = '{ tc }', airportID = { airport } where staffID = { id }", Globals.Connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Düzenleme başarısız! Aynı TC Kimlik Numarasına sahip başka bir eleman daha mevcut. { ex.Message }", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            RefreshStaff();
        }
        #endregion
        private void SelectAirport(object sender, EventArgs e)
        {
            if (gridAirports.SelectedRows.Count < 1) return;
            var row = gridAirports.SelectedRows[0].Index;
            if (row < 0) return;
            var r = gridAirports.Rows[row];
            if (row >= gridAirports.RowCount - 1) return;

            var idstr = r.Cells[0].Value.ToString();
            var name = r.Cells[1].Value.ToString();
            var city = r.Cells[2].Value.ToString();
            var admin = r.Cells[3].Value.ToString();
            var adminid = r.Cells[4].Value.ToString();
            var cityid = r.Cells[5].Value.ToString();

            txtAirportID.Text = idstr;
            txtAirportName.Text = name;
            txtAirportCityID.Text = cityid;
            txtAirportCityName.Text = city;
            txtAirportAdminID.Text = adminid;
            txtAirportAdminName.Text = admin;

            txtStaffAirportID.Text = idstr;
            txtStaffAirportName.Text = name;

            if (SwitchLanding)
            {
                txtFlightLandingAirportID.Text = idstr;
                txtFlightLandingAirportName.Text = name;
            }
            else
            {
                txtFlightTakeoffAirportID.Text = idstr;
                txtFlightTakeoffAirportName.Text = name;
            }
        }
        #region "Uçak Modelleri"
        private void AddPlaneModel(object sender, EventArgs e)
        {
            string name = txtPlaneModelName.Text.Trim();
            string typeid = txtPlaneModelTypeID.Text;
            int cap = (int)txtPlaneModelCap.Value;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(typeid))
            {
                MessageBox.Show("İsim veya tür alanı boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MySqlCommand cmd = new MySqlCommand($"insert into models (modelName, passengerCapacity, type) values ('{ name }', '{ cap }', '{ typeid }')", Globals.Connection);
            try
            {
                int rows = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Model ({ name }) zaten sistemde kayıtlı! { ex.Message }", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            cmd = new MySqlCommand("select max(modelID) from models", Globals.Connection);
            var rd = cmd.ExecuteReader();
            rd.Read();
            var id = rd.GetInt32(0);
            txtPlaneModelID.Text = id.ToString();

            rd.Close();

            MessageBox.Show($"Model ({ name }) eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            gridPlaneModels.Rows.Add(id, name, cap, txtPlaneModelTypeName.Text, typeid);
        }

        

        private void DeletePlaneModel(object sender, EventArgs e)
        {
            var id = txtPlaneModelID.Text;
            if (string.IsNullOrWhiteSpace(id) || gridPlaneModels.RowCount == 1)
            {
                MessageBox.Show("Listeden kayıt seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show($"Seçilen uçak modeli silinecek.\nUçak modeline bağlı bütün bilgiler (uçaklar, uçuşlar vb) silinecek.\nDikkat bu işlem geri alınamaz!", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                MySqlCommand cmd = new MySqlCommand($"delete from models where modelID = { id }", Globals.Connection);
                cmd.ExecuteNonQuery();
                foreach (DataGridViewRow r in gridPlaneModels.Rows)
                {
                    if (r.Cells[0].Value.ToString() == id)
                    {
                        gridPlaneModels.Rows.Remove(r);
                        return;
                    }
                }
            }
        }

        private void UpdatePlaneModel(object sender, EventArgs e)
        {
            string modelname = txtPlaneModelName.Text;
            int modelcap = (int)txtPlaneModelCap.Value;

            if (string.IsNullOrWhiteSpace(modelname) || string.IsNullOrWhiteSpace(txtPlaneModelID.Text))
            {
                MessageBox.Show("Düzenlenecek modeli seçin!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MySqlCommand cmd = new MySqlCommand($"update models set modelName = '{ modelname }', passengerCapacity = { modelcap } where modelID = '{ txtPlaneModelID.Text }'", Globals.Connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Düzenleme başarısız! Aynı isme sahip başka bir model daha mevcut. { ex.Message }", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (DataGridViewRow r in gridPlaneModels.Rows)
            {
                if (r.Cells[0].Value.ToString() == txtPlaneModelID.Text)
                {
                    r.Cells[1].Value = modelname;
                    r.Cells[2].Value = modelcap;
                    RefreshPlanes();
                    return;
                }
            }
        }
        private void SelectPlaneModel(object sender, EventArgs e)
        {
            if (gridPlaneModels.SelectedRows.Count < 1) return;
            var row = gridPlaneModels.SelectedRows[0].Index;
            if (row < 0) return;
            var r = gridPlaneModels.Rows[row];
            if (row >= gridPlaneModels.RowCount - 1) return;

            var idstr = r.Cells[0].Value.ToString();
            var name = r.Cells[1].Value.ToString();
            var cap = r.Cells[2].Value.ToString();
            var typename = r.Cells[3].Value.ToString();
            var type = r.Cells[4].Value.ToString();

            txtPlaneModelID.Text = idstr;
            txtPlaneModelName.Text = name;
            txtPlaneModelTypeID.Text = type;
            txtPlaneModelTypeName.Text = typename;
            txtPlaneModelCap.Value = int.Parse(cap);

            txtPlaneModelID2.Text = idstr;
            txtPlaneModelName2.Text = name;
        }
        #endregion
        #region"Uçak Tipleri"
        private void AddPlaneType(object sender, EventArgs e)
        {
            string type = txtPlaneTypeName.Text.Trim();
            if (string.IsNullOrWhiteSpace(type))
            {
                MessageBox.Show("Tür adı alanı boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MySqlCommand cmd = new MySqlCommand($"insert into types (name) values ('{ type }')", Globals.Connection);
            try
            {
                int rows = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Uçak türü ({ type }) zaten sistemde kayıtlı! { ex.Message }", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            cmd = new MySqlCommand("select max(typeID) from types", Globals.Connection);
            var rd = cmd.ExecuteReader();
            rd.Read();
            var id = rd.GetInt32(0);
            txtPlaneTypeID.Text = id.ToString();

            rd.Close();

            MessageBox.Show($"Uçak türü ({ type }) eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            gridPlaneTypes.Rows.Add(id, type);
        }


        private void DeletePlaneType(object sender, EventArgs e)
        {
            var id = txtPlaneTypeID.Text;
            if (string.IsNullOrWhiteSpace(id) || gridPlaneTypes.RowCount == 1)
            {
                MessageBox.Show("Listeden kayıt seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show($"Seçilen uçak tipi silinecek.\nUçak tipine bağlı bütün bilgiler (uçak modelleri, uçuşlar, uçaklar vb) silinecek.\nDikkat bu işlem geri alınamaz!", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                MySqlCommand cmd = new MySqlCommand($"delete from types where typeID = { id }", Globals.Connection);
                cmd.ExecuteNonQuery();
                foreach (DataGridViewRow r in gridPlaneTypes.Rows)
                {
                    if (r.Cells[0].Value.ToString() == id)
                    {
                        gridPlaneTypes.Rows.Remove(r);
                        return;
                    }
                }
            }
        }
        private void UpdatePlaneType(object sender, EventArgs e)
        {
            string typename = txtPlaneTypeName.Text;

            if (string.IsNullOrWhiteSpace(typename) || string.IsNullOrWhiteSpace(txtPlaneTypeID.Text))
            {
                MessageBox.Show("Düzenlenecek türü seçin!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MySqlCommand cmd = new MySqlCommand($"update types set name = '{ typename }' where typeID = '{ txtPlaneTypeID.Text }'", Globals.Connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Düzenleme başarısız! Aynı tür sistemde zaten mevcut. { ex.Message }", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (DataGridViewRow r in gridPlaneTypes.Rows)
            {
                if (r.Cells[0].Value.ToString() == txtPlaneTypeID.Text)
                {
                    r.Cells[1].Value = typename;
                    RefreshPlaneModels();
                    return;
                }
            }
        }
        private void SelectPlaneType(object sender, EventArgs e)
        {
            if (gridPlaneTypes.SelectedRows.Count < 1) return;
            var row = gridPlaneTypes.SelectedRows[0].Index;
            if (row < 0) return;
            var r = gridPlaneTypes.Rows[row];
            if (row >= gridPlaneTypes.RowCount - 1) return;

            var idstr = r.Cells[0].Value.ToString();
            var type = r.Cells[1].Value.ToString();

            txtPlaneTypeID.Text = idstr;
            txtPlaneTypeName.Text = type;

            txtPlaneModelTypeID.Text = idstr;
            txtPlaneModelTypeName.Text = type;
        }
        #endregion
        #region"Uçaklar"
        private void AddPlane(object sender, EventArgs e)
        {
            string planeid = txtPlaneID.Text.Trim();
            string modelid = txtPlaneModelID2.Text;
            if (string.IsNullOrWhiteSpace(planeid) || string.IsNullOrWhiteSpace(modelid))
            {
                MessageBox.Show("Uçak ID veya Model alanı boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MySqlCommand cmd = new MySqlCommand($"insert into planes (planeID, modelID) values ('{ planeid }', '{ modelid }')", Globals.Connection);
            try
            {
                int rows = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Uçak ({ planeid }) zaten sistemde kayıtlı! { ex.Message }", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            MessageBox.Show($"Uçak ({ planeid }) eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            gridPlanes.Rows.Add(planeid, txtPlaneModelName2.Text, modelid);
        }

        private void DeletePlane(object sender, EventArgs e)
        {
            var id = txtPlaneID.Text;
            if (string.IsNullOrWhiteSpace(id) || gridPlanes.RowCount == 1)
            {
                MessageBox.Show("Listeden kayıt seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show($"Seçilen uçak  silinecek.\nUçağa bağlı bütün bilgiler (uçuşlar vb) silinecek.\nDikkat bu işlem geri alınamaz!", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                MySqlCommand cmd = new MySqlCommand($"delete from planes where planeID = '{ id }'", Globals.Connection);
                cmd.ExecuteNonQuery();
                foreach (DataGridViewRow r in gridPlanes.Rows)
                {
                    if (r.Cells[0].Value.ToString() == id)
                    {
                        gridPlanes.Rows.Remove(r);
                        return;
                    }
                }
            }
        }
        private string SelectedPlaneID = "";
        private void SelectPlane(object sender, EventArgs e)
        {
            if (gridPlanes.SelectedRows.Count < 1) return;
            var row = gridPlanes.SelectedRows[0].Index;
            if (row < 0) return;
            var r = gridPlanes.Rows[row];
            if (row >= gridPlanes.RowCount - 1) return;

            var idstr = r.Cells[0].Value.ToString();
            var modelname = r.Cells[1].Value.ToString();
            var modelid = r.Cells[2].Value.ToString();

            SelectedPlaneID = idstr;
            txtPlaneID.Text = idstr;
            txtPlaneModelID2.Text = modelid;
            txtPlaneModelName2.Text = modelname;

            txtFlightPlane.Text = idstr;
        }
        private void UpdatePlane(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SelectedPlaneID))
            {
                MessageBox.Show("Düzenlenecek uçağı seçin!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MySqlCommand cmd = new MySqlCommand($"update planes set planeID = '{ txtPlaneID.Text }' where planeID = '{ SelectedPlaneID }'", Globals.Connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Düzenleme başarısız! Aynı isme sahip başka bir uçak daha mevcut. { ex.Message }", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (DataGridViewRow r in gridPlanes.Rows)
            {
                if (r.Cells[0].Value.ToString() == SelectedPlaneID)
                {
                    r.Cells[0].Value = txtPlaneID.Text;
                    return;
                }
            }
        }
        #endregion


        private void ApplicationExit(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        #region"Uçuşlar"
        private void AddFlight(object sender, EventArgs e)
        {
            string takeoffid = txtFlightTakeoffAirportID.Text;
            string landingid = txtFlightLandingAirportID.Text;

            if (takeoffid == landingid)
            {
                MessageBox.Show("Kalkış ve iniş aynı havaalanında gerçekliştirelemez.");
                return;
            }

            string airlineid = txtFlightAirlineID.Text;
            string planeid = txtFlightPlane.Text;
            string pilotid = txtFlightPilotID.Text;
            string copilotid = txtFlightCopilotID.Text;
            DateTime takeoffDate = dateFlightTakeoff.Value;
            DateTime landingDate = dateFlightLanding.Value;

            if (landingDate <= takeoffDate)
            {
                MessageBox.Show("İniş tarihi kalkış tarihinden büyük olmalıdır!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrWhiteSpace(takeoffid) || string.IsNullOrWhiteSpace(landingid))
            {
                MessageBox.Show("Kalkış ve varış noktalarını ilgili sekmeden seçiniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrWhiteSpace(airlineid))
            {
                MessageBox.Show("Havayolu şirketini ilgili sekmeden seçiniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrWhiteSpace(planeid))
            {
                MessageBox.Show("Uçağı ilgili sekmeden seçiniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrWhiteSpace(pilotid) || string.IsNullOrWhiteSpace(copilotid))
            {
                MessageBox.Show("Pilot ve yardımcı pilotu ilgili seçiniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (int.Parse(txtFlightTakeoffAirportID.Text) != Globals.ConnectedAdminAirportID && int.Parse(txtFlightLandingAirportID.Text) != Globals.ConnectedAdminAirportID)
            {
                MessageBox.Show("Yetkiniz olmayan ucus düzenliyorsunuz", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            
            MySqlCommand cmd = new MySqlCommand($"insert into flights (airlineID, takeoff, landing, takeoffDate, planeID, pilotID, coPilotID, landingDate) values ({ airlineid }, { takeoffid }, { landingid }, @date, '{ planeid }', { pilotid }, { copilotid }, @date2)", Globals.Connection);
            cmd.Parameters.Add("@date", MySqlDbType.DateTime).Value = takeoffDate;
            cmd.Parameters.Add("@date2", MySqlDbType.DateTime).Value = landingDate;
            try
            {
                int rows = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Sistem iç hatası, yandaki metni geliştiricilere bildirin: { ex.Message }", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            cmd = new MySqlCommand("select max(flightID) from flights", Globals.Connection);
            var rd = cmd.ExecuteReader();
            rd.Read();
            var id = rd.GetInt32(0);
            txtStaffID.Text = id.ToString();

            rd.Close();

            MessageBox.Show($"Uçuş (<{ id }>) eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            gridFlights.Rows.Add(id, takeoffid, txtFlightTakeoffAirportName.Text, landingid, txtFlightLandingAirportName.Text, airlineid, txtFlightAirlineName.Text, pilotid, txtFlightPilotName.Text, copilotid, txtFlightCopilotName.Text, planeid, takeoffDate, landingDate);
        }

        private void DeleteFlight(object sender, EventArgs e)
        {
            var id = txtFlightID.Text;
            if (string.IsNullOrWhiteSpace(id) || gridFlights.RowCount == 1)
            {
                MessageBox.Show("Listeden kayıt seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show($"Seçilen uçuş silinecek.\nUçuşa bağlı bütün veriler (yolcular) silinecek.\nDikkat bu işlem geri alınamaz!", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                MySqlCommand cmd = new MySqlCommand($"delete from flights where flightID = { id }", Globals.Connection);
                cmd.ExecuteNonQuery();
                foreach (DataGridViewRow r in gridFlights.Rows)
                {
                    if (r.Cells.Count < 0) break;
                    if (r.Cells[0].Value.ToString() == id)
                    {
                        gridFlights.Rows.Remove(r);
                        return;
                    }
                }
            }
        }

        private void UpdateFlight(object sender, EventArgs e)
        {
            string flightid = txtFlightID.Text;

            if (string.IsNullOrWhiteSpace(flightid))
            {
                MessageBox.Show("Listeden bir kayıt seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string takeoffid = txtFlightTakeoffAirportID.Text;
            string landingid = txtFlightLandingAirportID.Text;

            if (takeoffid == landingid)
            {
                MessageBox.Show("Kalkış ve iniş aynı havaalanında gerçekliştirelemez!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string airlineid = txtFlightAirlineID.Text;
            string planeid = txtFlightPlane.Text;
            string pilotid = txtFlightPilotID.Text;
            string copilotid = txtFlightCopilotID.Text;
            DateTime takeoffDate = dateFlightTakeoff.Value;
            DateTime landingDate = dateFlightLanding.Value;

            if (landingDate <= takeoffDate)
            {
                MessageBox.Show("İniş tarihi kalkış tarihinden büyük olmalıdır!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MySqlCommand cmd = new MySqlCommand($"update flights set airlineID = { airlineid }, takeoff = { takeoffid }, landing = { landingid }, takeoffDate = @date, planeID = '{ planeid }', pilotID = { pilotid }, copilotID = { copilotid }, landingDate = @date2 where flightID = { flightid }", Globals.Connection);
            cmd.Parameters.Add("@date", MySqlDbType.DateTime).Value = takeoffDate;
            cmd.Parameters.Add("@date2", MySqlDbType.DateTime).Value = landingDate;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Düzenleme başarısız! Sistem iç hatası: { ex.Message }", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (DataGridViewRow r in gridFlights.Rows)
            {
                if (r.Cells[0].Value.ToString() == flightid)
                {
                    r.SetValues(new object[] { flightid, takeoffid, txtFlightTakeoffAirportName.Text, landingid, txtFlightLandingAirportName.Text, airlineid, txtFlightAirlineName.Text, pilotid, txtFlightPilotName.Text, copilotid, txtFlightCopilotName.Text, planeid, takeoffDate, landingDate });
                    return;
                }
            }
        }
        private void SelectFlight(object sender, EventArgs e)
        {
            if (gridFlights.SelectedRows.Count < 1) return;
            var row = gridFlights.SelectedRows[0].Index;
            if (row < 0) return;
            var r = gridFlights.Rows[row];
            if (row >= gridFlights.RowCount - 1) return;

            var idstr = r.Cells[0].Value.ToString();
            var takeoffid = r.Cells[1].Value.ToString();
            var takeoffaip = r.Cells[2].Value.ToString();
            var landingid = r.Cells[3].Value.ToString();
            var landingaip = r.Cells[4].Value.ToString();
            var airlineid = r.Cells[5].Value.ToString();
            var airline = r.Cells[6].Value.ToString();
            var pilotid = r.Cells[7].Value.ToString();
            var pilot = r.Cells[8].Value.ToString();
            var copilotid = r.Cells[9].Value.ToString();
            var copilot = r.Cells[10].Value.ToString();
            var planeid = r.Cells[11].Value.ToString();
            var takeoffDateStr = r.Cells[12].Value.ToString();
            var landingDateStr = r.Cells[13].Value.ToString();

            txtFlightID.Text = idstr;
            txtFlightTakeoffAirportID.Text = takeoffid;
            txtFlightTakeoffAirportName.Text = takeoffaip;
            txtFlightLandingAirportID.Text = landingid;
            txtFlightLandingAirportName.Text = landingaip;
            txtFlightAirlineID.Text = airlineid;
            txtFlightAirlineName.Text = airline;
            txtFlightPilotID.Text = pilotid;
            txtFlightPilotName.Text = pilot;
            txtFlightCopilotID.Text = copilotid;
            txtFlightCopilotName.Text = copilot;
            txtFlightPlane.Text = planeid;
            var takeoffDate = DateTime.Parse(takeoffDateStr);
            dateFlightTakeoff.Value = takeoffDate;
            var landingDate = DateTime.Parse(landingDateStr);
            dateFlightLanding.Value = landingDate;

            if (takeoffDate < DateTime.Now && DateTime.Now < landingDate)
            {
                btnDeleteFlight.Enabled = false;
                //tooltipGeneral.Active = true;
            }
            else
            {
                btnDeleteFlight.Enabled = true;
                //tooltipGeneral.Active = false;
            }

            if (DateTime.Now > takeoffDate)
            {
                btnUpdateFlight.Enabled = false;
                //tooltipGeneral.Active = true;
            }
            else
            {
                btnUpdateFlight.Enabled = true;
                //tooltipGeneral.Active = false;
            }
        }
        #endregion


        
    }
 }
