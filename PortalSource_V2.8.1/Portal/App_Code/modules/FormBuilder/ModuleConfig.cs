using System;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using System.Collections.Generic;
using Portal.API;
using System.Runtime.Serialization;

namespace Portal.Modules.FormBuilder
{
  [XmlRoot("FormBuilder")]
  public class ModuleConfig
  {
    /// <summary>
    /// Dieses Objekt wird verwendet um sicherzustellen dass zwischen dem Laden und dem Speichern der Konfiguration 
    /// keine gleichzeitige Änderung erfolgt. Beim Speichern soll mit lock dieses Objekt geschützt werden, 
    /// anschliessend soll die Konfiguration eingelesen werden, bevor sie zusammen mit den Änderungen abgespeichert 
    /// wird. (Der Schutzt gilt für alle Instanzen gleichzeitig).
    /// </summary>
    static public object ConfigLock = new object();

    /// <summary>
    /// Der Formular Inhalt (HTML).
    /// </summary>
    [XmlElement("FormContent")]
    public string FormContent = "<input type=\"submit\" name=\"SubmitBtn\" value=\"OK\" />";


    /// <summary>
    /// Soll die Erfolgsseite angzeigt werden?
    /// </summary>
    [XmlElement("ShowSuccessContent")]
    public bool ShowSuccessPage = true;
    
    /// <summary>
    /// Die Erfolgsseite.
    /// </summary>
    [XmlElement("SuccessContent")]
    public string SuccessContent = "Thank you<br/><input type=\"submit\" name=\"SubmitBtn\" value=\"OK\" />";

    /// <summary>
    /// Der Platzhalter für die Fehlermeldung innerhalb der Fehlerseite.
    /// </summary>
    [XmlIgnore]
    public const string Messageplaceholder = "{ErrorMessage}";

    /// <summary>
    /// Die Standardfehlerseite.
    /// </summary>
    [XmlIgnore]
    public const string ErrorContentDefault = Messageplaceholder + "<br/><input type=\"button\" onclick=\"history.back(-1)\" name=\"Back\" value=\"Back\" />";
    
    /// <summary>
    /// Die Fehlerseite.
    /// </summary>
    [XmlElement("ErrorContent")]
    public string ErrorContent = ErrorContentDefault;

    [XmlElement("CommonErrorMsg")]
    public string CommonErrorMsg = "An error occured while processing the data. Please try it again later.";

    /// <summary>
    /// Soll eine Email versendet werden?
    /// </summary>
    [XmlElement("SendEmail")]
    public bool SendEmail = true;

    /// <summary>
    /// Die Email Konfiguration.
    /// </summary>
    [XmlElement("EmailConfig")]
    public EmailConfiguration EmailConfig = new EmailConfiguration();

    [Serializable]
    public class EmailConfiguration
    {
      [XmlElement("Server")]
      public string Server;

      [XmlElement("ServerNeedAuth")]
      public bool ServerNeedAuth = true;

      [XmlElement("LoginUser")]
      public string LoginUser;

      /// <summary>
      /// Verschlüsseltes Passwort.
      /// </summary>
      [XmlElement("LoginPassword")]
      public string loginPasswordEncrypted;

      /// <summary>
      /// Entschlüsseltes Passwort.
      /// </summary>
      [XmlIgnore]
      public string LoginPassword
      {
        get 
        {
          if (string.IsNullOrEmpty(loginPasswordEncrypted))
            return "";
          else
            return Crypto.Decrypt(loginPasswordEncrypted); 
        }
        set 
        {
          if (string.IsNullOrEmpty(value))
            loginPasswordEncrypted = "";
          else
            loginPasswordEncrypted = Crypto.Encrypt(value); 
        }
      }

      [XmlElement("Sender")]
      public string Sender;

      [XmlElement("ToRecipient")]
      public List<string> ToRecipient = new List<string>();

      [XmlElement("CcRecipient")]
      public List<string> CcRecipient = new List<string>();

      [XmlElement("BccRecipient")]
      public List<string> BccRecipient = new List<string>();

      [XmlElement("Subject")]
      public string Subject = "";

      [XmlElement("Body")]
      public string Body = "";

      [XmlElement("BodyIsHtml")]
      public bool BodyIsHtml = false;
    }


    [XmlElement("FormValidation")]
    public List<ValidationData> FormValidation = new List<ValidationData>();

    /// <summary>
    /// Konfiguration zur Validierung der Daten.
    /// </summary>
    [Serializable]
    public class ValidationData
    {
      public enum ValidationType
      {
        Text,
        Email,
        Number,
        RegularExpression,
      }

      /// <summary>
      /// Die Id des Validators.
      /// </summary>
      [XmlElement("Id")]
      public Guid Id
      {
        get { return _id; }
        set { _id = value; }
      }
      private Guid _id = Guid.NewGuid();
	

      /// <summary>
      /// Der Typ des zu überprüfenden Felds.
      /// </summary>
      [XmlElement("FieldType")]
      public ValidationType FieldType
      {
        get { return _fieldType; }
        set { _fieldType = value; }
      }
      private ValidationType _fieldType = ValidationType.Text;


      /// <summary>
      /// Der Name des Felds das überprüft werden soll.
      /// </summary>
      [XmlElement("FieldName")]
      public string FieldName
      {
        get { return _fieldName; }
        set { _fieldName = value; }
      }
      private string _fieldName;


      /// <summary>
      /// Die Fehlermeldung, wenn die Regel nicht eingehalten wurde.
      /// </summary>
      [XmlElement("ErrorMessage")]
      public string ErrorMessage
      {
        get { return _errorMessage; }
        set { _errorMessage = value; }
      }
      private string _errorMessage;
 

      /// <summary>
      /// Muss das Feld zwingend Daten enthalten?
      /// </summary>
      [XmlElement("Mandatory")]
      public bool IsMandatory
      {
        get { return _isMandatory; }
        set { _isMandatory = value; }
      }
      private bool _isMandatory = false;

      /// <summary>
      /// Die detailierten Eingenschaften.
      /// </summary>
      [XmlElement("SpecificProperties")]
      public Portal.API.SerializableDictionary<string, object> Properties = new SerializableDictionary<string, object>();
    }
  }
}
