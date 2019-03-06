using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;
using Capstone.DAL;
using Capstone.Models;
using System;

namespace Capstone.Tests
{
    [TestClass]
    public class ReservationSQLDALTest
    {
        int testInsert;
        // Define scope
        TransactionScope tran;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog = NationalParkReservation; Integrated Security = True";

        [TestInitialize]
        public void Initialize()
        {
            // Initialize a new transaction scope. This automatically begins the transaction.
            tran = new TransactionScope();
            // Open a SqlConnection object using the active transaction
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                conn.Open();

                //Insert a Dummy Record for Country                
                cmd = new SqlCommand("INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (37, 'Alec', '2019-07-04', '2019-07-24') SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                testInsert = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();

        }


        [TestMethod]
        public void SearchForReservationTest()
        {
            string park = "Acadia";
            string campground = "Blackwoods";
            DateTime arrivalDate = DateTime.MinValue;
            DateTime departureDate = DateTime.MinValue;
            List<Campsite> test = new List<Campsite>();
            ReservationSqlDAL reservationSqlDAL = new ReservationSqlDAL();
            test = reservationSqlDAL.SearchForReservation(park, campground, arrivalDate, departureDate);
            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void MakeReservationTest()
        {
            int result;
            string name = "Alec2";
            int campsiteId = 37;
            DateTime arrivalDate = DateTime.Parse("2019-07-04");
            DateTime departureDate = DateTime.Parse("2019-07-24");
            ReservationSqlDAL reservationSqlDAL = new ReservationSqlDAL();
            result = reservationSqlDAL.MakeReservation(name, campsiteId, arrivalDate, departureDate);
            Assert.AreEqual(testInsert + 1, result);
        }

        [TestMethod]
        public void GetParkReservationsTest()
        {
            string parkName = "Acadia";
            DateTime currentDate = DateTime.MinValue;
            DateTime futureMonthDate = DateTime.MinValue;
            ReservationSqlDAL reservationSqlDAL = new ReservationSqlDAL();
            List<Reservation> result = reservationSqlDAL.GetParkReversations(parkName, currentDate, futureMonthDate);
            Assert.IsNotNull(result);

        }
    }
}
