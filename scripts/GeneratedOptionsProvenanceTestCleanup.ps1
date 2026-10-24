function Remove-GeneratedOptionsProvenanceTestArtifacts {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string[]]$LiteralPath,
        [System.Management.Automation.ErrorRecord]$AssertionError
    )

    $cleanupErrors = @()
    foreach ($path in $LiteralPath) {
        for ($attempt = 1; $attempt -le 5; $attempt++) {
            try {
                if (Test-Path -LiteralPath $path -ErrorAction Stop) {
                    Remove-Item -LiteralPath $path -Recurse -Force -ErrorAction Stop
                }
                break
            }
            catch {
                # Directory activity and file locks can briefly outlive a Git command.
                if (($_.Exception -is [IO.IOException] -or
                    $_.Exception.GetBaseException() -is [IO.IOException]) -and $attempt -lt 5) {
                    Start-Sleep -Milliseconds 100
                    continue
                }
                $cleanupErrors += "'$path' (attempt $attempt): $($_.Exception.Message)"
                break
            }
        }
    }

    if ($cleanupErrors.Count -gt 0) {
        $message = "Failed to clean generated-options provenance test artifacts: $($cleanupErrors -join '; ')"
        if ($null -ne $AssertionError) {
            # A finally-block error must not replace the assertion that caused cleanup.
            Write-Warning $message -WarningAction Continue
        }
        else {
            throw $message
        }
    }
}
