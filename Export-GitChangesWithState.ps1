# Definizione delle variabili
$repoPath = "D:\test\aspnetCore\AirlineBookingSystem"
$baseExportPath = "$env:USERPROFILE\Desktop\ChatGPT_Export"
$stateFile = "$baseExportPath\last_commit_sha.txt"

# Pulisci la cartella di esportazione
if (Test-Path $baseExportPath) {
    Remove-Item $baseExportPath -Recurse -Force
}
New-Item -ItemType Directory -Path $baseExportPath -Force | Out-Null

# Ottieni il SHA del commit corrente
$currentCommit = git --git-dir="$repoPath\.git" --work-tree=$repoPath rev-parse HEAD
$currentCommit = $currentCommit.Trim()

# Se non c'è un commit precedente o è diverso
$previousCommit = if (Test-Path $stateFile) { (Get-Content $stateFile).Trim() } else { "" }

if ($currentCommit -ne $previousCommit) {
    Write-Host "Nuovo commit rilevato: $currentCommit"

    # Ottieni i file modificati nell'ultimo commit
    $filesOutput = git --git-dir="$repoPath\.git" --work-tree=$repoPath diff --name-only $previousCommit..HEAD
    $modifiedFiles = $filesOutput -split "`r?`n" | Where-Object { $_.Trim() -ne "" }

    if ($modifiedFiles.Count -eq 0) {
        Write-Host "Nessun file modificato in questo commit."
        Set-Content -Path $stateFile -Value $currentCommit
        exit
    }

    Write-Host "File modificati:"
    $modifiedFiles | ForEach-Object { Write-Host " - $_" }

    # Copia solo i file modificati
    foreach ($file in $modifiedFiles) {
        $source = Join-Path $repoPath $file
        $destination = Join-Path $baseExportPath $file
        $destDir = Split-Path $destination -Parent

        if (!(Test-Path $destDir)) {
            New-Item -ItemType Directory -Path $destDir -Force | Out-Null
        }

        Copy-Item $source -Destination $destination -Force
    }

    # Crea ZIP
    $timestamp = Get-Date -Format "dd-MM-yyyy_HHmmss"
    $zipName = "ChatGPT_Export_$timestamp.zip"
    $zipFullPath = "$env:USERPROFILE\Desktop\$zipName"

    Compress-Archive -Path "$baseExportPath\*" -DestinationPath $zipFullPath -Force

    Write-Host "ZIP creato: $zipFullPath"

    # Salva il nuovo SHA
    Set-Content -Path $stateFile -Value $currentCommit
} else {
    Write-Host "Nessun nuovo commit da esportare."
}

Write-Host "Script completato."