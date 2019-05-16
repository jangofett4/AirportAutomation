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

namespace AirportAutomation
{
    public partial class AAStaffPanel : Form
    {
        public AAStaffPanel()
        {
            InitializeComponent();
        }

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
            //txtStaffID.Text = id.ToString();

            rd.Close();

            MessageBox.Show($"Uçuş (<{ id }>) eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            gridFlights.Rows.Add(id, takeoffid, txtFlightTakeoffAirportName.Text, landingid, txtFlightLandingAirportName.Text, airlineid, txtFlightAirlineName.Text, pilotid, txtFlightPilotName.Text, copilotid, txtFlightCopilotName.Text, planeid, takeoffDate, landingDate);
        }
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
        private void SelectPlane(object sender, DataGridViewCellEventArgs e)
        {
            var row = e.RowIndex;
            if (row < 0) return;
            var r = gridPlanes.Rows[row];
            if (row >= gridPlanes.RowCount - 1) return;

            var idstr = r.Cells[0].Value.ToString();
            txtFlightPlane.Text = idstr;
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
        public void RefreshAirports()
        {
            gridAirports.Rows.Clear();

            MySqlCommand cmd = new MySqlCommand("select * from airportGridView", Globals.Connection);
            using (var result = cmd.ExecuteReader())
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
        }
        private void SelectAirport(object sender, DataGridViewCellEventArgs e)
    
    {
            var row = e.RowIndex;
            if (row < 0) return;
            var r = gridAirports.Rows[row];
            if (row >= gridAirports.RowCount - 1) return;

            var idstr = r.Cells[0].Value.ToString();
            var name = r.Cells[1].Value.ToString();

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
        private void AAStaffPanel_Load(object sender, EventArgs e)
        {
            RefreshPlanes();
            RefreshAirports();
            RefreshFlights();
            RefreshPassengers();
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
                        result.GetDateTime(14)
                    };

                    gridFlights.Rows.Add(objs);
                }
            }
            result.Close();
        }
        private void RefreshData(object sender, EventArgs e)
        {
            var src = contextRefresh.SourceControl;
            // if (src == gridCountries) RefreshCountries();
            // else if (src == gridAirportAdmins) RefreshAirportAdmins();
            // else if (src == gridCities) RefreshCities();
            // else if (src == gridAirlines) RefreshAirlines();
            // else if (src == gridPilots) RefreshPilots();
            if (src == gridAirports) RefreshAirports();
            // else if (src == gridPlaneTypes) RefreshPlaneTypes();
            else if (src == gridPlanes) RefreshPlanes();
            // else if (src == gridPlaneModels) RefreshPlaneModels();
            // else if (src == gridStaff) RefreshStaff();
            else if (src == gridFlights) RefreshFlights();
            else if (src == gridPassengers) RefreshPassengers();
        }
        public void AddPasenger()
        {
            string tc = txtPassengerTc.Text.Trim();
            string name = txtPassengerName.Text.Trim();
            string surname = txtPassengerSurname.Text.Trim();
            string flightID = txtPassengerFlightID.Text;
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
            {
                MessageBox.Show("İsim / Soy isim alanları boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!tc.IsTCValid())
            {
                MessageBox.Show("TC Kimlik numarası alanı 11 karakter ve sadece sayılardan oluşmalıdır!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            MySqlCommand cmd = new MySqlCommand($"insert into passengers(name,surname,tc,flightID) values ('{ name }', '{ surname }' , '{ tc }',{ flightID }) ",Globals.Connection);
            try
            {
                int rows = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Aynı TC Kimlik numarasına ya da kullanıcı adına sahip başka bir yönetici zaten sistemde kayıtlı! { ex.Message }", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            cmd = new MySqlCommand("select max(airportID) from airports", Globals.Connection);

            var rd = cmd.ExecuteReader();
            rd.Read();

            var id = rd.GetInt32(0);
            txtPassengerID.Text = id.ToString();

            rd.Close();

            MessageBox.Show($"Yolcu ({ name }) eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            gridPassengers.Rows.Add(id , tc, name, surname, flightID);
        }

        private void AddPlane(object sender, EventArgs e)
        {

        }

        private void DeletePlane(object sender, EventArgs e)
        {

        }

        private void UpdatePlane(object sender, EventArgs e)
        {

        }

        private void AddPlaneType(object sender, EventArgs e)
        {

        }

        private void DeletePlaneTypes(object sender, EventArgs e)
        {

        }

        private void UpdatePlaneType(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void selectFlight(object sender, DataGridViewCellEventArgs e)
        {
            var row = e.RowIndex;
            if (row < 0) return;
            var r = gridPassengers.Rows[row];
            if (row >= gridPassengers.RowCount - 1) return;

            var idstr = r.Cells[0].Value.ToString();
            txtPassengerFlightID.Text = idstr;
        }

        private void selectAirline(object sender, DataGridViewCellEventArgs e)
        {
            if (gridAirlines.SelectedRows.Count < 1) return;
            var row = gridAirlines.SelectedRows[0].Index;
            if (row < 0) return;
            var r = gridAirlines.Rows[row];
            if (row >= gridAirlines.RowCount - 1) return;

            var idstr = r.Cells[0].Value.ToString();
            var namestr = r.Cells[1].Value.ToString();

            txtFlightAirlineID.Text = idstr;
            txtFlightAirlineName.Text = namestr;
        }

        private void selectPilot(object sender, DataGridViewCellEventArgs e)
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

        private void addPassenger(object sender, EventArgs e)
        {
            string name = txtPassengerName.Text;
            string surname = txtPassengerSurname.Text;
            string tc = txtPassengerTc.Text;
            string flightID = txtPassengerFlightID.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(tc))
            {
                MessageBox.Show("Yolcu adı / soyadı / TC alanı boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!tc.IsTCValid())
            {
                MessageBox.Show("TC Kimlik numarası alanı 11 karakter ve sadece sayılardan oluşmalıdır!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MySqlCommand cmd = new MySqlCommand($"insert into passengers (name,surname,tc,flightID) values ('{ name }' , '{surname}' , '{tc}' , {flightID} )", Globals.Connection);
            try
            {
                int rows = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Bu yolcu sisteme zaten kayıtlı! { ex.Message }", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            cmd = new MySqlCommand("select max(staffID) from staff", Globals.Connection);
            var rd = cmd.ExecuteReader();
            rd.Read();
            var id = rd.GetInt32(0);
            txtPassengerID.Text = id.ToString();

            rd.Close();

            MessageBox.Show($"Yolcu ({ name + " " + surname }) eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            gridPassengers.Rows.Add(id, tc, name, surname, flightID);


        }

        private void selectPassenger(object sender, DataGridViewCellEventArgs e)
        {
            if (gridPassengers.SelectedRows.Count < 1) return;
            var row = gridPassengers.SelectedRows[0].Index;
            if (row < 0) return;
            var r = gridPassengers.Rows[row];
            if (row >= gridPassengers.RowCount - 1) return;

            var id = r.Cells[0].Value.ToString();
            var tc = r.Cells[1].Value.ToString();
            var name = r.Cells[2].Value.ToString();
            var surname = r.Cells[3].Value.ToString();
            var flightID = r.Cells[4].Value.ToString();

            txtPassengerFlightID.Text = flightID;
            txtPassengerTc.Text = tc;
            txtPassengerName.Text = name;
            txtPassengerSurname.Text = surname;
            txtPassengerID.Text = id;
        }

        private void editPassenger(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassengerID.Text))
            {
                MessageBox.Show("Düzenlenecek yolcuyu seçin!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var id = txtPassengerID.Text;
            var name = txtPassengerName.Text;
            var surname = txtPassengerSurname.Text;
            var tc = txtPassengerTc.Text;
            var flightID = txtPassengerFlightID.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(tc))
            {
                MessageBox.Show("Yolcu adı / soyadı / TC alanı boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!tc.IsTCValid())
            {
                MessageBox.Show("TC Kimlik numarası alanı 11 karakter ve sadece sayılardan oluşmalıdır!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            MySqlCommand cmd = new MySqlCommand($"update countries set tc = '{tc}' , name = '{name}',surname = '{surname}' , flightID = {flightID} where id = { id }", Globals.Connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Düzenleme başarısız! Aynı T.C. ye sahip başka bir yolcu daha mevcut. { ex.Message }", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            RefreshPassengers();
        }
        public void RefreshPassengers()
        {
            MySqlCommand cmd = new MySqlCommand("select * from passengers", Globals.Connection);
            var result = cmd.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {

                    int id = result.GetInt32(0);
                    string name = result.GetString(1);
                    string surname = result.GetString(2);
                    string tc = result.GetString(3);
                    string flightID = result.GetInt32(4).ToString();
                    gridPassengers.Rows.Add(id, tc,name,surname,flightID);
                }
            }
            result.Close();
        }
    }
}
