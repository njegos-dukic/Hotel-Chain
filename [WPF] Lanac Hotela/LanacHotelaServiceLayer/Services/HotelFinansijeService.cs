using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LanacHotelaServiceLayer
{
    public class HotelFinansijeService : IUniquelyReadable<HotelFinansije>, IReadable<HotelFinansije>
    {
        public async Task<HotelFinansije> GetById(int id)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();

            try
            {
                MySqlCommand command = new MySqlCommand("CALL KreirajRacuneAranzmana();", cnn);
                await command.ExecuteNonQueryAsync();

                string query = "SELECT * FROM FinansijeHotela WHERE `Hotel ID` = @ID;";
                command = new MySqlCommand(query, cnn);
                command.Parameters.AddWithValue("@ID", id);

                using System.Data.Common.DbDataReader rdr = await command.ExecuteReaderAsync();

                if (rdr.Read())
                {
                    return new HotelFinansije(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetDouble(3));
                }
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return null;
        }

        public async Task<List<HotelFinansije>> GetAll()
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            List<HotelFinansije> finansije = new List<HotelFinansije>();

            try
            {
                MySqlCommand command = new MySqlCommand("CALL KreirajRacuneAranzmana();", cnn);
                await command.ExecuteNonQueryAsync();

                command = new MySqlCommand("SELECT * FROM FinansijeHotela;", cnn);

                using System.Data.Common.DbDataReader rdr = await command.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    finansije.Add(new HotelFinansije(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetDouble(3)));
                }
            }
            catch (Exception)
            {
                return finansije;
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return finansije;
        }
    }
}
