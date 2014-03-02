//TODO check how to pass some parameters

var WshShell = new ActiveXObject("WScript.Shell");
// var allowedExtensions=".3g2,.3gp,.3gp2,.3gpp,.60d,.ajp,.asf,.asx,.avchd,.avi,.bik,.bix,.box,.cam,.dat,.divx,.dmf,.dv,.dvr-ms,.evo,.flc,.fli,.flic,.flv,.flx,.gvi,.gvp,.h264,.m1v,.m2p,.m2ts,.m2v,.m4e,.m4v,.mjp,.mjpeg,.mjpg,.mkv,.moov,.mov,.movhd,.movie,.movx,.mp4,.mpe,.mpeg,.mpg,.mpv,.mpv2,.mxf,.nsv,.nut,.ogg,.ogm,.omf,.ps,.qt,.ram,.rm,.rmvb,.swf,.ts,.vfw,.vid,.video,.viv,.vivo,.vob,.vro,.wm,.wmv,.wmx,.wrap,.wvx,.wx,.x264,.xvid".split(',');
var allowedExtensions = [".mp4"];

for (var i = 0; i < allowedExtensions.length; i++) {
	var extension = allowedExtensions[i];
	var progId;
	try	{
		//get default program id
		progId = WshShell.RegRead("HKCR\\" + extension + "\\");
		console.log(progId);

		try	{
			var regPath = "HKCR\\" + progId + "\\shell\\Find subtitles\\command\\";
			var regValue = "pathToSubtitlerApp";
			//write additional entry under default programId
			WshShell.RegWrite(regPath, regValue, "REG_SZ");

			//check written entry
			var writtenValue = WshShell.RegRead(regPath);
			console.log("Written value: " + writtenValue + " to " + regPath);
		}
		catch (e) {
			console.log(e);
		}

		
	} catch (e) {
		console.log(e);
	}
	
};







//HKEY_CURRENT_USER		- HKCU
//HKEY_LOCAL_MACHINE	- HKLM
//HKEY_CLASSES_ROOT		- HKCR
//HKEY_USERS			- HKEY_USERS
//HKEY_CURRENT_CONFIG	- HKEY_CURRENT_CONFIG