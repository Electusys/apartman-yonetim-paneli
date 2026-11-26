$adb = "C:\Program Files (x86)\Android\android-sdk\platform-tools\adb.exe"
if (-not (Test-Path -LiteralPath $adb)) {
    Write-Host "adb bulunamadı: $adb"
    exit 1
}
Write-Host "adb: $adb"
& $adb start-server
& $adb devices -l
& $adb reverse tcp:3306 tcp:3306
Write-Host "Telefonun 127.0.0.1:3306 adresi bu bilgisayarın MySQL portuna yönlendirildi."
