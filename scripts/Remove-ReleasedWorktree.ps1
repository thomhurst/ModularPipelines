#!/usr/bin/env pwsh
# Local checkout lifecycle. Keep branches; retain detached commits before removal.
# Called while AgentLocks still owns the lock, before another worker can acquire it.
function Remove-ReleasedWorktree {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string]$Worktree,
        [Parameter(Mandatory)][string]$LockName
    )

    # Resolve each ancestor as well as the leaf: Resolve-Path and GetFullPath
    # preserve aliases such as macOS /var, while Git reports physical paths.
    function Resolve-PhysicalDirectory([string]$Path) {
        $directory = [IO.DirectoryInfo]::new([IO.Path]::GetFullPath($Path))
        $linkTarget = if ($directory.Exists) { $directory.ResolveLinkTarget($true) } else { $null }
        if ($null -ne $linkTarget) { return Resolve-PhysicalDirectory $linkTarget.FullName }
        if ($null -eq $directory.Parent) { return $directory.FullName }
        return Join-Path (Resolve-PhysicalDirectory $directory.Parent.FullName) $directory.Name
    }

    if (-not (Test-Path -LiteralPath $Worktree)) { return }
    $target = Resolve-PhysicalDirectory $Worktree
    $common = git rev-parse --path-format=absolute --git-common-dir 2>$null
    if ($LASTEXITCODE -ne 0) { throw 'Cannot identify the calling repository.' }
    $common = Resolve-PhysicalDirectory $common
    $targetCommon = git -C $target rev-parse --path-format=absolute --git-common-dir 2>$null
    if ($LASTEXITCODE -ne 0 -or (Resolve-PhysicalDirectory $targetCommon) -ne $common) {
        throw "Worktree belongs to another repository: $target"
    }
    $top = git -C $target rev-parse --show-toplevel 2>$null
    if ($LASTEXITCODE -ne 0 -or (Resolve-PhysicalDirectory $top) -ne $target) {
        throw "Not a worktree root: $target"
    }
    $gitDirectory = git -C $target rev-parse --absolute-git-dir 2>$null
    if ($LASTEXITCODE -ne 0 -or (Resolve-PhysicalDirectory $gitDirectory) -eq $common -or
        (Test-Path -LiteralPath (Join-Path $gitDirectory 'locked'))) {
        Write-Host "Preserving main or Git-locked worktree: $target"
        return
    }
    . (Join-Path $PSScriptRoot 'WorktreeCleanup.ps1')
    $marker = git -C $target config --worktree --get agent.lockName 2>$null
    if ($LASTEXITCODE -ne 0 -or $marker -cne $LockName -or -not (Test-WorktreeLockMarkerOwner -Worktree $target)) {
        Write-Host "Preserving worktree without a matching lock marker: $target"
        return
    }
    $entries = @(git worktree list --porcelain)
    if ($LASTEXITCODE -ne 0) { throw 'Cannot list registered worktrees.' }
    $paths = @($entries | Where-Object { $_.StartsWith('worktree ') } |
        ForEach-Object { Resolve-PhysicalDirectory $_.Substring(9) })
    if ($target -notin $paths -or $paths.Count -eq 0) { throw "Unregistered worktree: $target" }
    $repo = $paths[0]
    $relative = [IO.Path]::GetRelativePath($repo, $target)
    if ($relative -eq '.' -or (-not [IO.Path]::IsPathRooted($relative) -and
        $relative -ne '..' -and -not $relative.StartsWith("..$([IO.Path]::DirectorySeparatorChar)"))) {
        Write-Host "Preserving main or harness-managed worktree: $target"
        return
    }

    $status = @(git -C $target status --porcelain=v1 --untracked-files=all --ignored=matching 2>$null)
    if ($LASTEXITCODE -ne 0) { throw "Cannot inspect worktree: $target" }
    $work = @(Get-WorktreeUncommittedWork -Status $status)
    if ($work.Count -gt 0) {
        Write-Host "Preserving dirty worktree: $target ($($work[0]))"
        return
    }
    # A stable ref keeps unpublished detached commits reachable after Git prunes the
    # worktree reflog.
    Save-DetachedWorktreeHead -Repo $repo -Worktree $target
    # Move this child shell out of the directory before deleting it on Windows. A parent
    # shell started inside the worktree still blocks deletion; the sweep retries it.
    Set-Location -LiteralPath $repo
    [Environment]::CurrentDirectory = $repo
    if (-not (Remove-WorktreeDirectoryLinks -Worktree $target)) {
        Write-Host "Preserving released worktree with directory links that could not be detached: $target"
        return
    }
    $pending = Register-PendingWorktreeRemoval -Repo $repo -Worktree $target -LockNames @($LockName)
    git -C $repo -c core.longpaths=true worktree remove --force -- $target
    if ($LASTEXITCODE -ne 0) {
        if (-not $pending) { throw "Could not remove released worktree: $target" }
        Write-Host "Released worktree removal is pending: $target is in use. Remove-MergedWorktrees.ps1 retries it; release from the shared checkout to avoid this."
        return
    }
    if ($pending) { Remove-Item -LiteralPath $pending -Force -ErrorAction SilentlyContinue }
    Write-Host "Removed released worktree: $target"
}
