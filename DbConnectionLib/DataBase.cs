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
        public event Action<string> Success;

        #endregion

        #region Constructor

        public DataBase()
        {
            _db = new MySqlConnection();
            _command = new MySqlCommand();
        }

        public DataBase(Action<string> info, Action<string> error, Action<string> success)
        {
            _db = new MySqlConnection();
            _command = new MySqlCommand();

            Info += info;
            Error += error;
            Success += success;
        }

        #endregion

        #region Init

        public bool Init(string path)
        {
            Info?.Invoke("�������������");
            var connection = DbConnection.DeserializeJson(path);

            if (connection is null)
            {
                Error?.Invoke("������ �������������");
                return false;
            }
            _db.ConnectionString = connection.ToString();
            _command.Connection = _db;

            Success?.Invoke("����� �������������");
            return true;
        }

        public bool Init()
        {
            return Init("db_connection.json");
        }

        #endregion

        #region Request

        private bool CheckSql(string sql)
        {
            return !string.IsNullOrEmpty(sql) && !string.IsNullOrWhiteSpace(sql);
        }

        private bool CheckConnectToDb()
        {
            try
            {
                _db.Open();
                Success?.Invoke("�������� �������� ��");
                return true;
            }
            catch (Exception)
            {
                Error?.Invoke("������ �������� ��");
                return false;
            }
        }

        public bool ExecuteSelect(in string sql, out MySqlDataReader outputData)
        {
            Info?.Invoke("���������� ������� SELECT");

            outputData = null;

            if (!CheckSql(sql))
            {
                Error?.Invoke("������� ������ ������ SQL");
                return false;
            }

            if (!CheckConnectToDb())
            {
                Error?.Invoke("������ ����������� � ��");
                return false;
            }

            _command.CommandText = sql;
            outputData = _command.ExecuteReader();
            Info?.Invoke("���������� ������� SQL �� ������� ��");

            _db.Close();
            Info?.Invoke("�������� ��");

            return outputData.HasRows;
        }

        public bool ExecuteNotSelect(in string sql, out int countRows)
        {
            Info?.Invoke("���������� ������� ��������� �� SELECT");

            countRows = 0;

            if (!CheckSql(sql))
            {
                Error?.Invoke("������� ������ ������ SQL");
                return false;
            }

            if (!CheckConnectToDb())
            {
                Error?.Invoke("������ ����������� � ��");
                return false;
            }

            _command.CommandText = sql;
            countRows = _command.ExecuteNonQuery();
            Info?.Invoke("���������� ������� SQL �� ������� ��");

            _db.Close();
            Info?.Invoke("�������� ��");

            return countRows > 0;
        }


        #endregion
    }
}
