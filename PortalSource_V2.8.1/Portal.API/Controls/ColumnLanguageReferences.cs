using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.API.Controls
{
    public class ColumnLanguageReferences
    {
        private string header;
        private string footer;

        public string HeaderText
        {
            get { return header; }
            set { header = value; }
        }

        public string FooterText
        {
            get { return footer; }
            set { footer = value; }
        }
    }
}
