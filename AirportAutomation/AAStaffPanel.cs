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
    }
}
