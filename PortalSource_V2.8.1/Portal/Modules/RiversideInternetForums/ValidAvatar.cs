using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace RiversideInternet.WebSolution
{
	public class ValidAvatar : BaseValidator, INamingContainer
	{
		private HtmlInputFile	_inputFile;

		public ValidAvatar()
		{
		}

		protected override bool EvaluateIsValid()
		{
			if (_inputFile != null && _inputFile.PostedFile != null && _inputFile.PostedFile.FileName != null && _inputFile.PostedFile.FileName != string.Empty)
			{
				string contentType = _inputFile.PostedFile.ContentType;
				int contentLength  = _inputFile.PostedFile.ContentLength;

				if (contentType != "image/pjpeg" && contentType != "image/gif" && contentType != "image/jpeg" && contentType != "image/x-png")
				{
					ErrorMessage = "Only GIF, JPEG and PNG image types are supported.";
					return false;
				}

				if (contentLength > 102400)
				{
					ErrorMessage = "Avatar images must be less than 100K in size.";
					return false;
				}

				bool dimensionsCorrect = false;

				try
				{
					System.Drawing.Image image = System.Drawing.Image.FromStream(_inputFile.PostedFile.InputStream);
					dimensionsCorrect = (image.Width > 0 && image.Width <= 150) && (image.Height > 0 && image.Height <= 150);
				}
				catch (Exception)
				{
				}

				if (!dimensionsCorrect)
				{
					ErrorMessage = "Avatar images must be no larger than 150 by 150 pixels.";
					return false;
				}
			}

			return true;
		}

		public HtmlInputFile InputFile
		{
			get
			{
				return _inputFile;
			}
			set
			{
				_inputFile = value;
			}
		}
	}
}
