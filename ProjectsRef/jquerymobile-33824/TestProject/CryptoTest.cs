using BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject
{
    
    
    /// <summary>
    ///This is a test class for CryptoTest and is intended
    ///to contain all CryptoTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CryptoTest
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
        ///A test for Decrypt
        ///</summary>
        [TestMethod()]
        public void DecryptTest()
        {
            string encryptedValue = "N0YsHD8fLD9QFkxuET8XRz8DkjrpWtm2A/zb7R4RjGMAs3MsF6XYW/z8733ddH6NNlMu6e24kam3+0MvB05IbKVLkbvwJvcWeUO3C7tpJ7wlVkxzRoer9kA8Die007P3uQymBfweAKffBfgH6S0sAw==";
            string passwordsalt = "QFVhVZdTHNY5RKKYHXEYuWMZhD5bCFRYf2GA2UHD";
            string expected = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ23456789~!@#$%^&*()_+-={}|[]:;<>?.,`";  
            string actual;
            actual = Crypto.Decrypt(encryptedValue, passwordsalt);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Encrypt
        ///</summary>
        [TestMethod()]
        public void EncryptTest()
        {
            string plainText = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ23456789~!@#$%^&*()_+-={}|[]:;<>?.,`"; 
            string passwordsalt = "QFVhVZdTHNY5RKKYHXEYuWMZhD5bCFRYf2GA2UHD"; 
            string expected = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ23456789~!@#$%^&*()_+-={}|[]:;<>?.,`"; 
            string actual;
            actual = Crypto.Decrypt(Crypto.Encrypt(plainText, passwordsalt), passwordsalt);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GeneratePasswordSalt
        ///</summary>
        [TestMethod()]
        public void GeneratePasswordSaltTest()
        {
            string actual;
            actual = Crypto.GeneratePasswordSalt();
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for GenerateRandomPassphrase
        ///</summary>
        [TestMethod()]
        public void GenerateRandomPassphraseTest()
        {
            string actual;
            actual = Crypto.GenerateRandomPassphrase();
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for GenerateSimplePassword
        ///</summary>
        [TestMethod()]
        public void GenerateSimplePasswordTest()
        {
            string actual;
            actual = Crypto.GenerateSimplePassword();
            Assert.IsNotNull(actual);
        }
    }
}
