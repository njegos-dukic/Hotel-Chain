using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace LanacHotelaServiceLayer
{
    public class ConnectionPool
    {
        private static ConnectionPool connectionPool = null;
        private static readonly List<MySqlConnection> connections = new List<MySqlConnection>();

        private ConnectionPool() { }

        public static ConnectionPool GetConnectionPool()
        {
            if (connectionPool == null)
            {
                connectionPool = new ConnectionPool();
                for (int i = 0; i < Constants.INITIAL_CONNECTIONS; i++)
                {
                    connections.Add(new MySqlConnection(Constants.CONNECTION_STRING));
                }
            }

            return connectionPool;
        }

        public MySqlConnection GetOpenConnection()
        {
            if (connections.Count < 5)
            {
                for (int i = 0; i < Constants.CONNECTIONS_INCREMENT; i++)
                {
                    connections.Add(new MySqlConnection(Constants.CONNECTION_STRING));
                }
            }

            MySqlConnection connection = connections[0];
            connections.RemoveAt(0);
            connection.Open();
            return connection;
        }

        public void ReleaseConnection(MySqlConnection connection)
        {
            connection.Close();
            connections.Add(new MySqlConnection(Constants.CONNECTION_STRING));
        }
    }
}
