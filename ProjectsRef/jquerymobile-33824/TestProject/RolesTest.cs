using BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject
{
    
    
    /// <summary>
    ///This is a test class for RolesTest and is intended
    ///to contain all RolesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RolesTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for CreateRole
        ///</summary>
        [TestMethod()]
        public void CreateRoleTest()
        {
            Roles roles = new Roles(Utils.DbContext);
            roles.CreateRole("Registered Users", "Basic Role");
            roles.CreateRole("Subscribers", "Web Subscribers ");
            roles.CreateRole("Administrators", "Super Users");
        }


        /// <summary>
        ///A test for AddUsersToRoles
        ///</summary>
        [TestMethod()]
        public void AddUsersToRolesTest()
        {
            int roleid = 3; 
            int userid = 1; 
            int portalID = 1;
            Roles roles = new Roles(Utils.DbContext);
            roles.AddUsersToRoles(roleid, userid, portalID);
        }
    }

}
