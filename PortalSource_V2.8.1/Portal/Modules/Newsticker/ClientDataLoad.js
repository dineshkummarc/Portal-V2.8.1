var ActiveRequestList = new Array();
var is_ie = (navigator.userAgent.indexOf('MSIE') >= 0) ? 1 : 0; 
var is_ie5 = (navigator.appVersion.indexOf("MSIE 5.5")!=-1) ? 1 : 0; 
var is_opera = ((navigator.userAgent.indexOf("Opera6")!=-1)||(navigator.userAgent.indexOf("Opera/6")!=-1)) ? 1 : 0; 
//netscape, safari, mozilla behave the same??? 
var is_netscape = (navigator.userAgent.indexOf('Netscape') >= 0) ? 1 : 0; 

function RequestData(requestURL, TargetCtrl)
{    
  // Erzeuge das XmlHttp Objekt zum Ermitteln der Daten.
  var xmlHttp = GetxmlHttpListect(ResponseHandler); 
    
  // Sende die Anforderung als "Get" Anforderung an den Empfänger.
  xmlHttp_Get(xmlHttp, requestURL); 
  
  // Damit wir die Asynchrone Antwort verarbeiten können, legen wir das Objekt zusammen mit dem Ziel-Control 
  // in der Liste ab.
  var NewRequest = new Object();
  NewRequest["XmlHttp"] = xmlHttp;
  NewRequest["TargetCtrl"] = TargetCtrl;
  ActiveRequestList.push(NewRequest);
} 

// Handler der aufgerufen wird, wenn die Asynchrone Antwort eintrifft.
function ResponseHandler() 
{ 
  // Wir suchen in der Liste nach dem Objekt, welches sich im Status 4, bzw. 'complete' befindet.
  for(var nzIndex = 0; nzIndex < ActiveRequestList.length; nzIndex++)
  {
    var xmlHttp = ActiveRequestList[nzIndex]["XmlHttp"];
    if (xmlHttp.readyState == 4 || xmlHttp.readyState == 'complete')
    { 
        // Antwortdaten.
        var strResponse = xmlHttp.responseText; 

        // Füge den Text in das Control ein.
        var objCtrl = document.getElementById(ActiveRequestList[nzIndex]["TargetCtrl"]);
        try
        {
          objCtrl.innerHTML = strResponse; 
        }
        catch(Error)
        {
          objCtrl.innerHTML = "Could not load data";
        }
          
        
        ActiveRequestList.splice(nzIndex, 1);
        nzIndex--;
    } 
  }
} 

// XMLHttp sendet den GET request 
function xmlHttp_Get(xmlhttp, url) 
{ 
    xmlhttp.open('GET', url, true); 
    xmlhttp.send(null); 
} 


function GetxmlHttpListect(handler) 
{ 
    var objXmlHttp = null;    //Holds the local xmlHTTP object instance 

    //Depending on the browser, try to create the xmlHttp object 
    if (is_ie){ 
        //The object to create depends on version of IE 
        //If it isn't ie5, then default to the Msxml2.XMLHTTP object 
        var strObjName = (is_ie5) ? 'Microsoft.XMLHTTP' : 'Msxml2.XMLHTTP'; 
          
        //Attempt to create the object 
        try{ 
            objXmlHttp = new ActiveXObject(strObjName); 
            objXmlHttp.onreadystatechange = handler; 
        } 
        catch(e){ 
        //Object creation errored 
            alert('IE detected, but object could not be created. Verify that active scripting and activeX controls are enabled'); 
            return; 
        } 
    } 
    else if (is_opera){ 
        //Opera has some issues with xmlHttp object functionality 
        alert('Opera detected. The page may not behave as expected.'); 
        return; 
    } 
    else{ 
        // Mozilla | Netscape | Safari 
        objXmlHttp = new XMLHttpRequest(); 
        objXmlHttp.onload = handler; 
        objXmlHttp.onerror = handler; 
    } 
      
    //Return the instantiated object 
    return objXmlHttp; 
} 

