using MySql.Data.MySqlClient;
using System;

namespace DbConnectionLib
{
    public class DataBase
    {
        #region Values

        private MySqlConnection _db;
        private MySqlCommand _command;

        public event Action<string> Info;
        public event Action<string> Error;
        public event Action<string>

        #endregion 
    }
}
