using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LanacHotelaServiceLayer
{
    public class SobaService : IReadable<Soba>, IInsertable<Soba>, IUniquelyUpdateable<Soba>, IUniquelyDeleteable
    {
        public async Task<List<Soba>> GetAll()
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            List<Soba> sobe = new List<Soba>();

            try
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM SOBA;", cnn);

                using System.Data.Common.DbDataReader rdr = await command.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    sobe.Add(new Soba(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetBoolean(4), rdr.GetBoolean(5), rdr.GetDouble(6), rdr.GetInt32(7)));
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

            return sobe;
        }

        public async Task<int> Insert(Soba s)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            int result = 0;

            try
            {
                foreach (Soba soba in await GetAll())
                {
                    if (soba.Equals(s))
                    {
                        return -1;
                    }
                }

                string query = @"INSERT INTO SOBA VALUES (0, @BrojSprata, @BrojSobe, @BrojKreveta, @ImaTV, @ImaKlimu, @CijenaNocenja, @HotelID);";
                MySqlCommand command = new MySqlCommand(query, cnn);

                command.Parameters.AddWithValue("@BrojSprata", s.BrojSprata);
                command.Parameters.AddWithValue("@BrojSobe", s.BrojSobe);
                command.Parameters.AddWithValue("@BrojKreveta", s.BrojKreveta);
                command.Parameters.AddWithValue("@ImaTV", s.ImaTV);
                command.Parameters.AddWithValue("@ImaKlimu", s.ImaKlimu);
                command.Parameters.AddWithValue("@CijenaNocenja", s.CijenaNocenja);
                command.Parameters.AddWithValue("@HotelID", s.HotelID);

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

        public async Task<int> Update(Soba s)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            int result = 0;

            try
            {
                int.Parse(s.BrojSprata.ToString());
                int.Parse(s.BrojSobe.ToString());
                int.Parse(s.BrojKreveta.ToString());
                double.Parse(s.CijenaNocenja.ToString());

                foreach (Soba soba in await GetAll())
                {
                    if (soba.Equals(s))
                    {
                        return -1;
                    }
                }

                string query = "UPDATE SOBA SET brojSprata = @BrojSprata, brojSobe = @BrojSobe, brojKreveta = @BrojKreveta, imaTV = @ImaTV, imaKlimu = @ImaKlimu, cijenaNocenja = @CijenaNocenja WHERE sobaID = @SobaID;";
                MySqlCommand command = new MySqlCommand(query, cnn);

                command.Parameters.AddWithValue("@BrojSprata", s.BrojSprata);
                command.Parameters.AddWithValue("@BrojSobe", s.BrojSobe);
                command.Parameters.AddWithValue("@BrojKreveta", s.BrojKreveta);
                command.Parameters.AddWithValue("@ImaTV", s.ImaTV);
                command.Parameters.AddWithValue("@ImaKlimu", s.ImaKlimu);
                command.Parameters.AddWithValue("@CijenaNocenja", s.CijenaNocenja);
                command.Parameters.AddWithValue("@SobaID", s.SobaID);

                result = await command.ExecuteNonQueryAsync();

                command = new MySqlCommand(@"CALL KreirajRacuneAranzmana();", cnn);
                await command.ExecuteNonQueryAsync();
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

        public async Task<List<Aranzman>> GetAllReservations(int sobaID)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            List<Aranzman> aranzmani = new List<Aranzman>();

            try
            {
                string query = "SELECT * FROM ARANZMAN WHERE sobaID = @SobaID;";
                MySqlCommand command = new MySqlCommand(query, cnn);
                command.Parameters.AddWithValue("@SobaID", sobaID);

                using System.Data.Common.DbDataReader rdr = await command.ExecuteReaderAsync();

                while (rdr.Read())
                {
                    aranzmani.Add(new Aranzman(rdr.GetInt32(0), rdr.GetDateTime(1), rdr.GetDateTime(2), rdr.GetBoolean(3), rdr.GetBoolean(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7)));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return aranzmani;
            }
            finally
            {
                ConnectionPool.GetConnectionPool().ReleaseConnection(cnn);
            }

            return aranzmani;
        }

        public async Task<int> Delete(int id)
        {
            MySqlConnection cnn = ConnectionPool.GetConnectionPool().GetOpenConnection();
            int result = 0;

            try
            {
                string query = "DELETE SOBA FROM SOBA WHERE sobaID = @ID;";
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
