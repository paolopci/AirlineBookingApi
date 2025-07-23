# Cartelle di partenza e destinazione
$repoPath = "D:\test\aspnetCore\AirlineBookingSystem"     # <-- Modifica qui
$exportPath = "$env:USERPROFILE\Desktop\ChatGPT_Export"

if (Test-Path $exportPath) {
    Remove-Item $exportPath -Recurse -Force
}
New-Item -ItemType Directory -Path $exportPath | Out-Null

Set-Location $repoPath

# File modificati nell'ultimo commit
$files = git diff-tree --no-commit-id --name-only -r HEAD

# (Opzionale) anche file non tracciati
$untrackedFiles = git ls-files --others --exclude-standard
$files += $untrackedFiles

foreach ($file in $files) {
    $source = Join-Path $repoPath $file
    $destination = Join-Path $exportPath $file
    $destinationDir = Split-Path $destination -Parent

    if (!(Test-Path $destinationDir)) {
        New-Item -ItemType Directory -Path $destinationDir -Force | Out-Null
    }

    if (Test-Path $source) {
        Copy-Item $source -Destination $destination -Force
    }
}

# Comprime la cartella in zip
$zipPath = "$exportPath.zip"
if (Test-Path $zipPath) {
    Remove-Item $zipPath -Force
}
Compress-Archive -Path "$exportPath\*" -DestinationPath $zipPath

Write-Host "✅ File modificati esportati e compressi in: $zipPath"
