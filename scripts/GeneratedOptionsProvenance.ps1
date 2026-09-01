Set-StrictMode -Version Latest

function Assert-GeneratedOptionsToken {
    param(
        [Parameter(Mandatory)][string]$Name,
        [Parameter(Mandatory)][string]$Value
    )

    if ($Value -notmatch '^[A-Za-z0-9][A-Za-z0-9._-]*$') {
        throw "$Name contains unsupported characters: '$Value'."
    }
}

function Get-GeneratedOptionsSourcePath {
    return @(
        '.github/workflows/generate-cli-options.yml',
        'Directory.Build.props',
        'Directory.Packages.props',
        'global.json',
        'scripts/GeneratedOptionsProvenance.ps1',
        'scripts/Write-GeneratedOptionsProvenance.ps1',
        'src/ModularPipelines/Attributes/CliOptionValueArity.cs',
        'src/ModularPipelines/Attributes/CommandLinePhase.cs',
        'src/ModularPipelines/Helpers/Internal/WindowsCommandResolver.cs',
        'src/ModularPipelines/Options/AdditionalCommandLineArgument.cs',
        'src/ModularPipelines/Options/CommandLineToolOptions.cs',
        'tools/Directory.Build.props',
        'tools/ModularPipelines.OptionsGenerator'
    )
}

function Get-GeneratedOptionsSourceFingerprint {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string]$RepositoryRoot,
        [string]$Revision = 'HEAD'
    )

    $objectIds = @(
        foreach ($sourcePath in Get-GeneratedOptionsSourcePath) {
            $objectId = git -C $RepositoryRoot rev-parse "${Revision}:$sourcePath"
            if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($objectId)) {
                throw "Could not resolve generated-options source '$sourcePath' at '$Revision'."
            }

            $objectId.Trim()
        }
    )

    $bytes = [Text.Encoding]::UTF8.GetBytes($objectIds -join "`n")
    return [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes)).ToLowerInvariant()
}
