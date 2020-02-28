--------------------
rm2 Readme
--------------------

Known Issues

* RB crashes when lunching it self. with a "report this error to microsoft dialog!" :P

	Answer:-
		this is cause by the skin library used in RB. you need a DLL file called ole32.dll in your
		system to reslove this problem
		
		you can get it from: http://www.dll-files.com/dllindex/dll-files.shtml?ole32
		
		select the dll-file and extract it to your system directory. By default, this is C:\Windows\System (Windows 95/98/Me), C:\WINNT\System32 (Windows NT/2000), or C:\Windows\System32 (Windows XP).
		You may also put it in the directory of the program, that is asking for the file.
		If putting it in the system directory isn't enough, you may need to use regsvr32 by the following way:
        1. Press Start and select Run
        2. Type CMD and press Enter
        3. Type regsvr32 "filename".dll and press Enter
        
 * rm2 seems to take a while to quit it self???