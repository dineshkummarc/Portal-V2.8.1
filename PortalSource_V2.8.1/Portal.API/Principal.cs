using System;
using System.Security.Principal;

namespace Portal.API
{
  /// <summary>
  /// Zusammenfassungsbeschreibung für Principal
  /// </summary>
  public class Principal : System.Security.Principal.IPrincipal
  {
    private IIdentity m_identity;
    private Guid m_id = Guid.Empty;
    private string m_firstName = "";
    private string m_surName = "";
    private string m_email = "";
    private string[] m_roles;

    public Principal(IIdentity identity, string[] roles)
    {
      m_identity = identity;
      m_roles = new string[roles.Length];
      Roles = roles;
    }

    public bool IsInRole(string role)
    {
      return Array.BinarySearch(m_roles, role) >= 0 ? true : false;
    }
    public IIdentity Identity
    {
      get
      {
        return m_identity;
      }
    }


    #region Properties
    public Guid Id
    {
      get
      {
        return m_id;
      }
      set
      {
        m_id = value;
      }
    }
    public string Login
    {
      get
      {
        return m_identity.Name;
      }
    }
    public string FirstName
    {
      get
      {
        return m_firstName;
      }
      set
      {
        m_firstName = value;
      }
    }
    public string SurName
    {
      get
      {
        return m_surName;
      }
      set
      {
        m_surName = value;
      }
    }
    public string FullName
    {
      get
      {
        return FirstName + " " + SurName;
      }
    }
    public string EMail
    {
      get
      {
        return m_email;
      }
      set
      {
        m_email = value;
      }
    }
    public string[] Roles
    {
      get
      {
        return m_roles;
      }
      set
      {
        value.CopyTo(m_roles, 0);
        Array.Sort(m_roles);
      }
    }
    #endregion
  }
}