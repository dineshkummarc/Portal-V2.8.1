using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Net;

namespace Portal.Modules.FormBuilder
{
  /// <summary>
  /// Verarbeitet ein Formular indem die daten in einer Email versendet werden.
  /// </summary>
  public class EmailSender
  {
    public EmailSender()
    {

    }

    /// <summary>
    /// Verarbeitet die Daten.
    /// </summary>
    /// <param name="config">Die Konfiguration zum senden des Emails.</param>
   
    public void Process(ModuleConfig config)
    {
      if (config == null)
        throw new ArgumentNullException("config");

      // Validierung der Konfiguration.
      bool readyToSend = true;

      // Es wird nur eine Email versendet, wenn auch mindestens ein Empfänger angegeben ist.
      readyToSend = readyToSend && (config.EmailConfig.ToRecipient.Count > 0) 
                                || (config.EmailConfig.CcRecipient.Count > 0)
                                || (config.EmailConfig.BccRecipient.Count > 0);

      if (readyToSend)
      {
        // Hilfsobjekt zum ersetzen der Platzhalter erzeugen.
        PlaceholderReplacer.ReplacePlaceholder replaceDelegate =
                                                          new PlaceholderReplacer.ReplacePlaceholder(ReplacePlaceholder);
        PlaceholderReplacer replacer = new PlaceholderReplacer(replaceDelegate);


        try
        {
          // Email konfigurieren.
          MailMessage message = new MailMessage();
          message.From = new MailAddress(replacer.Replace(config.EmailConfig.Sender));

          foreach (string address in config.EmailConfig.ToRecipient)
            message.To.Add(replacer.Replace(address));

          foreach (string address in config.EmailConfig.CcRecipient)
            message.CC.Add(replacer.Replace(address));

          foreach (string address in config.EmailConfig.BccRecipient)
            message.Bcc.Add(replacer.Replace(address));

          message.Subject = replacer.Replace(config.EmailConfig.Subject);

          message.IsBodyHtml = config.EmailConfig.BodyIsHtml;
          message.Body = replacer.Replace(config.EmailConfig.Body);

          // Smtp Client konfigurieren.
          SmtpClient smtpClient = new SmtpClient();
          smtpClient.Host = config.EmailConfig.Server;
          smtpClient.Port = 25;
          if (config.EmailConfig.ServerNeedAuth)
          {
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Credentials = new NetworkCredential(config.EmailConfig.LoginUser, 
                                                           config.EmailConfig.LoginPassword);
          }

          // Email versenden.
          smtpClient.Send(message);
        }
        catch (Exception ex)
        {
          // TODO: Detailliertere Fehlerbehandlung.
          throw;
        }
      }     
    }

    /// <summary>
    /// Ersetzt den Platzhalter aufgrund der Formulardaten in der Form Collection des Requests.
    /// </summary>
    /// <param name="placeholder"></param>
    /// <returns></returns>
    private string ReplacePlaceholder(string placeholder)
    {
      string result = HttpContext.Current.Request.Form[placeholder];
      if (result == null) // nicht gefunden.
        result = "";
      
      return result;
    }
  }
}