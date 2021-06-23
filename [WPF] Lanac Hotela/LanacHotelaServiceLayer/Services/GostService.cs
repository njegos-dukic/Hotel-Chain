using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LanacHotelaServiceLayer
{
    public class GostService : IReadable<Gost>, IInsertable<Gost>, IUniquelyUpdateable<Gost>, IUniquelyDeleteable
    {
        public async Task<List<Gost>> GetAll()
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            List<Gost> gosti = new List<Gost>();

            try
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM GOST;", cnn);

                using System.Data.Common.DbDataReader rdr = await command.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    gosti.Add(new Gost(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3)));
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

            return gosti;
        }

        public async Task<int> Insert(Gost g)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            int result = 0;

            try
            {
                foreach (Gost gost in await GetAll())
                {
                    if (gost.JMBG == g.JMBG)
                    {
                        return -1;
                    }
                }

                string query = @"INSERT INTO GOST VALUES(0, @JMBG, @Ime, @Prezime);";
                MySqlCommand command = new MySqlCommand(query, cnn);

                command.Parameters.AddWithValue("@JMBG", g.JMBG);
                command.Parameters.AddWithValue("@Ime", g.Ime);
                command.Parameters.AddWithValue("@Prezime", g.Prezime);

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

        public async Task<int> Update(Gost g)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            int result = 0;

            try
            {
                foreach (Gost gost in await GetAll())
                {
                    if (gost.GostID != g.GostID && gost.JMBG == g.JMBG)
                    {
                        return -1;
                    }
                }

                string query = "UPDATE GOST SET JMBG = @JMBG, ime = @Ime, prezime = @Prezime WHERE gostID = @ID;";
                MySqlCommand command = new MySqlCommand(query, cnn);

                command.Parameters.AddWithValue("@JMBG", g.JMBG);
                command.Parameters.AddWithValue("@Ime", g.Ime);
                command.Parameters.AddWithValue("@Prezime", g.Prezime);
                command.Parameters.AddWithValue("@ID", g.GostID);

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

        public async Task<List<Gost>> GetAllReservations(int gostID)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            List<Gost> gosti = new List<Gost>();

            try
            {
                string query = "SELECT * FROM ARANZMAN WHERE gostID = @GostID;";
                MySqlCommand command = new MySqlCommand(query, cnn);
                command.Parameters.AddWithValue("@GostID", gostID);

                using System.Data.Common.DbDataReader rdr = await command.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    gosti.Add(new Gost(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3)));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return gosti;
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return gosti;
        }

        public async Task<int> Delete(int id)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            int result = 0;

            try
            {
                string query = "CALL ObrisiGosta(@ID);";
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
