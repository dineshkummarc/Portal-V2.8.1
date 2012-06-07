using System;
using System.Data;
using System.Web;

namespace Portal.API {


    partial class Users
    {
        public UserRow FindById(Guid id)
        {
            foreach (DataRow row in tableUser.Rows)
            {
                object obj = row["id"];
                if (null != obj)
                {
                    if (obj.GetType().Equals(typeof(Guid)))
                    {
                        if (((Guid)obj).CompareTo(id) == 0)
                        {
                            return (UserRow)row;
                        }
                    }
                }
            }
            return null;
        }

        partial class UserRow
        {
            public string SafeFirstName
            {
                get { return HttpUtility.HtmlDecode(firstName); }
                set { firstName = HttpUtility.HtmlEncode(value); }
            }

            public string SafeSurName
            {
                get { return HttpUtility.HtmlDecode(surName); }
                set { surName = HttpUtility.HtmlEncode(value); }
            }
        }
    }
}
