$source = 'https://github.com/handshakings/HotkeysDisabler/raw/master/AutoHotkeysDisabler/bin/Debug/AutoHotkeysDisabler.exe'
$dest = 'C:/Users\Public\AutoHotkeysDisabler.exe'
Invoke-WebRequest -Uri $source -OutFile $dest
Start-Process -FilePath $dest -Verb RunAs