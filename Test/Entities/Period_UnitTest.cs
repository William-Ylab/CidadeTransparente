using System;
using Lib.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Entities
{
    [TestClass]
    public class Period_UnitTest
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
        public void Test_Period_CRUD_Operations()
        {
            // Create
            Period newPeriod = _testSupport.CreateTestPeriod(DateTime.Now.AddYears(3).AddDays(2), DateTime.Now.AddYears(3).AddDays(9));

            Assert.AreNotEqual(0, newPeriod.Id, "O Id da instância depois de salva não pode ser zero.");


            // Read
            Period periodFromDb = _testSupport.PeriodRepository.getInstanceById(newPeriod.Id);

            Assert.AreEqual(newPeriod.Name, periodFromDb.Name, "O nome não foi salvo corretamente.");
            Assert.AreEqual(newPeriod.InitialDate.ToString("yyyy/MM/dd HH:mm:ss"), periodFromDb.InitialDate.ToString("yyyy/MM/dd HH:mm:ss"), "A data inicial do periodo não foi salvo corretamente.");
            Assert.AreEqual(newPeriod.FinalDate.ToString("yyyy/MM/dd HH:mm:ss"), periodFromDb.FinalDate.ToString("yyyy/MM/dd HH:mm:ss"), "A data final do periodo não foi salvo corretamente.");
            Assert.AreEqual(newPeriod.Open, false, "O periodo criado deveria estar fechado.");
            Assert.IsTrue(periodFromDb.Published == true, "Deveria estar publicado.");

            //Criando periodos de testes
            var otherPeriod = _testSupport.CreateTestPeriod(DateTime.Now.AddYears(3).AddDays(2), DateTime.Now.AddYears(3).AddDays(9));
            Assert.IsTrue(_testSupport.PeriodRepository.HasErrors, "Precisava gerar um erro de conflito de datas");

            _testSupport.PeriodRepository.Errors.Clear();
            otherPeriod = _testSupport.CreateTestPeriod(DateTime.Now.AddYears(3).AddDays(1), DateTime.Now.AddYears(3).AddDays(9));
            Assert.IsTrue(_testSupport.PeriodRepository.HasErrors, "Precisava gerar um erro de conflito de datas");

            _testSupport.PeriodRepository.Errors.Clear();
            otherPeriod = _testSupport.CreateTestPeriod(DateTime.Now.AddYears(3).AddDays(3), DateTime.Now.AddYears(3).AddDays(5));
            Assert.IsTrue(_testSupport.PeriodRepository.HasErrors, "Precisava gerar um erro de conflito de datas");

            _testSupport.PeriodRepository.Errors.Clear();
            otherPeriod = _testSupport.CreateTestPeriod(DateTime.Now.AddYears(3).AddDays(1), DateTime.Now.AddYears(3).AddDays(11));
            Assert.IsTrue(_testSupport.PeriodRepository.HasErrors, "Precisava gerar um erro de conflito de datas");

            _testSupport.PeriodRepository.Errors.Clear();
            otherPeriod = _testSupport.CreateTestPeriod(DateTime.Now.AddYears(3).AddDays(2), DateTime.Now.AddYears(3).AddDays(11));
            Assert.IsTrue(_testSupport.PeriodRepository.HasErrors, "Precisava gerar um erro de conflito de datas");

            _testSupport.PeriodRepository.Errors.Clear();
            otherPeriod = _testSupport.CreateTestPeriod(DateTime.Now.AddYears(3).AddDays(9), DateTime.Now.AddYears(2).AddDays(11));
            Assert.IsTrue(_testSupport.PeriodRepository.HasErrors, "Precisava gerar um erro de conflito de datas");

            _testSupport.PeriodRepository.Errors.Clear();
            otherPeriod = _testSupport.CreateTestPeriod(DateTime.Now.AddYears(3).AddDays(12), DateTime.Now.AddYears(3).AddDays(14));
            Assert.IsTrue(!_testSupport.PeriodRepository.HasErrors, "Não deveria gerar erro");


            newPeriod.Name = "Period Test - Updated name";
            newPeriod.Open = true;
            newPeriod.Published = false;

            //Adiando a data final do periodo
            newPeriod.FinalDate = newPeriod.FinalDate.AddDays(1);

            //Update
            _testSupport.PeriodRepository.save(newPeriod);

            periodFromDb = null;
            periodFromDb = _testSupport.PeriodRepository.getInstanceById(newPeriod.Id);

            Assert.AreEqual(newPeriod.Name, periodFromDb.Name, "A alteração deveria ter sido salva com sucesso.");
            Assert.AreEqual(newPeriod.FinalDate, periodFromDb.FinalDate, "A data final não foi adiada com sucesso.");
            Assert.AreEqual(newPeriod.Open, periodFromDb.Open, "Periodo deveria estar aberto");
            Assert.AreEqual(newPeriod.Published, periodFromDb.Published, "Periodo não deveria estar publicado");

            //Abre um outro periodo
            otherPeriod.Open = true;
            _testSupport.PeriodRepository.Errors.Clear();
            _testSupport.PeriodRepository.save(otherPeriod);
            Assert.IsTrue(_testSupport.PeriodRepository.HasErrors, "Deveria lançar error de existe periodo em aberto");


            // Delete
            _testSupport.PeriodRepository.delete(newPeriod);

            periodFromDb = null;
            periodFromDb = _testSupport.PeriodRepository.getInstanceById(newPeriod.Id);

            Assert.IsNull(periodFromDb, "Não deveria haver nenhuma periodo com o Id informado.");
        }
    }
}
