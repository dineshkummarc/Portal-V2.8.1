using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Caching;

using Portal.API.Statistics.Data;
using Portal.API.Statistics.Service;

namespace Portal.API.Statistics.Worker
{
  /// <summary>
  /// This class increases the loggin count of the current user and saves his latest login date.
  /// </summary>
  internal class LoginWorker : WorkerBase
  {
    // Reference to the calling service.
    LoginStatisticService service;

    // ID of the user that logged in.
    private Guid userId;

    // Time-Stamp of the current login.
    private DateTime lastLogin;

    // Access to the data must be synchronized.
    static object lockObject = new object();

    /// <summary>
    /// Constructor of the LoginWorker class.
    /// </summary>
    /// <param name="context">Users HttpContext.</param>
    /// <param name="userId">ID of the user.</param>
    public LoginWorker(LoginStatisticService service, HttpContext context, Guid userId)
      : base(context)
    {
      this.service = service;
      this.userId = userId;
      this.lastLogin = DateTime.Now;
    }

    /// <summary>
    /// Increases the users loggin count and actualizes his latest login date.
    /// </summary>
    public void AddLogin()
    {
      lock (lockObject)
      {
        try
        {
          LoginStatisticData data = service.GetData(context);

          // Find the user.
          if (null == System.Web.HttpContext.Current)
            System.Web.HttpContext.Current = context;
          Portal.API.Users.UserRow user = UserManagement.Users.FindById(userId);

          // Search the row for the current user.
          // If there is now row for the current user, create a new one.
          LoginStatisticData.TLoginRow[] rows = (LoginStatisticData.TLoginRow[])data.TLogin.Select("UserId = '" + userId.ToString() + "'");
          LoginStatisticData.TLoginRow row;
          if (rows.Length == 0)
          {
            row = data.TLogin.NewTLoginRow();
            row.UserId = userId;
            row.Login = user.login;
            row.FirstName = user.SafeFirstName;
            row.LastName = user.SafeSurName;
            row.LoginCount = 0;
            row.LoginCount = row.LoginCount + 1;
            row.LastLogin = this.lastLogin;
            data.TLogin.AddTLoginRow(row);
          }
          else
          {
            row = rows[0];
            row.Login = user.login;
            row.FirstName = user.SafeFirstName;
            row.LastName = user.SafeSurName;
            row.LoginCount = row.LoginCount + 1;
            row.LastLogin = this.lastLogin;
          }

          service.SaveData(context, data);
        }
        catch (Exception ex)
        {
          HandleException("Unexpected error while adding a login to the login statistics.", ex);        	
        }
      }
    }

    protected override void HandleException(string helpMessage, Exception ex)
    {
      base.HandleException(helpMessage, ex);

      if (null == service.LastException)
        service.LastException = LastException;
    }
  }
}
