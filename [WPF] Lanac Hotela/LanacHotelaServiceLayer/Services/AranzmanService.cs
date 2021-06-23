using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LanacHotelaServiceLayer
{
    public class AranzmanService : IAranzmanService
    {
        public async Task<Aranzman> GetById(int id)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();

            try
            {
                string query = @"SELECT * FROM ARANZMAN WHERE aranzmanID = @ID;";
                MySqlCommand command = new MySqlCommand(query, cnn);
                command.Parameters.AddWithValue("@ID", id);

                using System.Data.Common.DbDataReader rdr = await command.ExecuteReaderAsync();

                if (rdr.Read())
                {
                    return new Aranzman(rdr.GetInt32(0), rdr.GetDateTime(1), rdr.GetDateTime(2), rdr.GetBoolean(3), rdr.GetBoolean(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7));
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

        public async Task<List<Aranzman>> GetAll()
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            List<Aranzman> aranzmani = new List<Aranzman>();

            try
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM ARANZMAN;", cnn);

                using System.Data.Common.DbDataReader rdr = await command.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    aranzmani.Add(new Aranzman(rdr.GetInt32(0), rdr.GetDateTime(1), rdr.GetDateTime(2), rdr.GetBoolean(3), rdr.GetBoolean(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7)));
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

            return aranzmani;
        }

        public async Task<int> Insert(Aranzman a)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            int result = 0;

            // Da li je termin zauzet.
            TimeSpan noTime = new TimeSpan(0);
            foreach (Aranzman aranzman in await GetAllForHotel(a.HotelID))
            {
                if (((a.Pocetak.Subtract(aranzman.Pocetak) >= noTime &&
                    a.Pocetak.Subtract(aranzman.Kraj) <= noTime) ||

                    (a.Kraj.Subtract(aranzman.Pocetak) >= noTime &&
                    a.Kraj.Subtract(aranzman.Kraj) <= noTime)) &&

                    a.SobaID == aranzman.SobaID &&
                    !aranzman.JeOtkazan)
                {
                    return -1;
                }
            }

            try
            {
                string pocetak = $"{a.Pocetak.Date.Year}{(a.Pocetak.Date.Month < 10 ? "0" + a.Pocetak.Date.Month.ToString() : a.Pocetak.Date.Month)}{(a.Pocetak.Date.Day < 10 ? "0" + a.Pocetak.Date.Day.ToString() : a.Pocetak.Date.Day)}";
                string kraj = $"{a.Kraj.Date.Year}{(a.Kraj.Date.Month < 10 ? "0" + a.Kraj.Date.Month.ToString() : a.Kraj.Date.Month)}{(a.Kraj.Date.Day < 10 ? "0" + a.Kraj.Date.Day.ToString() : a.Kraj.Date.Day)}";

                string query = @"INSERT INTO ARANZMAN VALUES (0, @Pocetak, @Kraj, @JeOtkazan, @JeZavrsen, @HotelID, @GostID, @SobaID);";
                MySqlCommand command = new MySqlCommand(query, cnn);

                command.Parameters.AddWithValue("@Pocetak", pocetak);
                command.Parameters.AddWithValue("@Kraj", kraj);
                command.Parameters.AddWithValue("@JeOtkazan", (a.JeOtkazan == true ? 1 : 0));
                command.Parameters.AddWithValue("@JeZavrsen", (a.JeZavrsen == true ? 1 : 0));
                command.Parameters.AddWithValue("@HotelID", a.HotelID);
                command.Parameters.AddWithValue("@GostID", a.GostID);
                command.Parameters.AddWithValue("@SobaID", a.SobaID);

                result = await command.ExecuteNonQueryAsync();

                command = new MySqlCommand(@"CALL KreirajRacuneAranzmana();", cnn);
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return result;
        }

        public async Task<int> Update(AranzmanDetaljno ad)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            int result = 0;

            try
            {
                AranzmanService aranzmanService = new AranzmanService();
                Aranzman beingUpdated = await aranzmanService.GetById(ad.AranzmanID);
                if (beingUpdated.JeOtkazan == ad.Otkazan && beingUpdated.JeZavrsen == ad.Zavrsen)
                {
                    return -1;
                }

                string query = "UPDATE ARANZMAN SET jeOtkazan = @Otkazan, jeZavrsen = @Zavrsen WHERE aranzmanID = @ID;";
                MySqlCommand command = new MySqlCommand(query, cnn);

                command.Parameters.AddWithValue("@Otkazan", ad.Otkazan);
                command.Parameters.AddWithValue("@Zavrsen", ad.Zavrsen);
                command.Parameters.AddWithValue("@ID", ad.AranzmanID);

                result = await command.ExecuteNonQueryAsync();

                command = new MySqlCommand(@"CALL KreirajRacuneAranzmana();", cnn);
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return result;
        }

        public async Task<int> Delete(int id)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            int result = 0;

            try
            {
                string query = "CALL ObrisiAranzman(@ID);";
                MySqlCommand command = new MySqlCommand(query, cnn);
                command.Parameters.AddWithValue("@ID", id);

                result = await command.ExecuteNonQueryAsync();

                command = new MySqlCommand(@"CALL KreirajRacuneAranzmana();", cnn);
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return result;
        }

        public async Task<List<Aranzman>> GetAllForHotel(int hotelID)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            List<Aranzman> aranzmani = new List<Aranzman>();

            try
            {
                string query = "SELECT * FROM ARANZMAN WHERE hotelID = @HotelID;";
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

        public async Task<List<Aranzman>> GetAllForGuest(int gostID)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            List<Aranzman> aranzmani = new List<Aranzman>();

            try
            {
                string query = "SELECT * FROM ARANZMAN WHERE gostID = @GostID;";
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
    }
}