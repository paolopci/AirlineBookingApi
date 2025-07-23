# Definizione delle variabili
$repoPath = "D:\test\aspnetCore\AirlineBookingSystem"
$baseExportPath = "$env:USERPROFILE\Desktop\ChatGPT_Export"
$stateFile = "$baseExportPath\last_commit_sha.txt"
$gitDir = "$repoPath\.git"
$workTree = "$repoPath"

# Debug output iniziale
Write-Host "=== DEBUG INFO ==="
Write-Host "Repo Path: $repoPath"
Write-Host "Export Path: $baseExportPath"
Write-Host "State File: $stateFile"
Write-Host "Git Dir: $gitDir"
Write-Host "Work Tree: $workTree"
Write-Host "=================="

# CREA cartella di esportazione (pulita ogni volta)
if (Test-Path $baseExportPath) {
    Write-Host "Rimozione cartella esistente: $baseExportPath"
    Remove-Item $baseExportPath -Recurse -Force
}
Write-Host "Creazione nuova cartella: $baseExportPath"
New-Item -ItemType Directory -Path $baseExportPath -Force | Out-Null

# Ottieni il SHA del commit corrente
try {
    $currentCommit = git --git-dir=$gitDir --work-tree=$workTree rev-parse HEAD
    Write-Host "SHA corrente: $currentCommit"
}
catch {
    Write-Host "ERRORE: Impossibile ottenere il commit corrente. Errore: $($_.Exception.Message)"
    return
}

# Validazione del commit corrente
Write-Host "Checking commit validity: git --git-dir=$gitDir --work-tree=$workTree cat-file -e '$currentCommit^{commit}'"
try {
    # Esegui il comando cat-file con debug dettagliato
    $validationResult = git --git-dir=$gitDir --work-tree=$workTree cat-file -e "$currentCommit^{commit}" 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Host "ERRORE: Current commit SHA non valido: $currentCommit"
        Write-Host "Output di errore: $validationResult"
        return
    }
    else {
        Write-Host "Commit valido: $currentCommit"
    }
}
catch {
    Write-Host "ERRORE durante la validazione del commit: $($_.Exception.Message)"
    return
}

# Ottieni il SHA del commit precedente dal file di stato
$previousCommit = $null
if (Test-Path $stateFile) {
    try {
        $previousCommit = (Get-Content $stateFile -Raw).Trim()
        Write-Host "SHA precedente da file: $previousCommit"
        
        # Validazione del commit precedente
        if ($previousCommit -and -not (git --git-dir=$gitDir --work-tree=$workTree cat-file -e "$previousCommit^{commit}" 2>$null)) {
            Write-Host "ATTENZIONE: SHA $previousCommit non valido in questa repo. Trattato come prima esecuzione."
            $previousCommit = $null
        }
    }
    catch {
        Write-Host "ERRORE: Impossibile leggere il file dello stato. Procedo come se fosse la prima esecuzione. Errore: $($_.Exception.Message)"
        $previousCommit = $null
    }
}
else {
    Write-Host "State file non trovato. Procedo come prima esecuzione."
}

# Prima esecuzione: salva e chiudi
if (-not $previousCommit) {
    Write-Host "Prima esecuzione: salvo SHA e chiudo."
    Set-Content -Path $stateFile -Value $currentCommit
    Write-Host "SHA salvato: $currentCommit"
    return
}

# Nessuna differenza tra i commit → niente da fare
if ($previousCommit -eq $currentCommit) {
    Write-Host "Nessun nuovo commit: SHA corrente e precedente sono uguali."
    return
}

# Identifica i file modificati tra i due commit
Write-Host "Controllo differenze tra commit..."
try {
    $filesOutput = git --git-dir=$gitDir --work-tree=$workTree diff --name-only $previousCommit $currentCommit
    Write-Host "`n--- git diff output ---"
    Write-Host $filesOutput
    Write-Host "-----------------------`n"
}
catch {
    Write-Host "ERRORE: Impossibile eseguire git diff. Errore: $($_.Exception.Message)"
    return
}

$files = @()
if ($filesOutput) {
    $files = $filesOutput -split "`r?`n" | Where-Object { $_.Trim() -ne "" }
}

Write-Host "Conteggio file: $($files.Count)"

if ($files.Count -eq 0) {
    Write-Host "Commit nuovo, ma nessun file modificato."
    Set-Content -Path $stateFile -Value $currentCommit
    return
}

Write-Host "File da esportare:"
$files | ForEach-Object { Write-Host " - $_" }

# Copia i file nella cartella export
foreach ($file in $files) {
    $source = Join-Path $repoPath $file
    $destination = Join-Path $baseExportPath $file
    $destinationDir = Split-Path $destination -Parent

    Write-Host "Processing file: $file"
    Write-Host "Source path: $source"
    Write-Host "Destination path: $destination"

    if (!(Test-Path $destinationDir)) {
        Write-Host "Creazione directory: $destinationDir"
        try {
            New-Item -ItemType Directory -Path $destinationDir -Force | Out-Null
            Write-Host "Directory creata con successo"
        }
        catch {
            Write-Host "ERRORE: Impossibile creare directory $destinationDir. Errore: $($_.Exception.Message)"
            continue
        }
    }

    if (Test-Path $source) {
        try {
            Copy-Item $source -Destination $destination -Force -ErrorAction Stop
            Write-Host "SUCCESSO: Copiato $source a $destination"
        }
        catch {
            Write-Host "ERRORE: Impossibile copiare file $source. Errore: $($_.Exception.Message)"
        }
    }
    else {
        Write-Host "WARNING: File sorgente non trovato: $source"
    }
}

# Verifica se la cartella contiene file prima di creare lo zip
Write-Host "Verifica file esportati..."
try {
    $exportedFiles = Get-ChildItem -Path $baseExportPath -Recurse -File | Where-Object { $_.Name -ne "last_commit_sha.txt" }
    Write-Host "Numero di file esportati: $($exportedFiles.Count)"
    
    if ($exportedFiles.Count -eq 0) {
        Write-Host "Nessun file trovato nella cartella di esportazione. Zip non creato."
    }
    else {
        Write-Host "File esportati trovati:"
        $exportedFiles | ForEach-Object { Write-Host " - $($_.FullName)" }
        
        $timestamp = Get-Date -Format "dd-MM-yyyy_HHmmss"
        $zipName = "ChatGPT_Export_$timestamp.zip"
        $zipFullPath = "$env:USERPROFILE\Desktop\$zipName"

        Write-Host "Creazione ZIP: $zipFullPath"
        try {
            if (Test-Path $baseExportPath) {
                # Crea ZIP solo dei file esportati, escludendo il file di stato
                $itemsToZip = Get-ChildItem -Path $baseExportPath -Recurse -File | Where-Object { $_.Name -ne "last_commit_sha.txt" }
                if ($itemsToZip.Count -gt 0) {
                    $tempDir = "$baseExportPath\temp_zip"
                    if (Test-Path $tempDir) { Remove-Item $tempDir -Recurse -Force }
                    New-Item -ItemType Directory -Path $tempDir -Force | Out-Null
                    
                    # Copia i file nella directory temporanea
                    foreach ($item in $itemsToZip) {
                        $relativePath = $item.FullName.Substring($baseExportPath.Length + 1)
                        $destPath = Join-Path $tempDir $relativePath
                        $destDir = Split-Path $destPath -Parent
                        if (!(Test-Path $destDir)) {
                            New-Item -ItemType Directory -Path $destDir -Force | Out-Null
                        }
                        Copy-Item $item.FullName -Destination $destPath -Force
                    }
                    
                    Compress-Archive -Path "$tempDir\*" -DestinationPath $zipFullPath -Force -ErrorAction Stop
                    Remove-Item $tempDir -Recurse -Force
                    Write-Host "Creato con successo: $zipFullPath"
                }
                else {
                    Write-Host "Nessun file da comprimere. ZIP non creato."
                }
            }
            else {
                Write-Host "ERRORE: La cartella di esportazione non esiste. Zip non creato."
            }
        }
        catch {
            Write-Host "ERRORE durante la creazione dello ZIP: $($_.Exception.Message)"
        }
    }
}
catch {
    Write-Host "ERRORE durante la verifica dei file esportati: $($_.Exception.Message)"
}

# Salva SHA corrente
Write-Host "Salvataggio SHA corrente: $currentCommit"
try {
    Set-Content -Path $stateFile -Value $currentCommit
    Write-Host "SHA salvato con successo"
}
catch {
    Write-Host "ERRORE: Impossibile salvare lo SHA nel file di stato. Errore: $($_.Exception.Message)"
}

Write-Host "Script completato."