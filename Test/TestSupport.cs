using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Entities;
using Lib.Repositories;

namespace Test
{
    /// <summary>
    /// Classe de apoio para a realização dos testes unitários.
    /// </summary>
    public class TestSupport
    {
        private Lib.Entities.User activeUser = null;

        private List<BaseForm> _createdBaseForm = null;

        private List<Period> _createdPeriods = null;

        private List<ResponseForm> _createdResponseForm = null;

        public BaseFormRepository BaseFormRepository = null;

        public PeriodRepository PeriodRepository = null;

        public ResponseFormRepository ResponseFormRepository = null;

        public UserRepository UserRepository = null;

        public List<User> _createdUser = null;

        public SubmitRepository SubmitRepository = null;

        public List<Submit> _createdSubmit = null;

        /// <summary>
        /// Inicializa as propriedades.
        /// </summary>
        public TestSupport()
        {
            _createdBaseForm = new List<BaseForm>();
            _createdPeriods = new List<Period>();
            _createdResponseForm = new List<ResponseForm>();
            _createdUser = new List<User>();
            _createdSubmit = new List<Submit>();

            //Cria o usuário Master
            createMasterUser();

            BaseFormRepository = new BaseFormRepository(activeUser);
            PeriodRepository = new PeriodRepository(activeUser);
            ResponseFormRepository = new ResponseFormRepository(activeUser);
            UserRepository = new UserRepository(activeUser);
            SubmitRepository = new SubmitRepository(activeUser);
        }

        /// <summary>
        /// Limpa todas as operações realizadas durante os testes unitários.
        /// </summary>
        public void SupportCleanup()
        {
            deleteBaseForm();
            deletePeriods();
            deleteSubmits();
            deleteResponseForm();
            deleteUsers();
        }

        /// <summary>
        /// Cria um novo Delivery para a realização dos testes unitários.
        /// </summary>
        /// <returns>Instância de Delivery para os testes unitários.</returns>
        public BaseForm CreateTestBaseForm()
        {
            BaseForm newTestForm = new BaseForm();
            newTestForm.Name = "Form " + Guid.NewGuid().ToString().Substring(0, 4);
            newTestForm.BaseBlocks = new List<BaseBlock>();
            newTestForm.PeriodId = CreateTestPeriod(DateTime.Now.AddMonths(2), DateTime.Now.AddMonths(3)).Id;
            newTestForm.BaseBlocks.Add(CreateTestBaseBlock(1, 1));
            newTestForm.BaseBlocks.Add(CreateTestBaseBlock(1, 1));
            newTestForm.BaseBlocks.Add(CreateTestBaseBlock(1, 1));

            _createdBaseForm.Add(newTestForm);
            BaseFormRepository.save(newTestForm);

            return newTestForm;
        }

        /// <summary>
        /// Cria um novo Delivery para a realização dos testes unitários.
        /// </summary>
        /// <returns>Instância de Delivery para os testes unitários.</returns>
        public User CreateTestUser()
        {
            User newUser = new User();
            newUser.Name = "Form " + Guid.NewGuid().ToString().Substring(0, 4);
            newUser.Login = "testelogin";
            newUser.Password = "123456";
            newUser.UserType = (int)Lib.Enumerations.UserType.Admin;
            newUser.Email = "teste@teste.com.br";
            newUser.Mime = "jpeg";
            newUser.Thumb = new byte[5] { 0, 1, 0, 1, 0 };
            newUser.Active = true;

            _createdUser.Add(newUser);
            UserRepository.save(newUser);

            return newUser;
        }

        private BaseBlock CreateTestBaseBlock(int totalQuestions, int totalsubblocks)
        {
            BaseBlock baseBlock = new BaseBlock();

            baseBlock.Name = "Bloco 1" + Guid.NewGuid().ToString().Substring(0, 4);
            baseBlock.BaseSubBlocks = new List<BaseSubBlock>();

            for (int i = 0; i < totalsubblocks; i++)
            {
                baseBlock.BaseSubBlocks.Add(CreateTestBaseSubBlock(i, totalQuestions));
            }

            return baseBlock;
        }

        private BaseSubBlock CreateTestBaseSubBlock(int blockIndex, int totalQuestions)
        {
            BaseSubBlock baseBlock = new BaseSubBlock();

            baseBlock.Name = "SubBloco " + blockIndex + Guid.NewGuid().ToString().Substring(0, 4);
            baseBlock.Index = blockIndex + 1;
            baseBlock.Weight = 1;

            baseBlock.BaseQuestions = new List<BaseQuestion>();

            for (int i = 0; i < totalQuestions; i++)
            {
                baseBlock.BaseQuestions.Add(CreateBaseQuestion(i));
            }

            return baseBlock;
        }

        private BaseQuestion CreateBaseQuestion(int questionIndex)
        {
            Random r = new Random();

            BaseQuestion baseQuestion = new BaseQuestion();

            baseQuestion.Question = "Question " + questionIndex + Guid.NewGuid().ToString().Substring(0, 4);
            baseQuestion.Index = questionIndex + 1;
            baseQuestion.Value = Convert.ToDecimal(r.Next(1, 3));
            baseQuestion.Tip = "Dica...";

            return baseQuestion;
        }

        public Period CreateTestPeriod(DateTime dateMin, DateTime dateMax)
        {
            Period newPeriod = new Period();
            newPeriod.Name = "Period Test " + Guid.NewGuid().ToString().Substring(0, 4);
            newPeriod.InitialDate = dateMin;
            newPeriod.FinalDate = dateMax;
            newPeriod.Published = true;

            _createdPeriods.Add(newPeriod);
            PeriodRepository.save(newPeriod);

            return newPeriod;
        }

        public ResponseForm CreateTestResponseForm()
        {
            BaseForm bf = CreateTestBaseForm();
            List<decimal?> scores = new List<decimal?>();
            scores.Add(0);
            scores.Add(0.25m);
            scores.Add(0.5m);
            scores.Add(0.75m);
            scores.Add(1m);
            scores.Add(null);

            Random r = new Random();

            ResponseForm rf = new ResponseForm();
            rf.Answers = new List<Answer>();
            rf.BaseFormId = bf.Id;
            rf.CityId = 1;
            rf.UserId = 1;
            bf.BaseBlocks.ForEach(bb =>
                {
                    bb.BaseSubBlocks.ForEach(bsb =>
                        {
                            bsb.BaseQuestions.ForEach(bq =>
                                {
                                    Answer a = new Answer();
                                    a.BaseQuestionId = bq.Id;
                                    a.Observation = Guid.NewGuid().ToString();
                                    a.Score = scores[r.Next(0, 5)];

                                    rf.Answers.Add(a);
                                });
                        });
                });

            _createdResponseForm.Add(rf);
            ResponseFormRepository.save(rf);

            return rf;
        }

        public Submit CreateTestSubmit()
        {
            Submit newSubmit = new Submit();
            newSubmit.Date = DateTime.Now;
            newSubmit.Observation = "Nenhuma observação";
            newSubmit.ResponseFormId = CreateTestResponseForm().Id;
            newSubmit.Status = (int)Lib.Enumerations.SubmitStatus.NotApproved;

            _createdSubmit.Add(newSubmit);
            SubmitRepository.save(newSubmit);

            return newSubmit;
        }

        private void deleteBaseForm()
        {
            if (_createdBaseForm == null)
                return;

            foreach (var entity in _createdBaseForm)
            {
                if (BaseFormRepository.getInstanceById(entity.Id) != null)
                {
                    BaseFormRepository.delete(entity);
                }
            }
        }

        private void deletePeriods()
        {
            if (_createdPeriods == null)
                return;

            foreach (var entity in _createdPeriods)
            {
                if (PeriodRepository.getInstanceById(entity.Id) != null)
                {
                    PeriodRepository.delete(entity);
                }
            }
        }

        private void deleteResponseForm()
        {
            if (_createdResponseForm == null)
                return;

            foreach (var entity in _createdResponseForm)
            {
                if (ResponseFormRepository.getInstanceById(entity.Id) != null)
                {
                    ResponseFormRepository.delete(entity);
                }
            }
        }

        private void deleteUsers()
        {
            if (_createdUser == null)
                return;

            foreach (var entity in _createdUser)
            {
                if (UserRepository.getInstanceById(entity.Id) != null)
                {
                    UserRepository.delete(entity);
                }
            }
        }

        private void deleteSubmits()
        {
            if (_createdSubmit == null)
                return;

            foreach (var entity in _createdSubmit)
            {
                if (SubmitRepository.getInstanceById(entity.Id) != null)
                {
                    SubmitRepository.delete(entity);
                }
            }
        }

        private void createMasterUser()
        {
            using (Lib.Repositories.UserRepository rep = new UserRepository(null))
            {
                Lib.Entities.User masterUser = new User();
                masterUser.Login = "testeMaster";
                masterUser.Name = "testeMaster";
                masterUser.Password = "123456";
                masterUser.UserType = (int)Lib.Enumerations.UserType.Master;
                masterUser.Email = "master@teste.com.br";

                rep.createUserMaster(masterUser);

                _createdUser.Add(masterUser);
                activeUser = masterUser;
            }
        }

    }
}
