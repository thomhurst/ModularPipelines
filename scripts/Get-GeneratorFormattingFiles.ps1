[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [ValidateSet('pull_request', 'push', 'workflow_dispatch')]
    [string]$EventName,
    [string]$BaseSha,
    [string]$HeadSha = 'HEAD',
    [string]$RepositoryRoot = (Split-Path $PSScriptRoot -Parent)
)

$ErrorActionPreference = 'Stop'
$sourcePath = 'tools/ModularPipelines.OptionsGenerator/src'

if ($EventName -eq 'workflow_dispatch' -or
    ($EventName -eq 'push' -and ([string]::IsNullOrWhiteSpace($BaseSha) -or $BaseSha -match '^0+$'))) {
    $paths = @(git -C $RepositoryRoot -c core.quotepath=false ls-tree -r --name-only $HeadSha -- $sourcePath)
}
else {
    if ([string]::IsNullOrWhiteSpace($BaseSha)) {
        throw 'A base commit is required to select generator formatting files.'
    }

    if ($EventName -eq 'pull_request') {
        $BaseSha = git -C $RepositoryRoot merge-base $BaseSha $HeadSha
        if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($BaseSha)) {
            throw 'Unable to determine the pull request merge base.'
        }
    }

    # Deleted files cannot be formatted; a rename checks its surviving destination.
    $paths = @(git -C $RepositoryRoot -c core.quotepath=false diff --name-only --diff-filter=ACMRT $BaseSha $HeadSha -- $sourcePath)
}

if ($LASTEXITCODE -ne 0) {
    throw 'Unable to determine generator formatting files.'
}

$paths | Where-Object {
    $_ -clike '*.cs' -and $_ -cnotlike '*.Generated.cs' -and $_ -cnotlike '*.g.cs'
} | Sort-Object -Unique
