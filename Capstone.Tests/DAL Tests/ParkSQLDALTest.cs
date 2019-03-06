using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone.Tests
{
    [TestClass]
    public class ParkSQLDALTest
    {

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

                //Insert a Dummy Record for Park             
                cmd = new SqlCommand("INSERT INTO park (name, location, establish_date, area, visitors, description) VALUES ('Fun Park', 'Ohio', '2018-08-08', 100000, 5, 'Roller Coasters and Craft Beer')", conn);
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();

        }


        [TestMethod]
        public void GetParkNames()
        {
            List<string> test = new List<string>();
            ParkSqlDAL parkSqlDAL = new ParkSqlDAL();
            test = parkSqlDAL.GetParkName();
            Assert.IsNotNull(test);
            CollectionAssert.Contains(test, "Fun Park");
        }

        [TestMethod]
        public void GetParkInfoTest()
        {
            Park test = new Park();
            string name = "Fun Park";
            ParkSqlDAL parkSqlDAL = new ParkSqlDAL();
            test = parkSqlDAL.GetParkInfo(name);
            Assert.IsNotNull(test);
            Assert.AreEqual("Fun Park", test.Name);
        }
    }
}
