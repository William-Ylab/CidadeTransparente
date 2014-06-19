using Lib.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Entities
{
    [TestClass]
    public class Entity_UnitTest
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

        //[TestMethod]
        //public void Test_Entity_CRUD_Operations()
        //{
        //    // Create
        //    Entity newEntity = _testSupport.CreateTestEntity();

        //    Assert.AreNotEqual(0, newEntity.Id, "O Id da instância depois de salva não pode ser zero.");

        //    // Read
        //    Entity entityFromDb = _testSupport.EntityRepository.getInstanceById(newEntity.Id);

        //    Assert.AreEqual(newEntity.Id, entityFromDb.Id, "O valor do Id não foi atualizado corretamente.");
        //    Assert.AreEqual(newEntity.Name, entityFromDb.Name, "O nome não foi salvo corretamente.");

        //    newEntity.Name = "Unit Test - New Entity Name";

        //    //Update
        //    _testSupport.EntityRepository.save(newEntity);

        //    entityFromDb = null;
        //    entityFromDb = _testSupport.EntityRepository.getInstanceById(newEntity.Id);

        //    Assert.AreEqual(newEntity.Name, entityFromDb.Name, "A alteração deveria ter sido salva com sucesso.");

        //    // Delete
        //    _testSupport.EntityRepository.delete(newEntity);

        //    entityFromDb = null;
        //    entityFromDb = _testSupport.EntityRepository.getInstanceById(newEntity.Id);

        //    Assert.IsNull(entityFromDb, "Não deveria haver nenhuma entidade com o Id informado.");
        //}
    }
}
