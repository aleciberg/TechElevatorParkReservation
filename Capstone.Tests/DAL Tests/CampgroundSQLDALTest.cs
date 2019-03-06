using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone.Tests
{
    [TestClass]
    public class CampgroundSQLDALTest
    {

        [TestMethod]
        public void GetCampgroundsTest()
        {
            CampgroundSqlDAL campgroundSqlDAL = new CampgroundSqlDAL();
            string name = "Acadia";
            List<Campground> test = campgroundSqlDAL.GetCampgrounds(name);
            Assert.IsNotNull(test);
        }
    }
}
