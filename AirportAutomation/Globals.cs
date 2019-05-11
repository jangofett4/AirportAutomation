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
        public static MySqlConnection Connection = new MySqlConnection("Server=localhost;Database=airport;Uid=root;Pwd=toor;");

        public static string ConnectedAdminUsername = "";
        public static string ConnectedAdminPassword = "";
    }
}
