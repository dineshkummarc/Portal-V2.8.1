<%@ Page Language="VB" Debug="true" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="System.Drawing.Imaging" %>
<%@ Import Namespace="System.Drawing.Drawing2d" %>
<%
    ' useage = <img src="counter/counter.aspx?src=digits.gif&digits=5&id=countername">


    ' ***** MANAGE COUNTER VALUE CREATION, RESTORATION, AND SAVING *****

    ' Get the counter ID (unique to each counter in the site)
    Dim counterid As String = Request("id")

    ' Get current counter value
    Dim value As Integer = CInt(Application("Counter_" & counterid))
    
    Dim path As String = Portal.API.Config.GetModuleDataPhysicalPath()
   

    If Session("CounterTemp_" & counterid) Is Nothing Then
	
        ' If the text file exists load the saved value 
        ' Always loading it from the file rather than just using the application variable allows for manual modification of counter values while the application is running (by editing the text file). To stop it from being able to be modified just uncomment the following 'if..then' and the 'end if'.  This will give a slight performance boost to the counter incrementing as it will stop a file operation.  Yes, I'm an optimization freak! :)
        'if value = 0 then

        If File.Exists(path & "HitCounter\" & counterid & ".txt") Then
            Dim sr As StreamReader = File.OpenText(path & "HitCounter\" & counterid & ".txt")
            value = CInt(sr.ReadLine().ToString())
            sr.Close()
        End If
        'end if
	
        ' Increment counter
        value += 1
	
        ' Save counter to an application var (the locks are there to make sure noone else changes it at the same time)
        Application.Lock()
        Application("Counter_" & counterid) = value.ToString()
        Application.UnLock()
	
        ' Save counter to a text file
        If Directory.Exists(path & "HitCounter") = False Then
            Directory.CreateDirectory(path & "HitCounter")
        End If
        
        Dim fs As FileStream = New FileStream(path & "HitCounter\" & counterid & ".txt", FileMode.Create, FileAccess.Write)
        Dim sw As StreamWriter = New StreamWriter(fs)
        sw.WriteLine(Convert.ToString(value))
        sw.Close()
        fs.Close()
	
        ' Set a session variable so this counter doesn't fire again in the current session
        Session.Add(("CounterTemp_" & counterid), "True")
    End If

    ' ***** CREATE OUTPUT GRAPHIC FOR THE COUNTER *****

    ' Load digits graphic (must be in 0 through 9 format in graphic w/ all digits of set width)
    Dim i As System.Drawing.Image = System.Drawing.Image.FromFile(Server.MapPath(Request("src")))

    ' Get digit dynamics from the graphic
    Dim dgwidth As Integer = i.Width / 10
    Dim dgheight As Integer = i.Height

    ' Get number of digits to display in the output graphic
    Dim digits As Integer = CInt(Request("digits"))

    ' Create output object
    Dim imgOutput As New Bitmap(dgwidth * digits, dgheight, PixelFormat.Format24bppRgb)
    Dim g As Graphics = Graphics.FromImage(imgOutput)

    Dim j As Integer, dg As Integer
    For j = 0 To (digits - 1)
        ' Extract digit from value
        dg = Fix(value / (10 ^ (digits - j - 1))) - Fix(value / (10 ^ (digits - j))) * 10
        ' Add digit to the output graphic
        g.DrawImage(i, New Rectangle(j * dgwidth, 0, dgwidth, dgheight), New Rectangle(dg * dgwidth, 0, dgwidth, dgheight), GraphicsUnit.Pixel)
	
    Next j

    ' Set the content type and return output image
    Response.ContentType = "image/jpeg"
    imgOutput.Save(Response.OutputStream, ImageFormat.Jpeg)

    ' Clean up
    g.Dispose()
    imgOutput.Dispose()
    i.Dispose()
%>
