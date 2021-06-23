using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LanacHotelaServiceLayer
{
    public class KontaktService : IKontaktService
    {
        public async Task<Kontakt> GetById(int id)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();

            try
            {
                string query = @"SELECT * FROM KONTAKT WHERE kontaktID = @ID;";
                MySqlCommand command = new MySqlCommand(query, cnn);
                command.Parameters.AddWithValue("@ID", id);

                using System.Data.Common.DbDataReader rdr = await command.ExecuteReaderAsync();

                if (rdr.Read())
                {
                    return new Kontakt(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), (rdr.IsDBNull(3) ? 0 : rdr.GetInt32(3)), (rdr.IsDBNull(4) ? 0 : rdr.GetInt32(4)));
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

        public async Task<List<Aranzman>> GetAllForGuest(int gostID)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            List<Aranzman> aranzmani = new List<Aranzman>();

            try
            {
                string query = "SELECT * FROM KONTAKT WHERE gostID = @GostID;";
                MySqlCommand command = new MySqlCommand(query, cnn);
                command.Parameters.AddWithValue("@GostID", gostID);

                using System.Data.Common.DbDataReader rdr = await command.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    aranzmani.Add(new Aranzman(rdr.GetInt32(0), rdr.GetDateTime(1), rdr.GetDateTime(2), rdr.GetBoolean(3), rdr.GetBoolean(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7)));
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

        public async Task<List<Aranzman>> GetAllForHotel(int hotelID)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            List<Aranzman> aranzmani = new List<Aranzman>();

            try
            {
                string query = "SELECT * FROM KONTAKT WHERE hotelID = @HotelID;";
                MySqlCommand command = new MySqlCommand(query, cnn);
                command.Parameters.AddWithValue("@HotelID", hotelID);

                using System.Data.Common.DbDataReader rdr = await command.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    aranzmani.Add(new Aranzman(rdr.GetInt32(0), rdr.GetDateTime(1), rdr.GetDateTime(2), rdr.GetBoolean(3), rdr.GetBoolean(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7)));
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

        public async Task<List<Kontakt>> GetAll()
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            List<Kontakt> kontakti = new List<Kontakt>();

            try
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM KONTAKT;", cnn);

                using System.Data.Common.DbDataReader rdr = await command.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    kontakti.Add(new Kontakt(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), (rdr.IsDBNull(3) ? 0 : rdr.GetInt32(3)), (rdr.IsDBNull(4) ? 0 : rdr.GetInt32(4))));
                }
            }
            catch (Exception)
            {
                return kontakti;
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return kontakti;
        }

        public async Task<int> Insert(Kontakt t)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            int result = 0;

            try
            {
                foreach (Kontakt k in await GetAll())
                {
                    if (t.Equals(k))
                    {
                        return -1;
                    }
                }

                string query = @"INSERT INTO KONTAKT VALUES (0, @Tip, @Info, @H, @G);";
                MySqlCommand command = new MySqlCommand(query, cnn);

                command.Parameters.AddWithValue("@Tip", t.Tip);
                command.Parameters.AddWithValue("@Info", t.Info);
                command.Parameters.AddWithValue("@H", DBNull.Value);
                command.Parameters["@H"].IsNullable = true;
                command.Parameters.AddWithValue("@G", DBNull.Value);
                command.Parameters["@G"].IsNullable = true;

                result = await command.ExecuteNonQueryAsync();
            }
            catch (Exception)
            {
                result = -1;
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return result;
        }

        public async Task<int> Update(Kontakt t)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            int result = 0;

            try
            {
                KontaktService kontaktService = new KontaktService();
                Kontakt beingUpdated = await kontaktService.GetById(t.KontaktID);

                if (beingUpdated.Equals(t))
                {
                    return -1;
                }

                string query = @"UPDATE KONTAKT SET tip = @Tip, info = @Info, hotelID = @HotelID, gostID = @GostID WHERE kontaktID = @ID;";
                MySqlCommand command = new MySqlCommand(query, cnn);

                command = new MySqlCommand(query, cnn);
                command.Parameters.AddWithValue("@Tip", t.Tip.ToLower());
                command.Parameters.AddWithValue("@Info", t.Info.ToLower());
                try
                {
                    command.Parameters.AddWithValue("@HotelID", t.HotelID == 0 ? DBNull.Value : t.HotelID);
                }
                catch (Exception)
                {
                    command.Parameters["@HotelID"].IsNullable = true;
                }
                try
                {
                    command.Parameters.AddWithValue("@GostID", t.GostID == 0 ? DBNull.Value : t.GostID);
                }
                catch (Exception)
                {
                    command.Parameters["@GostID"].IsNullable = true;
                }
                command.Parameters.AddWithValue("@ID", t.KontaktID);

                result = await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result = -1;
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return result;
        }

        public async Task<int> Update(Kontakt t, int hotelID)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            int result = 0;

            try
            {
                KontaktService kontaktService = new KontaktService();
                Kontakt beingUpdated = await kontaktService.GetById(t.KontaktID);

                string query = @"UPDATE KONTAKT SET hotelID = @HotelID WHERE kontaktID = @KontaktID;";
                MySqlCommand command = new MySqlCommand(query, cnn);

                command = new MySqlCommand(query, cnn);
                command.Parameters.AddWithValue("@HotelID", hotelID);
                command.Parameters.AddWithValue("@KontaktID", t.KontaktID);

                result = await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result = -1;
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return result;
        }

        // TODO: Testiraj
        public async Task<int> Delete(int id)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            int result = 0;

            try
            {
                string query = @"ObrisiKontakt(@KontaktID);";
                MySqlCommand command = new MySqlCommand(query, cnn);
                command.Parameters.AddWithValue("@KontaktID", id);
                result = await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                result = -1;
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return result;
        }

        public async Task<List<Kontakt>> GetAllUnassigned()
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            List<Kontakt> kontakti = new List<Kontakt>();

            try
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM KONTAKT WHERE hotelID IS NULL AND gostID IS NULL;", cnn);

                using System.Data.Common.DbDataReader rdr = await command.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    kontakti.Add(new Kontakt(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), (rdr.IsDBNull(3) ? 0 : rdr.GetInt32(3)), (rdr.IsDBNull(4) ? 0 : rdr.GetInt32(4))));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return kontakti;
        }
    }
}
