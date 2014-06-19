using System;
using Lib.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Entities
{
    [TestClass]
    public class Submit_UnitTest
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
        public void Test_Submit_CRUD_Operations()
        {
            // Create
            Submit newSubmit = _testSupport.CreateTestSubmit();

            Assert.AreNotEqual(0, newSubmit.Id, "O Id da instância depois de salva não pode ser zero.");

            // Read
            Submit submitFromDb = _testSupport.SubmitRepository.getInstanceById(newSubmit.Id);

            Assert.AreEqual(newSubmit.Date, submitFromDb.Date, "O Date não foi salvo corretamente.");
            Assert.AreEqual(newSubmit.Observation, submitFromDb.Observation, "O Observation não foi salvo corretamente");
            Assert.AreEqual(newSubmit.ResponseFormId, submitFromDb.ResponseFormId, "ResponseFormId não foi salvo corretamente");
            Assert.AreEqual(newSubmit.Status, submitFromDb.Status, "Status não foi salvo corretamente");

            newSubmit.Status = (int)Lib.Enumerations.SubmitStatus.Approved;
            newSubmit.Observation = "Alterando o status para aprovado";

            //Update
            _testSupport.SubmitRepository.save(newSubmit);

            submitFromDb = null;
            submitFromDb = _testSupport.SubmitRepository.getInstanceById(newSubmit.Id);

            Assert.AreEqual(newSubmit.Status, submitFromDb.Status, "A alteração do Status deveria ter sido salva com sucesso.");
            Assert.AreEqual(newSubmit.Observation, submitFromDb.Observation, "A alteração do Status deveria ter sido salva com sucesso.");


            // Delete
            _testSupport.SubmitRepository.delete(submitFromDb);

            submitFromDb = null;
            submitFromDb = _testSupport.SubmitRepository.getInstanceById(newSubmit.Id);

            Assert.IsNull(submitFromDb, "Não deveria haver nenhuma submit com o Id informado.");
        }
    }
}
