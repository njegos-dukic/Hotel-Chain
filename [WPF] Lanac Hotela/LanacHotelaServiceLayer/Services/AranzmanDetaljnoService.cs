using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LanacHotelaServiceLayer
{
    public class AranzmanDetaljnoService : IAranzmanDetaljno
    {
        public async Task<List<AranzmanDetaljno>> GetAll()
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            List<AranzmanDetaljno> aranzmani = new List<AranzmanDetaljno>();

            try
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM SviAranzmaniDetaljno;", cnn);

                using System.Data.Common.DbDataReader rdr = await command.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    aranzmani.Add(new AranzmanDetaljno(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetDateTime(5), rdr.GetDateTime(6), rdr.GetBoolean(7), rdr.GetBoolean(8), rdr.GetInt32(9), rdr.GetDouble(10), rdr.GetInt32(11)));
                }
            }
            catch (Exception)
            {
                return aranzmani;
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return aranzmani;
        }

        public async Task<List<AranzmanDetaljno>> GetAllForHotel(int hotelID)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            List<AranzmanDetaljno> aranzmani = new List<AranzmanDetaljno>();

            try
            {
                string query = @"SELECT * FROM SviAranzmaniDetaljno WHERE `Hotel ID` = @HotelID;";
                MySqlCommand command = new MySqlCommand(query, cnn);
                command.Parameters.AddWithValue("@HotelID", hotelID);

                using System.Data.Common.DbDataReader rdr = await command.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    aranzmani.Add(new AranzmanDetaljno(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetDateTime(5), rdr.GetDateTime(6), rdr.GetBoolean(7), rdr.GetBoolean(8), rdr.GetInt32(9), rdr.GetDouble(10), rdr.GetInt32(11)));
                }
            }
            catch (Exception)
            {
                return aranzmani;
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return aranzmani;
        }
    }
}
