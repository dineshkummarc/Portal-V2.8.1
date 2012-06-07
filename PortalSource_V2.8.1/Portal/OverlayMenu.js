function OverlayMenuOnMouseOver(ctrl)
{
	ctrl.style.color = '#ffffff';
	ctrl.style.background = '#333399';
}
function OverlayMenuOnMouseOut(ctrl)
{
	ctrl.style.color = '#000000';
	ctrl.style.background = '#ffffff';
}

function getLeft(l)
{
	if (l.offsetParent) return (l.offsetLeft + getLeft(l.offsetParent));
	else return (l.offsetLeft);
}
function getTop(l)
{
	if (l.offsetParent) return (l.offsetTop + getTop(l.offsetParent));
	else return (l.offsetTop);
}

function CloseOverlayMenu(ctrl)
{
	ctrl.style.display = 'none';
}
function CloseAllOverlayMenu()
{
	menus = document.getElementsByTagName("div");
	for(i=0;i<menus.length;i++)
	{
		if(menus[i].className == 'OverlayMenu')
		{
			CloseOverlayMenu(menus[i])
		}
	}
}	
function OpenOverlayMenu(ctrl, root)
{
	if(ctrl.style.display == 'block') 
	{
		CloseOverlayMenu(ctrl);
		return;
	}
	
	CloseAllOverlayMenu();
	
	ctrl.style.display = 'block';
	ctrl.style.left = getLeft(root) - ctrl.clientWidth - 4;
	ctrl.style.top = getTop(root);
}

