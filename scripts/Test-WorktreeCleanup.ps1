$ErrorActionPreference = 'Stop'

$cleanupScript = Join-Path $PSScriptRoot 'WorktreeCleanup.ps1'
$testRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("worktree-cleanup-{0}" -f [guid]::NewGuid())
$primaryRoot = Join-Path $testRoot 'primary'
$isolatedRoot = Join-Path $testRoot 'isolated'
$script:gitCalled = $false

New-Item -ItemType Directory -Path $primaryRoot | Out-Null
New-Item -ItemType Directory -Path $isolatedRoot | Out-Null

function global:git {
    $script:gitCalled = $true
    if ($args -contains 'remove') {
        Remove-Item -LiteralPath $isolatedRoot -Recurse -Force
    }
}

try {
    . $cleanupScript

    $output = Remove-MergedWorktree -Repo "$primaryRoot\" -Worktree $primaryRoot.ToUpperInvariant() -Label '#test' 6>&1

    if ($script:gitCalled) {
        throw 'Primary-checkout protection ran after a git operation.'
    }

    if (-not (Test-Path -LiteralPath $primaryRoot)) {
        throw 'Primary checkout was removed.'
    }

    if (($output -join "`n") -notmatch 'primary checkout') {
        throw 'Cleanup must explain why the primary checkout was preserved.'
    }

    Remove-MergedWorktree -Repo $primaryRoot -Worktree $isolatedRoot -Label '#test'

    if (-not $script:gitCalled) {
        throw 'Normal isolated-worktree cleanup did not run git.'
    }

    if (Test-Path -LiteralPath $isolatedRoot) {
        throw 'Normal isolated worktree was not removed.'
    }
}
finally {
    Remove-Item -LiteralPath Function:\git -ErrorAction SilentlyContinue
    Remove-Item -LiteralPath $testRoot -Recurse -Force -ErrorAction SilentlyContinue
}

Write-Output 'OK primary checkout cleanup guard passed.'
