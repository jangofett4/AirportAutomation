using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace AirportAutomation
{
    public static class Globals
    {
        public static MySqlConnection Connection = new MySqlConnection($"Server={ MySqlCredentials.MySqlAddress };Database=airport;Uid={ MySqlCredentials.MySqlUsername };Pwd={ MySqlCredentials.MySqlPassword };");

        public static string ConnectedAdminUsername = "";
        public static string ConnectedAdminPassword = "";
        public static int ConnectedAdminID = 0;
        public static int ConnectedAdminAirportID = 0;
    }
}
