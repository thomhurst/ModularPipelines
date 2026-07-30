$ErrorActionPreference = 'Stop'

$repositoryRoot = Resolve-Path (Join-Path $PSScriptRoot '../../..')
$legacyFiles = @(
    & git -C $repositoryRoot grep -l --fixed-strings '[Description(' -- 'src/**/*.Generated.cs' 2>$null
)
$grepExitCode = $LASTEXITCODE

if ($grepExitCode -gt 1) {
    throw "git grep failed with exit code $grepExitCode."
}

if ($legacyFiles.Count -eq 0) {
    Write-Host 'Generated enum metadata uses EnumValueAttribute.'
    exit 0
}

Write-Error (
    "Generated files still use DescriptionAttribute for CLI enum values:`n{0}" -f
    ($legacyFiles -join "`n")
)
exit 1
