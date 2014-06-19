using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Commons
{
    public class EmailUtils
    {
        public string PrefixoTitulo { get; set; }
        public string Assinatura { get; set; }
        public MailAddress Remetente { get; set; }

        /// <summary>
        /// Instancia da classe de envio de e-mails
        /// </summary>
        /// <param name="prefixoTitulo">Prefixo será adicionado ao titulo do e-mail</param>
        /// <param name="assinatura">Assinatura ou informações importantes no rodapé de e-mail</param>
        /// <param name="emailRemetente">Email do remetente (Necessário e-mail válido)</param>
        public EmailUtils(string prefixoTitulo, string assinatura, string emailRemetente)
        {
            if (String.IsNullOrWhiteSpace(emailRemetente))
            {
                throw new ArgumentNullException("emailRemetente");
            }
            else
            {
                if (!ValidateUtils.validateEmail(emailRemetente))
                    throw new FormatException("Formato inválido de e-mail");
            }

            PrefixoTitulo = prefixoTitulo;
            Assinatura = assinatura;
            Remetente = new MailAddress(emailRemetente);
        }

        /// <summary>
        /// Envia e-mail para um unico destinatário.
        /// </summary>
        /// <param name="corpo">Corpo do e-mail</param>
        /// <param name="assunto">Assunto do e-mail</param>
        /// <param name="email">E-mail do destinatário</param>
        public void enviaEmail(string corpo, string assunto, MailAddress email)
        {
            enviaEmail(corpo, assunto, email, null);
        }

        /// <summary>
        /// Envia e-mail para um unico destinatário com anexo
        /// </summary>
        /// <param name="corpo">Corpo do e-mail</param>
        /// <param name="assunto">Assunto do e-mail</param>
        /// <param name="email">E-mail do destinatário</param>
        /// <param name="anexo">Anexo</param>
        public void enviaEmail(string corpo, string assunto, MailAddress email, Attachment anexo)
        {
            try
            {
                //Adiciona a assinatura ao e-mail
                StringBuilder _sb = new StringBuilder();
                _sb.Append(corpo);

                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(email);
                mailMessage.Body = _sb.ToString();
                mailMessage.Subject = String.Format("{1}{0}", PrefixoTitulo, assunto);
                mailMessage.IsBodyHtml = true;
                mailMessage.From = Remetente;

                if (anexo != null)
                {
                    mailMessage.Attachments.Add(anexo);
                }

                SmtpClient client = new SmtpClient();
                client.EnableSsl = true;
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
