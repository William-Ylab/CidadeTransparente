using System;
using Lib.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Entities
{
    [TestClass]
    public class BaseForm_UnitTest
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
        public void Test_BaseForm_CRUD_Operations()
        {
            // Create
            BaseForm newForm = _testSupport.CreateTestBaseForm();

            Assert.AreNotEqual(0, newForm.Id, "O Id da instância depois de salva não pode ser zero.");

            // Read
            BaseForm baseFormFromDb = _testSupport.BaseFormRepository.getInstanceById(newForm.Id);

            Assert.AreEqual(newForm.Name, baseFormFromDb.Name, "O nome não foi salvo corretamente.");
            Assert.AreEqual(newForm.BaseBlocks.Count, baseFormFromDb.BaseBlocks.Count, "O questionário não salvou os blocos criados");

            newForm.Name = "Unit Test - Updated Form name";

            //Update
            _testSupport.BaseFormRepository.save(newForm);

            baseFormFromDb = null;
            baseFormFromDb = _testSupport.BaseFormRepository.getInstanceById(newForm.Id);

            Assert.AreEqual(newForm.Name, baseFormFromDb.Name, "A alteração deveria ter sido salva com sucesso.");


            //Deletando um determinado bloco
            newForm.BaseBlocks.RemoveAt(0);

            _testSupport.BaseFormRepository.save(newForm);

            baseFormFromDb = null;
            baseFormFromDb = _testSupport.BaseFormRepository.getInstanceById(newForm.Id);

            Assert.AreEqual(newForm.BaseBlocks.Count, baseFormFromDb.BaseBlocks.Count, "O bloco removido não foi deletado com sucesso.");

            //Deletando um subbloco
            newForm.BaseBlocks[0].BaseSubBlocks.RemoveAt(0);
            
            _testSupport.BaseFormRepository.save(newForm);

            baseFormFromDb = null;
            baseFormFromDb = _testSupport.BaseFormRepository.getInstanceById(newForm.Id);

            Assert.AreEqual(newForm.BaseBlocks[0].BaseSubBlocks.Count, baseFormFromDb.BaseBlocks[0].BaseSubBlocks.Count, "o subbloco removido não foi deletado com sucesso.");

            //Deletando uma pergunta
            newForm.BaseBlocks[0].BaseSubBlocks[0].BaseQuestions.RemoveAt(0);

            _testSupport.BaseFormRepository.save(newForm);

            baseFormFromDb = null;
            baseFormFromDb = _testSupport.BaseFormRepository.getInstanceById(newForm.Id);

            Assert.AreEqual(newForm.BaseBlocks[0].BaseSubBlocks[0].BaseQuestions.Count, baseFormFromDb.BaseBlocks[0].BaseSubBlocks[0].BaseQuestions.Count, "A pergunta removido não foi deletada com sucesso.");

            // Delete
            _testSupport.BaseFormRepository.delete(baseFormFromDb);

            baseFormFromDb = null;
            baseFormFromDb = _testSupport.BaseFormRepository.getInstanceById(newForm.Id);

            Assert.IsNull(baseFormFromDb, "Não deveria haver nenhuma questionário com o Id informado.");
        }
    }
}
