using System;
using Lib.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Entities
{
    [TestClass]
    public class ResponseForm_UnitTest
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
        public void Test_ResponseForm_CRUD_Operations()
        {
            ResponseForm rf = _testSupport.CreateTestResponseForm();

            Assert.IsTrue(rf.Id > 0, "O Id deveria ser maior que 0!");
            
            ResponseForm rfFromBd = _testSupport.ResponseFormRepository.getInstanceById(rf.Id);

            var x = rfFromBd.calculateTotalScore();

            Review r = new Review();
            r.Accepted = true;
            r.Date = DateTime.Now;
            r.Observations = "noa";
            r.ResponseFormId = rf.Id;
            r.UserId = 1;

            _testSupport.ResponseFormRepository.addReview(r);

            rfFromBd = _testSupport.ResponseFormRepository.getInstanceById(rf.Id);

            Assert.IsTrue(rfFromBd.Reviews.Count > 0, "Deve possuir mais de um review!");

            //atualiza uma determinada resposta
            rfFromBd.Answers[0].Observation = "Observação de atualização";
            rfFromBd.Answers[0].Score = 1;

            _testSupport.ResponseFormRepository.save(rfFromBd);

            rfFromBd = _testSupport.ResponseFormRepository.getInstanceById(rf.Id);

            Assert.IsTrue(rfFromBd.Answers[0].Observation == "Observação de atualização", "Observação foi atualizada com sucesso!");
            Assert.IsTrue(rfFromBd.Answers[0].Score == 1, "Score não foi atualizado com sucesso!");
        }

        [TestMethod]
        public void Test_PercentageSubBlock()
        {
            ResponseForm rf = _testSupport.CreateTestResponseForm();

            Assert.IsTrue(rf.Id > 0, "O Id deveria ser maior que 0!");

            ResponseForm rfFromBd = _testSupport.ResponseFormRepository.getInstanceById(rf.Id);

            decimal? percent = 0;

            rfFromBd.BaseForm.BaseBlocks.ForEach(bb =>
            {
                bb.BaseSubBlocks.ForEach(bsb =>
                {
                    percent = bsb.calculatePercent(rfFromBd.Id);
                });
            });
        }
    }
}
