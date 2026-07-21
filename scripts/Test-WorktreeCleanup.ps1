$ErrorActionPreference = 'Stop'

$cleanupScript = Join-Path $PSScriptRoot 'WorktreeCleanup.ps1'
$testRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("worktree-cleanup-{0}" -f [guid]::NewGuid())
$primaryRoot = Join-Path $testRoot 'primary'
$isolatedRoot = Join-Path $testRoot 'isolated'
$aliasRoot = Join-Path $testRoot 'primary-alias'
$casePrimaryRoot = Join-Path $testRoot 'Repo'
$caseIsolatedRoot = Join-Path $testRoot 'repo'
$script:gitCalled = $false

New-Item -ItemType Directory -Path $primaryRoot | Out-Null
New-Item -ItemType Directory -Path $isolatedRoot | Out-Null
New-Item -ItemType Directory -Path (Join-Path $primaryRoot '.git') | Out-Null
Set-Content -LiteralPath (Join-Path $isolatedRoot '.git') -Value 'gitdir: /tmp/fake-worktree'
if (-not $IsWindows) {
    New-Item -ItemType Directory -Path (Join-Path $casePrimaryRoot '.git') | Out-Null
    New-Item -ItemType Directory -Path $caseIsolatedRoot | Out-Null
    Set-Content -LiteralPath (Join-Path $caseIsolatedRoot '.git') -Value 'gitdir: /tmp/fake-case-worktree'
}
if ($IsWindows) {
    New-Item -ItemType Junction -Path $aliasRoot -Target $primaryRoot | Out-Null
}
else {
    New-Item -ItemType SymbolicLink -Path $aliasRoot -Target $primaryRoot | Out-Null
}

function global:git {
    $script:gitCalled = $true
    if ($args -contains 'remove') {
        $target = $args[-1]
        if ($target -eq $aliasRoot) {
            Remove-Item -LiteralPath $aliasRoot -Force
        }
        else {
            Remove-Item -LiteralPath $target -Recurse -Force
        }
    }
}

try {
    . $cleanupScript

    $primarySpelling = if ($IsWindows) { ($primaryRoot + '\').ToUpperInvariant() } else { $primaryRoot + '/' }
    $output = Remove-MergedWorktree -Repo $primaryRoot -Worktree $primarySpelling -Label '#test' 6>&1

    if ($script:gitCalled) {
        throw 'Primary-checkout protection ran after a git operation.'
    }

    if (-not (Test-Path -LiteralPath $primaryRoot)) {
        throw 'Primary checkout was removed.'
    }

    if (($output -join "`n") -notmatch 'primary checkout') {
        throw 'Cleanup must explain why the primary checkout was preserved.'
    }

    $script:gitCalled = $false
    Remove-MergedWorktree -Repo $primaryRoot -Worktree $aliasRoot -Label '#alias'

    if ($script:gitCalled) {
        throw 'A primary-checkout alias reached a git operation.'
    }

    if (-not (Test-Path -LiteralPath $primaryRoot)) {
        throw 'Primary checkout was removed through an alias.'
    }

    Remove-MergedWorktree -Repo $primaryRoot -Worktree $isolatedRoot -Label '#test'

    if (-not $script:gitCalled) {
        throw 'Normal isolated-worktree cleanup did not run git.'
    }

    if (Test-Path -LiteralPath $isolatedRoot) {
        throw 'Normal isolated worktree was not removed.'
    }

    if (-not $IsWindows) {
        $script:gitCalled = $false
        Remove-MergedWorktree -Repo $casePrimaryRoot -Worktree $caseIsolatedRoot -Label '#case'

        if (-not $script:gitCalled -or (Test-Path -LiteralPath $caseIsolatedRoot)) {
            throw 'Case-distinct isolated worktree was incorrectly preserved.'
        }

        if (-not (Test-Path -LiteralPath $casePrimaryRoot)) {
            throw 'Case-distinct primary checkout was removed.'
        }
    }
}
finally {
    Remove-Item -LiteralPath Function:\git -ErrorAction SilentlyContinue
    Remove-Item -LiteralPath $aliasRoot -Force -ErrorAction SilentlyContinue
    Remove-Item -LiteralPath $testRoot -Recurse -Force -ErrorAction SilentlyContinue
}

Write-Output 'OK primary checkout cleanup guard passed.'
