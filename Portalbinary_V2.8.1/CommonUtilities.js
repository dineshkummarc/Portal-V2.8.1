function AddLifeguard(nIntervalInMs, nReconnect)
{
  window.setTimeout("LifeguardReconnect(" + nIntervalInMs + ", " + nReconnect + ")", nIntervalInMs);
}

function LifeguardReconnect(nIntervalInMs, nReconnect)
{
  // Get something (not cached) from the server, to refresh the session.
  var DataImg = new Image(1,1);
  DataImg.src = "LifeguardData.aspx";
  
  if(nReconnect > 1)
    AddLifeguard(nIntervalInMs, nReconnect - 1);  // Start next Interval.
  else if(0 == nReconnect)
    AddLifeguard(nIntervalInMs, 0);               // Infinite.    
}

