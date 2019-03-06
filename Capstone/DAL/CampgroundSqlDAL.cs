using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class CampgroundSqlDAL
    {
        private const string SQL_GetCampgrounds = "SELECT * FROM campground JOIN park ON park.park_id = campground.park_id WHERE park.name = @name";
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog = NationalParkReservation; Integrated Security = True";

        public List<Campground> GetCampgrounds(string name)
        {
            List<Campground> campgrounds = new List<Campground>();

            try
            {
                using(SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_GetCampgrounds, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while(reader.Read())
                    {
                        Campground campground = new Campground();
                        campground.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        campground.Name = Convert.ToString(reader["name"]);
                        campground.OpenMonth = Convert.ToInt32(reader["open_from_mm"]);
                        campground.ClosingMonth = Convert.ToInt32(reader["open_to_mm"]);
                        campground.DailyFee = Convert.ToDecimal(reader["daily_fee"]);
                        campgrounds.Add(campground);
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            return campgrounds;
        }
    }
}
