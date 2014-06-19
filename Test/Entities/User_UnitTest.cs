using System;
using Lib.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Entities
{
    [TestClass]
    public class User_UnitTest
    {
        /// <summary>
        /// Acesso a classe de suporte aos Testes Unitários.
        /// </summary>
        private static TestSupport _testSupport = null;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _testSupport = new TestSupport();
        }

        [ClassCleanup]
        public static void TestCleanup()
        {
            _testSupport.SupportCleanup();
            _testSupport = null;
        }

        [TestMethod]
        public void Test_User_CRUD_Operations()
        {
            // Create
            User newUser = _testSupport.CreateTestUser();
            
            Assert.AreNotEqual(0, newUser.Id, "O Id da instância depois de salva não pode ser zero.");

            // Read
            User userFromDb = _testSupport.UserRepository.getInstanceById(newUser.Id);

            Assert.AreEqual(newUser.Name, userFromDb.Name, "O nome não foi salvo corretamente.");
            Assert.AreEqual(newUser.Email, userFromDb.Email, "O email não foi salvo corretamente");
            Assert.AreEqual(newUser.Login, userFromDb.Login, "Login não foi salvo corretamente");
            Assert.AreEqual(newUser.Password, userFromDb.Password, "Password não foi salvo corretamente");
            Assert.AreEqual(newUser.UserType, userFromDb.UserType, "UserType não foi salvo corretamente");
            Assert.AreEqual(newUser.Mime, userFromDb.Mime, "Mime não foi salvo corretamente");
            Assert.AreEqual(newUser.Thumb, userFromDb.Thumb, "Thumb não foi salvo corretamente");
            Assert.AreEqual(newUser.Active, userFromDb.Active, "Active não foi salvo corretamente");

            newUser.Name = "Unit Test - Updated user name";
            newUser.Thumb = null;
            newUser.Mime = null;
            newUser.Active = false;

            //Update
            _testSupport.UserRepository.save(newUser);

            userFromDb = null;
            userFromDb = _testSupport.UserRepository.getInstanceById(newUser.Id);

            Assert.AreEqual(newUser.Name, userFromDb.Name, "A alteração deveria ter sido salva com sucesso.");
            Assert.AreEqual(newUser.Mime, userFromDb.Mime, "A alteração deveria ter sido salva com sucesso.");
            Assert.AreEqual(newUser.Thumb, userFromDb.Thumb, "A alteração deveria ter sido salva com sucesso.");
            Assert.AreEqual(newUser.Active, userFromDb.Active, "A alteração deveria ter sido salva com sucesso.");

            // Delete
            _testSupport.UserRepository.delete(userFromDb);

            userFromDb = null;
            userFromDb = _testSupport.UserRepository.getInstanceById(newUser.Id);

            Assert.IsNull(userFromDb, "Não deveria haver nenhuma questionário com o Id informado.");
        }
    }
}
