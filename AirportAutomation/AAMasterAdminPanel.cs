using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AirportAutomation
{
    public partial class AAMasterAdminPanel : Form
    {
        public AAMasterAdminPanel()
        {
            MySqlCommand cmd = new MySqlCommand($"select * from admins where username = '{ Globals.ConnectedAdminUsername }' and password = '{ Globals. ConnectedAdminPassword }';", Globals.Connection);
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

        #region "Zıkkım"

        public void r_countries()
        {
            gridCountries.Rows.Clear();

            MySqlCommand cmd = new MySqlCommand("select * from countries", Globals.Connection);
            var result = cmd.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {

                    int id = result.GetInt32(0);
                    string name = result.GetString(1);
                    gridCountries.Rows.Add(id, name);
                }
            }
            result.Close();
        }

        public void r_airportAdmins()
        {
            gridAirportAdmins.Rows.Clear();

            MySqlCommand cmd = new MySqlCommand("select * from airport_admins", Globals.Connection);
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
                    gridAirportAdmins.Rows.Add(id, tc, name, surname, username, password);
                }
            }
            result.Close();
        }

        public void r_cities()
        {
            r_countries();

            gridCities.Rows.Clear();

            MySqlCommand cmd = new MySqlCommand("select * from cities", Globals.Connection);
            var result = cmd.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    int id = result.GetInt32(0);
                    int cid = result.GetInt32(1);
                    string name = result.GetString(2);
                    string cname = "";
                    foreach (DataGridViewRow r in gridCountries.Rows)
                        if (r.Cells[0].Value.ToString() == cid.ToString())
                        {
                            cname = r.Cells[1].Value.ToString();
                            break;
                        }
                    gridCities.Rows.Add(id, name, cname, cid);
                }
            }
            result.Close();
        }

        public void r_airlines()
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

        public void r_pilots()
        {
            r_airlines();

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

        public void r_airports()
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

        public void r_types()
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

        public void r_models()
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

        public void r_planes()
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

        #endregion

        #region "Ülkeler"

        private void AddCountry(object sender, EventArgs e)
        {
            string country = txtCountryName.Text.Trim();
            if (string.IsNullOrWhiteSpace(country))
            {
                MessageBox.Show("Ülke adı alanı boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MySqlCommand cmd = new MySqlCommand($"insert into countries (name) values ('{ country }')", Globals.Connection);
            try
            {
                int rows = cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                MessageBox.Show($"Ülke ({ country }) zaten sistemde kayıtlı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            cmd = new MySqlCommand("select max(countryID) from countries", Globals.Connection);

            var rd = cmd.ExecuteReader();
            rd.Read();

            var id = rd.GetInt32(0);
            txtCountryID.Text = id.ToString();

            rd.Close();

            MessageBox.Show($"Ülke ({ country }) eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            gridCountries.Rows.Add(id, country);
        }

        private void SelectCountry(object sender, DataGridViewCellEventArgs e)
        {
            var row = e.RowIndex;
            if (row < 0) return;
            var r = gridCountries.Rows[row];
            if (row >= gridCountries.RowCount - 1) return;

            var idstr = r.Cells[0].Value.ToString();
            var namestr = r.Cells[1].Value.ToString();

            txtCountryID.Text = idstr;
            txtCountryName.Text = namestr;

            txtCityCountryID.Text = idstr;
            txtCityCountryName.Text = namestr;
        }

        private void UpdateCountry(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCountryID.Text))
            {
                MessageBox.Show("Düzenlenecek ülkeyi seçin!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var id = int.Parse(txtCountryID.Text);

            MySqlCommand cmd = new MySqlCommand($"update countries set name = '{ txtCountryName.Text }' where countryID = { id }", Globals.Connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Düzenleme başarısız! Aynı isme sahip başka bir ülke daha mevcut. { ex.Message }", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (DataGridViewRow r in gridCountries.Rows)
            {
                if ((int)r.Cells[0].Value == id)
                {
                    r.Cells[1].Value = txtCountryName.Text;
                    r_cities();
                    return;
                }
            }
        }

        private void DeleteCountry(object sender, EventArgs e)
        {
            var id = int.Parse(txtCountryID.Text);
            if (MessageBox.Show($"Seçilen ülke silinecek.\nÜlkeya bağlı bütün bilgiler (havalimanı, uçuşlar vb) silinecek.\nDikkat bu işlem geri alınamaz!", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {                
                MySqlCommand cmd = new MySqlCommand($"delete from countries where countryID = { id }", Globals.Connection);
                cmd.ExecuteNonQuery();
            }

            foreach (DataGridViewRow r in gridCountries.Rows)
            {
                if ((int)r.Cells[0].Value == id)
                {
                    gridCountries.Rows.Remove(r);
                    r_cities();
                    return;
                }
            }
        }

        private void ClearFormCountry(object sender, EventArgs e)
        {
            txtCountryID.Text = "";
            txtCountryName.Text = "";
        }

        #endregion
        #region "Şehirler"

        private void AddCity(object sender, EventArgs e)
        {
            string city = txtCityName.Text.Trim();
            if (string.IsNullOrWhiteSpace(city))
            {
                MessageBox.Show("Şehir adı alanı boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MySqlCommand cmd = new MySqlCommand($"insert into cities (name, countryID) values ('{ city }', { txtCityCountryID.Text })", Globals.Connection);
            try
            {
                int rows = cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                MessageBox.Show($"Şehir ({ city }) zaten sistemde kayıtlı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            cmd = new MySqlCommand("select max(cityID) from cities", Globals.Connection);
            var rd = cmd.ExecuteReader();
            rd.Read();
            var id = rd.GetInt32(0);
            txtCityID.Text = id.ToString();

            rd.Close();

            MessageBox.Show($"Şehir ({ city }) eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            gridCities.Rows.Add(id, city, txtCityCountryName.Text, txtCityCountryID.Text);
        }

        private void CityUpdate(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCityID.Text))
            {
                MessageBox.Show("Düzenlenecek şehri seçin!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var id = int.Parse(txtCityID.Text);
            var cid = int.Parse(txtCityCountryID.Text);

            MySqlCommand cmd = new MySqlCommand($"update cities set name = '{ txtCityName.Text }', countryID = { cid } where cityID = { id }", Globals.Connection);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Düzenleme başarısız! Aynı isme sahip başka bir şehir daha mevcut. { ex.Message }", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (DataGridViewRow r in gridCities.Rows)
            {
                if ((int)r.Cells[0].Value == id)
                {
                    r.Cells[1].Value = txtCityName.Text;

                    string ct = "";

                    foreach (DataGridViewRow c in gridCountries.Rows)
                        if ((int)c.Cells[0].Value == cid)
                        {
                            ct = c.Cells[1].Value.ToString();
                            break;
                        }

                    r.Cells[2].Value = ct;
                    r.Cells[3].Value = cid;
                    return;
                }
            }
        }

        private void CitySelect(object sender, DataGridViewCellEventArgs e)
        {
            var row = e.RowIndex;
            if (row < 0) return;
            var r = gridCities.Rows[row];
            if (row >= gridCities.RowCount - 1) return;

            var idstr = r.Cells[0].Value.ToString();
            var namestr = r.Cells[1].Value.ToString();
            var country = r.Cells[2].Value.ToString();
            var cid = r.Cells[3].Value.ToString();

            txtCityID.Text = idstr;
            txtCityName.Text = namestr;
            txtCityCountryName.Text = country;
            txtCityCountryID.Text = cid;

            txtAirportCityID.Text = idstr;
            txtAirportCityName.Text = namestr;
        }

        private void CityFormClear(object sender, EventArgs e)
        {
            txtCityName.Text = "";
            txtCityID.Text = "";
            txtCityCountryID.Text = "";
            txtCityCountryName.Text = "";
        }

        private void DeleteCity(object sender, EventArgs e)
        {
            var id = int.Parse(txtCityID.Text);
            if (MessageBox.Show($"Seçilen şehir silinecek.\nŞehire bağlı bütün bilgiler (havalimanı, uçuşlar vb) silinecek.\nDikkat bu işlem geri alınamaz!", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                MySqlCommand cmd = new MySqlCommand($"delete from cities where cityID = { id }", Globals.Connection);
                cmd.ExecuteNonQuery();
            }

            foreach (DataGridViewRow r in gridCities.Rows)
            {
                if ((int)r.Cells[0].Value == id)
                {
                    gridCities.Rows.Remove(r);
                    return;
                }
            }
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
            var id = int.Parse(txtAirlineID.Text);
            if (MessageBox.Show($"Seçilen havayolu silinecek.\nHavayoluna bağlı bütün bilgiler (uçuşlar, pilotlar) silinecek.\nDikkat bu işlem geri alınamaz!", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                MySqlCommand cmd = new MySqlCommand($"delete from airlines where airlineID = { id }", Globals.Connection);
                cmd.ExecuteNonQuery();
            }

            foreach (DataGridViewRow r in gridAirlines.Rows)
            {
                if ((int)r.Cells[0].Value == id)
                {
                    gridAirlines.Rows.Remove(r);
                    txtAirlineID.Text = "";
                    return;
                }
            }
        }

        private void SelectAirline(object sender, DataGridViewCellEventArgs e)
        {
            var row = e.RowIndex;
            if (row < 0) return;
            var r = gridAirlines.Rows[row];
            if (row >= gridAirlines.RowCount - 1) return;

            var idstr = r.Cells[0].Value.ToString();
            var namestr = r.Cells[1].Value.ToString();

            txtAirlineID.Text = idstr;
            txtAirlineName.Text = namestr;

            txtPilotAirlineID.Text = idstr;
            txtPilotAirlineName.Text = namestr;
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
        #region "Pilotlar"

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

        #endregion
        #region "Adminler"

        private void AddAirportAdmin(object sender, EventArgs e)
        {
            string name = txtAAdminName.Text.Trim();
            string surname = txtAAdminSurname.Text.Trim();
            string username = txtAAdminUsername.Text;
            string password = txtAAdminPassword.Text;
            string tc = txtAAdminTC.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("İsim / Soy isim / Kullanıcı Adı / Şifre alanları boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!tc.IsTCValid())
            {
                MessageBox.Show("TC Kimlik numarası alanı 11 karakter ve sadece sayılardan oluşmalıdır!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MySqlCommand cmd = new MySqlCommand($"insert into airport_admins (name, surname, username, password, tc) values ('{ name }', '{ surname }', '{ username }', '{ password }', '{ tc }')", Globals.Connection);
            try
            {
                int rows = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Aynı TC Kimlik numarasına ya da kullanıcı adına sahip başka bir yönetici zaten sistemde kayıtlı! { ex.Message }", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            cmd = new MySqlCommand("select max(adminID) from airport_admins", Globals.Connection);

            var rd = cmd.ExecuteReader();
            rd.Read();

            var id = rd.GetInt32(0);
            txtAAdminID.Text = id.ToString();

            rd.Close();

            MessageBox.Show($"Admin ({ name + " " + surname }) eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            gridAirportAdmins.Rows.Add(id, tc, name, surname, username, password);
        }

        private void SelectAirportAdmin(object sender, DataGridViewCellEventArgs e)
        {
            var row = e.RowIndex;
            if (row < 0) return;
            var r = gridAirportAdmins.Rows[row];
            if (row >= gridAirportAdmins.RowCount - 1) return;

            var idstr = r.Cells[0].Value.ToString();
            var tcstr = r.Cells[1].Value.ToString();
            var name = r.Cells[2].Value.ToString();
            var surname = r.Cells[3].Value.ToString();
            var username = r.Cells[4].Value.ToString();
            var password = r.Cells[5].Value.ToString();

            txtAAdminID.Text = idstr;
            txtAAdminTC.Text = tcstr;
            txtAAdminName.Text = name;
            txtAAdminSurname.Text = surname;
            txtAAdminUsername.Text = username;
            txtAAdminPassword.Text = password;

            txtAirportAdminID.Text = idstr;
            txtAirportAdminName.Text = name + " " + surname;
        }

        #endregion
        #region "Havalimanları"

        private void AddAirport(object sender, EventArgs e)
        {
            string name = txtAirportName.Text.Trim();
            string city = txtAirportCityID.Text.Trim();
            string admin = txtAirportAdminID.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(city) || string.IsNullOrEmpty(admin))
            {
                MessageBox.Show("Havalimanı İsmi / Şehir / Yönetici alanları boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MySqlCommand cmd = new MySqlCommand($"insert into airports (name, cityID, adminID) values ('{ name }', { city }, { admin })", Globals.Connection);
            try
            {
                int rows = cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                MessageBox.Show($"Aynı yöneticiye ya da isme sahip başka bir havalimanı zaten sistemde kayıtlı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            cmd = new MySqlCommand("select max(airportID) from airports", Globals.Connection);

            var rd = cmd.ExecuteReader();
            rd.Read();

            var id = rd.GetInt32(0);
            txtPilotID.Text = id.ToString();

            rd.Close();

            MessageBox.Show($"Havalimanı ({ name }) eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            gridAirports.Rows.Add(id, name, txtAirportCityName.Text, txtAirportAdminName.Text, admin, city);
        }

        #endregion

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
                MessageBox.Show($"Uçak türü ({ type }) zaten sistemde kayıtlı! { ex.Message }" , "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        private void SelectPlaneType(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void RefreshData(object sender, EventArgs e)
        {
            var src = contextRefresh.SourceControl;

            if (src == gridCountries) r_countries();
            else if (src == gridAirportAdmins) r_airportAdmins();
            else if (src == gridCities) r_cities();
            else if (src == gridAirlines) r_airlines();
            else if (src == gridPilots) r_pilots();
            else if (src == gridAirports) r_airports();
            else if (src == gridPlaneTypes) r_types();
            else if (src == gridPlanes) r_planes();
            else if (src == gridPlaneModels) r_models();
        }

        private void DeleteModel(object sender, EventArgs e)
        {
            var id = int.Parse(txtPlaneModelID.Text);
            if (MessageBox.Show($"Seçilen uçak modeli silinecek.\nUçak modeline bağlı bütün bilgiler (uçaklar, uçuşlar vb) silinecek.\nDikkat bu işlem geri alınamaz!", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                MySqlCommand cmd = new MySqlCommand($"delete from models where modelID = { id }", Globals.Connection);
                cmd.ExecuteNonQuery();
            }

            foreach (DataGridViewRow r in gridPlaneModels.Rows)
            {
                if ((int)r.Cells[0].Value == id)
                {
                    gridPlaneModels.Rows.Remove(r);
                    return;
                }
            }
        }

        private void DeletePlaneTypes(object sender, EventArgs e)
        {
            var id = int.Parse(txtPlaneTypeID.Text);
            if (MessageBox.Show($"Seçilen uçak tipi silinecek.\nUçak tipine bağlı bütün bilgiler (uçak modelleri, uçuşlar, uçaklar vb) silinecek.\nDikkat bu işlem geri alınamaz!", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                MySqlCommand cmd = new MySqlCommand($"delete from types where typeID = { id }", Globals.Connection);
                cmd.ExecuteNonQuery();
            }

            foreach (DataGridViewRow r in gridPlaneTypes.Rows)
            {
                if ((int)r.Cells[0].Value == id)
                {
                    gridPlaneTypes.Rows.Remove(r);
                    return;
                }
            }
        }

        private void DeletePlane(object sender, EventArgs e)
        {
            var id = int.Parse(txtPlaneID.Text);
            if (MessageBox.Show($"Seçilen uçak  silinecek.\nUçağa bağlı bütün bilgiler (uçuşlar vb) silinecek.\nDikkat bu işlem geri alınamaz!", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                MySqlCommand cmd = new MySqlCommand($"delete from planes where planeID = { id }", Globals.Connection);
                cmd.ExecuteNonQuery();
            }

            foreach (DataGridViewRow r in gridPlanes.Rows)
            {
                if ((int)r.Cells[0].Value == id)
                {
                    gridPlanes.Rows.Remove(r);
                    return;
                }
            }
        }
    }
}
