$log = Join-Path $PSScriptRoot "ag-ac-sonuc.txt"
"basladi $(Get-Date -Format o)" | Set-Content -LiteralPath $log
netsh interface portproxy delete v4tov4 listenaddress=0.0.0.0 listenport=3306 | Out-String | Add-Content -LiteralPath $log
netsh interface portproxy delete v4tov4 listenaddress=192.168.1.12 listenport=3306 | Out-String | Add-Content -LiteralPath $log
netsh interface portproxy add v4tov4 listenaddress=192.168.1.12 listenport=3306 connectaddress=127.0.0.1 connectport=3306 | Out-String | Add-Content -LiteralPath $log
netsh advfirewall firewall delete rule name="Apartman MySQL 3306" | Out-String | Add-Content -LiteralPath $log
netsh advfirewall firewall add rule name="Apartman MySQL 3306" dir=in action=allow protocol=TCP localport=3306 remoteip=192.168.1.0/24 profile=any | Out-String | Add-Content -LiteralPath $log
"----" | Add-Content -LiteralPath $log
netsh interface portproxy show v4tov4 | Out-String | Add-Content -LiteralPath $log
"bitti $(Get-Date -Format o)" | Add-Content -LiteralPath $log
