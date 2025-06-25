using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HRMSDAL.Helper
{
    public static class DBHelper
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

        private static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        private static SqlCommand CreateCommand(string commandText, SqlConnection connection, CommandType commandType, IDataParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand(commandText, connection)
            {
                CommandType = commandType
            };

            if (parameters != null)
            {
                foreach (IDataParameter param in parameters)
                {
                    cmd.Parameters.Add(param);
                }
            }

            return cmd;
        }

        public static int ExecuteNonQuery(string commandText, CommandType commandType, IDataParameter[] parameters = null)
        {
            using (var connection = GetConnection())
            using (var command = CreateCommand(commandText, connection, commandType, parameters))
            {
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(string commandText, CommandType commandType, IDataParameter[] parameters = null)
        {
            using (var connection = GetConnection())
            using (var command = CreateCommand(commandText, connection, commandType, parameters))
            {
                connection.Open();
                return command.ExecuteScalar();
            }
        }

        public static List<Dictionary<string, object>> ExecuteReader(string commandText, CommandType commandType, IDataParameter[] parameters = null)
        {
            using (var connection = GetConnection())
            using (var command = CreateCommand(commandText, connection, commandType, parameters))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    var results = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }

                        results.Add(row);
                    }

                    return results;
                }
            }
        }
    }
}