function Invoke-WindowsStartupProbe {
    param(
        [string]$FilePath,
        [string[]]$Arguments,
        [int]$TimeoutSeconds = 10
    )

    $process = [Diagnostics.Process]::new()
    $process.StartInfo.FileName = $FilePath
    $process.StartInfo.UseShellExecute = $false
    $process.StartInfo.CreateNoWindow = $true
    foreach ($argument in $Arguments) {
        $process.StartInfo.ArgumentList.Add($argument)
    }

    $timer = [Diagnostics.Stopwatch]::StartNew()
    $exitCode = 126
    $errorType = $null
    try {
        if ($process.Start()) {
            if ($process.WaitForExit($TimeoutSeconds * 1000)) {
                $exitCode = $process.ExitCode
            }
            else {
                $exitCode = 124
                $process.Kill($true)
                $null = $process.WaitForExit(1000)
            }
        }
    }
    catch {
        # Exception messages may contain arbitrary arguments or paths.
        $errorType = $_.Exception.GetType().FullName
    }
    finally {
        $process.Dispose()
    }

    [pscustomobject]@{
        Executable = [IO.Path]::GetFileName($FilePath)
        ExitCode = $exitCode
        ErrorType = $errorType
        ElapsedMilliseconds = $timer.ElapsedMilliseconds
    }
}

function ConvertTo-WindowsStartupEvent {
    param($Record, [string]$LogName)

    $data = [ordered]@{}
    $xml = [xml]$Record.ToXml()
    foreach ($item in $xml.Event.EventData.Data) {
        if ($item.Name -in @('AppName', 'AppVersion', 'ModuleName', 'ModuleVersion', 'ExceptionCode', 'ProcessId')) {
            $data[$item.Name] = $item.InnerText
        }
    }
    [pscustomobject]@{
        Log = $LogName
        Provider = $Record.ProviderName
        Id = $Record.Id
        TimeCreated = $Record.TimeCreated
        Data = $data
    }
}

function Write-WindowsStartupSnapshot {
    param([string]$Phase, [switch]$IncludeEvents)

    Write-Output "Windows startup snapshot: $Phase at $([DateTimeOffset]::UtcNow.ToString('O'))"
    # No environment, command lines, paths, or arbitrary event messages are collected.
    $processes = @(Get-Process)
    Write-Output "Process count: $($processes.Count); total handles: $(($processes | Measure-Object Handles -Sum).Sum)"
    try {
        $processes | Where-Object ProcessName -Match '^(conhost|OpenConsole|pwsh|dotnet|MSBuild|VBCSCompiler|Runner\.(Worker|Listener))$' |
            Select-Object Id, ProcessName, SessionId, Handles, WorkingSet64 |
            ConvertTo-Json -Compress | Write-Output
    }
    finally {
        foreach ($process in $processes) { $process.Dispose() }
    }

    if (-not $IsWindows -or -not $IncludeEvents) {
        return
    }

    foreach ($logName in @('Application', 'System')) {
        $reader = $null
        try {
            # Bound both the history and record count. ReadEvent also has a deadline.
            $query = [Diagnostics.Eventing.Reader.EventLogQuery]::new(
                $logName, [Diagnostics.Eventing.Reader.PathType]::LogName,
                '*[System[(EventID=1000 or EventID=1001 or EventID=26 or EventID=243 or EventID=2004) and TimeCreated[timediff(@SystemTime) <= 120000]]]')
            $query.ReverseDirection = $true
            $reader = [Diagnostics.Eventing.Reader.EventLogReader]::new($query)
            for ($index = 0; $index -lt 20; $index++) {
                $record = $reader.ReadEvent([TimeSpan]::FromSeconds(1))
                if ($null -eq $record) { break }
                try {
                    ConvertTo-WindowsStartupEvent -Record $record -LogName $logName |
                        ConvertTo-Json -Depth 4 -Compress | Write-Output
                }
                finally {
                    $record.Dispose()
                }
            }
        }
        catch {
            Write-Output "Windows event collection unavailable: $logName ($($_.Exception.GetType().FullName))"
        }
        finally {
            if ($null -ne $reader) { $reader.Dispose() }
        }
    }
}
