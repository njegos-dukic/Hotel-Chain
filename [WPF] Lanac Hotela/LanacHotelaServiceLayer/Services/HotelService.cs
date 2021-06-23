using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LanacHotelaServiceLayer
{
    public class HotelService : IUniquelyReadable<Hotel>, IReadable<Hotel>, IInsertable<Hotel>, IUniquelyUpdateable<Hotel>, IUniquelyDeleteable
    {
        public async Task<Hotel> GetById(int id)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();

            try
            {
                string query = "SELECT * FROM HOTEL WHERE hotelID = @ID;";
                MySqlCommand command = new MySqlCommand(query, cnn);
                command.Parameters.AddWithValue("@ID", id);

                using System.Data.Common.DbDataReader rdr = await command.ExecuteReaderAsync();

                if (rdr.Read())
                {
                    return new Hotel(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(5), rdr.GetInt32(6), rdr.GetString(7));
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

        public async Task<List<Hotel>> GetAll()
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            List<Hotel> hotels = new List<Hotel>();

            try
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM HOTEL;", cnn);

                using System.Data.Common.DbDataReader rdr = await command.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    hotels.Add(new Hotel(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(5), rdr.GetInt32(6), rdr.GetString(7)));
                }
            }
            catch (Exception)
            {
                return hotels;
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return hotels;
        }

        public async Task<int> Insert(Hotel h)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            int result = 0;

            try
            {
                string query = @"INSERT INTO HOTEL VALUES (0, @Ime, @BrojZvjezdica, @Ulica, @Broj, @Grad, @ZIP, @Drzava);";
                MySqlCommand command = new MySqlCommand(query, cnn);

                command.Parameters.AddWithValue("@Ime", h.Ime);
                command.Parameters.AddWithValue("@BrojZvjezdica", h.BrojZvjezdica);
                command.Parameters.AddWithValue("@Ulica", h.Ulica);
                command.Parameters.AddWithValue("@Broj", h.Broj);
                command.Parameters.AddWithValue("@Grad", h.Grad);
                command.Parameters.AddWithValue("@ZIP", h.PostanskiBroj);
                command.Parameters.AddWithValue("@Drzava", h.Drzava);

                result = await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return result;
        }

        public async Task<int> Update(Hotel h)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            int result = 0;

            try
            {
                foreach (Hotel hotel in await GetAll())
                {
                    if (hotel.Equals(h))
                    {
                        return -1;
                    }
                }

                string query = "UPDATE HOTEL SET ime = @Ime, brojZvjezdica = @BrojZvjezdica, ulica = @Ulica, broj = @Broj, grad = @Grad, postanskiBroj = @ZIP, drzava = @Drzava WHERE hotelID = @ID;";
                MySqlCommand command = new MySqlCommand(query, cnn);

                command.Parameters.AddWithValue("@Ime", h.Ime);
                command.Parameters.AddWithValue("@BrojZvjezdica", h.BrojZvjezdica);
                command.Parameters.AddWithValue("@Ulica", h.Ulica);
                command.Parameters.AddWithValue("@Broj", h.Broj);
                command.Parameters.AddWithValue("@Grad", h.Grad);
                command.Parameters.AddWithValue("@ZIP", h.PostanskiBroj);
                command.Parameters.AddWithValue("@Drzava", h.Drzava);

                command.Parameters.AddWithValue("@ID", h.HotelID);

                result = await command.ExecuteNonQueryAsync();
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
                string query = "CALL ObrisiHotel(@ID);";
                MySqlCommand command = new MySqlCommand(query, cnn);
                command.Parameters.AddWithValue("@ID", id);

                result = await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return result;
        }
    }
}
