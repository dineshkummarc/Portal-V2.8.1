namespace Portal.Modules.ServerInformation
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Diagnostics;
	using System.Management;
	using System.Globalization;

	/// <summary>
	///		Zusammenfassung für ServerInformation.
	/// </summary>
	public partial class ServerInformation : Portal.API.Module
	{
		protected System.Management.ManagementScope m_ManagementScope;
		protected const string m_szTitleTemplate = "<tr><td colspan=\"4\" style=\"border-top: solid 1px #000000; border-bottom: solid 1px #000000\"><b>Title</b></td></tr>";
		protected const string m_szDataTemplate  = "<tr><td width=\"40px\"><p>&nbsp;</p></td><td noWrap>Name</td><td width=\"20px\"><p>&nbsp;</p></td><td width=\"100%\">Value</td></tr>";
		protected const string m_szHrTemplate = "<tr><td colspan=\"4\"><hr></td></tr>";
		protected const string m_szEmptyLineTemplate = "<tr><td colspan=\"4\"><p>&nbsp;</p></td></tr>";

		protected void Page_Load(object sender, System.EventArgs e)
		{
      int nofItems = 0;
      try
      {
        if (m_ManagementScope == null)
          m_ManagementScope = new System.Management.ManagementScope("\\\\localhost\\root\\cimv2");

        lbSystemInfo.Text = "<table border=\"0\"";
        try
        {
          lbSystemInfo.Text += GetServerInfo() + m_szEmptyLineTemplate;
          ++nofItems;
        }
        catch(Exception) {}

        try
        {
          lbSystemInfo.Text += GetOsInfo() + m_szEmptyLineTemplate;
          ++nofItems;
        }
        catch (Exception) { }

        try
        {
          lbSystemInfo.Text += GetSystemInfo() + m_szEmptyLineTemplate;
          ++nofItems;
        }
        catch (Exception) { }
        
        try
        {
          lbSystemInfo.Text += GetCpuInfo() + m_szEmptyLineTemplate;
          ++nofItems;
        }
        catch (Exception) { }
        lbSystemInfo.Text += "</table>";
      }
      catch (Exception) { }

      if(nofItems == 0)
      {
        lbSystemInfo.Text = Portal.API.Language.GetText(this, "NotSupported");
      }

		}

		private string GetSystemInfo()
		{
			string szInfo = GetTitle(GetText("ComputerSystem"));
      
			//get processor info
			ObjectQuery oq = new System.Management.ObjectQuery("SELECT * FROM Win32_ComputerSystem");
			ManagementObjectSearcher query = new ManagementObjectSearcher(m_ManagementScope,oq);
			ManagementObjectCollection queryCollection = query.Get();

			foreach (ManagementObject mo in queryCollection)
			{
				szInfo += GetRow(GetText("Manufacturer"), (string)mo["Manufacturer"]);
				szInfo += GetRow(GetText("Model"), (string)mo["model"]);
				szInfo += GetRow(GetText("SystemType"), (string)mo["SystemType"]);
				szInfo += GetRow(GetText("TotalPhysicalMemory"), formatSize(Int64.Parse(mo["totalphysicalmemory"].ToString()), false));
				szInfo += GetRow(GetText("Domain"), (string)mo["Domain"]);
				szInfo += GetRow(GetText("Username"), (string)mo["UserName"]);
				szInfo += m_szHrTemplate;
			}

			szInfo = szInfo.Substring(0, szInfo.Length - m_szHrTemplate.Length);
      
			return szInfo;
		}

		private string GetCpuInfo()
		{
			string szInfo = GetTitle(GetText("Processor"));
      
			//get processor info
			ObjectQuery oq = new System.Management.ObjectQuery("SELECT * FROM Win32_processor");
			ManagementObjectSearcher query = new ManagementObjectSearcher(m_ManagementScope,oq);
			ManagementObjectCollection queryCollection = query.Get();

			foreach (ManagementObject mo in queryCollection)
			{
				szInfo += GetRow(GetText("Manufacturer"), (string)mo["Manufacturer"]);
				szInfo += GetRow(GetText("Type"), (string)mo["Caption"]);
				szInfo += GetRow(GetText("ClockSpeed"), formatSpeed(Int64.Parse(mo["MaxClockSpeed"].ToString())));
				szInfo += GetRow(GetText("L2CacheSize"), formatSize(Int64.Parse(mo["L2CacheSize"].ToString()), false));
				szInfo += m_szHrTemplate;
			}

			szInfo = szInfo.Substring(0, szInfo.Length - m_szHrTemplate.Length);
      
			return szInfo;
		}

		private string GetOsInfo()
		{
			string szInfo = GetTitle(GetText("OS"));

			//Query remote computer across the connection
			ObjectQuery oq = new System.Management.ObjectQuery("SELECT * FROM Win32_OperatingSystem");
			ManagementObjectSearcher query = new ManagementObjectSearcher(m_ManagementScope,oq);
			ManagementObjectCollection queryCollection = query.Get();

			foreach (ManagementObject mo in queryCollection)
			{
				szInfo += GetRow(GetText("OS"), (string)mo["Caption"]);
				szInfo += GetRow(GetText("Version"), (string)mo["Version"]);
				szInfo += GetRow(GetText("Manufacturer"), (string)mo["Manufacturer"]);
				szInfo += GetRow(GetText("WindowsDirectory"), (string)mo["WindowsDirectory"]);
				szInfo += GetRow(GetText("SerialNumber"), (string)mo["SerialNumber"]);
				szInfo += GetRow(GetText("CLRVersion"), Environment.Version.ToString());
			}

			return szInfo;
		}

		private string GetServerInfo()
		{
			string szInfo = GetTitle(GetText("Common"));
			szInfo += GetRow(GetText("ComputerName"), Server.MachineName);
			szInfo += GetRow(GetText("UpTime"), GetUpTime());
			return szInfo;
		}

		private string GetTitle(string szTitle)
		{
			string szTemp = m_szTitleTemplate;
			szTemp = szTemp.Replace("Title", szTitle);
			return m_szEmptyLineTemplate + szTemp;
		}

		private string GetRow(string szName, string szValue)
		{
			string szTemp = m_szDataTemplate;
			szTemp = szTemp.Replace("Name", szName);
			return szTemp.Replace("Value", szValue);
		}

		private string GetUpTime()
		{
			PerformanceCounter pc = new PerformanceCounter("System", "System Up Time");

			//Normally starts with zero. do Next Value always.
			pc.NextValue(); 
			TimeSpan ts = TimeSpan.FromSeconds(pc.NextValue());
    
			return ts.Days.ToString() + " " + GetText("Days") + ", " + 
				ts.Hours.ToString() + " " + GetText("Hour") + ", " + ts.Minutes + " " + GetText("Minute") + ", " + ts.Seconds + " " + GetText("Second");
		}

		/// <summary>
		/// Formate speed to Hz
		/// </summary>
		/// <param name="lSpeed"></param>
		/// <returns>stringSpeed</returns>
		private string formatSpeed(Int64 lSpeed)
		{
			//Format number to Hz
			float floatSpeed = 0;
			string stringSpeed = "";
			NumberFormatInfo myNfi = new NumberFormatInfo();

			if (lSpeed < 1000 ) 
			{
				//less than 1GHz
				stringSpeed = lSpeed.ToString() + "MHz";
			}
			else 
			{
				//convert to Giga Hz
				floatSpeed = (float) lSpeed / 1000;
				stringSpeed = floatSpeed.ToString() + "GHz";
			}

			return stringSpeed;
		}

		/// <summary>
		/// formatnumber to KB
		/// </summary>
		/// <param name="lSize"></param>
		/// <param name="booleanFormatOnly"></param>
		/// <returns>stringSize + " KB"</returns>
		private string formatSize(Int64 lSize, bool booleanFormatOnly)
		{
			//Format number to KB
			string stringSize = "";
			NumberFormatInfo myNfi = new NumberFormatInfo();

			Int64 lKBSize = 0;

			if (lSize < 1024 ) 
			{
				if (lSize == 0) 
				{
					//zero byte
					stringSize = "0";
				}
				else 
				{
					//less than 1K but not zero byte
					stringSize = "1";
				}
			}
			else 
			{
				if (booleanFormatOnly == false)
				{
					//convert to KB
					lKBSize = lSize / 1024;
				}
				else 
				{
					lKBSize = lSize;
				}

				//format number with default format
				stringSize = lKBSize.ToString("n",myNfi);
				//remove decimal
				stringSize = stringSize.Replace(".00", "");
			}

			return stringSize + " KB";
		}

		string GetText(string szText)
		{
			return Portal.API.Language.GetText(Portal.API.Module.GetModuleControl(this), szText);
		}

		#region Vom Web Form-Designer generierter Code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: Dieser Aufruf ist für den ASP.NET Web Form-Designer erforderlich.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Erforderliche Methode für die Designerunterstützung
		///		Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
