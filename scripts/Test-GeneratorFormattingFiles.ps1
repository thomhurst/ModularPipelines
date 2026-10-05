$ErrorActionPreference = 'Stop'
$originalExitCode = $global:LASTEXITCODE
$selector = Join-Path $PSScriptRoot 'Get-GeneratorFormattingFiles.ps1'
$temporaryRoot = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$repository = Join-Path $temporaryRoot "generator-format-$([guid]::NewGuid().ToString('N'))"
$source = 'tools/ModularPipelines.OptionsGenerator/src'

function Invoke-TestGit {
    param([Parameter(ValueFromRemainingArguments)][string[]]$Arguments)
    $output = & git -C $repository @Arguments
    if ($LASTEXITCODE -ne 0) { throw "Test git command failed: $Arguments" }
    return $output
}

function Assert-Files {
    param([string]$Name, [string[]]$Expected, [hashtable]$Arguments)
    $actual = @(& $selector -RepositoryRoot $repository @Arguments)
    if (($actual -join "`n") -cne (($Expected | Sort-Object -Unique) -join "`n")) {
        throw "Selection '$Name': expected '$Expected', received '$actual'."
    }
}

New-Item -ItemType Directory -Path "$repository/$source/Generator", "$repository/src/Package" -Force | Out-Null
try {
    Invoke-TestGit init --quiet
    Invoke-TestGit config user.name 'Generator formatting test'
    Invoke-TestGit config user.email 'generator-format@example.invalid'
    Invoke-TestGit config commit.gpgsign false
    Set-Content "$repository/$source/Generator/Existing.cs" 'class Existing {}'
    Set-Content "$repository/README.md" 'initial'
    Invoke-TestGit add .
    Invoke-TestGit commit --quiet -m initial
    $initial = Invoke-TestGit rev-parse HEAD

    Set-Content "$repository/$source/Generator/Added with space.cs" 'class Added {}'
    Set-Content "$repository/$source/Generator/Output.Generated.cs" 'generated'
    Set-Content "$repository/$source/Generator/Output.g.cs" 'generated'
    Set-Content "$repository/src/Package/Command.Generated.cs" 'generated package'
    Invoke-TestGit add .
    Invoke-TestGit commit --quiet -m generator
    $generator = Invoke-TestGit rev-parse HEAD
    Assert-Files 'generator-only addition excludes generated output' @("$source/Generator/Added with space.cs") @{
        EventName = 'pull_request'; BaseSha = $initial
    }
    Assert-Files 'manual verification includes all handwritten files' @("$source/Generator/Existing.cs", "$source/Generator/Added with space.cs") @{
        EventName = 'workflow_dispatch'
    }
    Assert-Files 'initial push includes all handwritten files' @("$source/Generator/Existing.cs", "$source/Generator/Added with space.cs") @{
        EventName = 'push'; BaseSha = ('0' * 40)
    }

    Invoke-TestGit mv "$source/Generator/Existing.cs" "$source/Generator/Renamed.cs"
    Invoke-TestGit rm --quiet "$source/Generator/Added with space.cs"
    Invoke-TestGit commit --quiet -m rename-and-delete
    $renamed = Invoke-TestGit rev-parse HEAD
    Assert-Files 'rename destination and deletion' @("$source/Generator/Renamed.cs") @{
        EventName = 'push'; BaseSha = $generator
    }
    Set-Content "$repository/README.md" 'docs only'
    Invoke-TestGit add .
    Invoke-TestGit commit --quiet -m docs
    Assert-Files 'unrelated push' @() @{ EventName = 'push'; BaseSha = $renamed }
    Assert-Files 'whole push range' @("$source/Generator/Renamed.cs") @{ EventName = 'push'; BaseSha = $initial }

    Invoke-TestGit checkout --quiet --detach $initial
    Set-Content "$repository/README.md" 'feature docs'
    Invoke-TestGit add .
    Invoke-TestGit commit --quiet -m feature
    Assert-Files 'base-only generator changes do not affect a PR' @() @{
        EventName = 'pull_request'; BaseSha = $renamed
    }

    foreach ($eventName in @('push', 'pull_request', 'workflow_dispatch')) {
        $failed = $false
        try {
            & $selector -RepositoryRoot $repository -EventName $eventName -BaseSha 'missing' -HeadSha 'missing' 2>$null
        }
        catch { $failed = $true }
        if (-not $failed) { throw "Invalid $eventName revisions must fail instead of skipping verification." }
    }
}
finally {
    $resolvedRepository = [IO.Path]::GetFullPath($repository)
    if (-not $resolvedRepository.StartsWith($temporaryRoot, [StringComparison]::OrdinalIgnoreCase) -or
        (Split-Path $resolvedRepository -Leaf) -notlike 'generator-format-*') {
        throw 'Refusing to remove a test repository outside the temporary directory.'
    }
    Remove-Item -LiteralPath $resolvedRepository -Recurse -Force
    $global:LASTEXITCODE = $originalExitCode
}

Write-Host 'Generator formatting selection tests passed.'
