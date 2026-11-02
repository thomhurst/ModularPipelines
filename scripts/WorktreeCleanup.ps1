#!/usr/bin/env pwsh
# WorktreeCleanup.ps1
# Shared worktree-removal helper, dot-sourced by Merge-Pr.ps1, Remove-MergedWorktrees.ps1,
# Remove-ReleasedWorktree.ps1 and AgentLocks.ps1. Not meant to be run directly.
#
# Removal policy (one place, both callers):
#   - PRESERVE tracked changes and untracked/ignored files outside known generated paths.
#   - CLEAR known build artifacts and root-level workflow output covered by .gitignore.
#   - DETACH directory junctions and symlinks before deleting, so no delete follows a link.
#   - Long-path safe: git's own delete now works because core.longpaths=true is set
#     system-wide; the `\\?\` extended-length Remove-Item is kept as a fallback for
#     environments where that config is missing.

# A named branch whose tip is already on main has no commits of its own: it was just
# cut from main, or never got work. Its tip is main's tip, so commit association and
# rebase-merged PR heads point at whichever PR produced main's tip. Only the branch
# name (tier 1) can prove such a branch merged. Unknown main tip fails closed.
function Test-HeadHasNoWorkBeyondMain {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string]$Repo,
        [Parameter(Mandatory)][string]$Head,
        [AllowNull()][string]$MainTip
    )

    if (-not $MainTip) { return $true }
    git -C $Repo merge-base --is-ancestor $Head $MainTip 2>$null
    return $LASTEXITCODE -eq 0
}

function New-OrdinalStringMap {
    [CmdletBinding()]
    [OutputType([System.Collections.Generic.Dictionary[string, bool]])]
    param()

    return [System.Collections.Generic.Dictionary[string, bool]]::new([System.StringComparer]::Ordinal)
}

$script:DisposableWorktreeGeneratedDirectories = New-OrdinalStringMap
foreach ($directory in @(
    '.artifacts',
    '.vs',
    '__pycache__',
    'ARM',
    'ARM64',
    'artifacts',
    'benchmark-results',
    'BenchmarkDotNet.Artifacts',
    'bin',
    'bld',
    'CodeCoverage',
    'Debug',
    'DebugPublic',
    'log',
    'logs',
    'node_modules',
    'obj',
    'Release',
    'Releases',
    'results',
    'StrykerOutput',
    'temptest',
    'TestResults',
    'Win32',
    'x64',
    'x86'
)) {
    $script:DisposableWorktreeGeneratedDirectories[$directory] = $true
}

$script:DisposableWorktreeScopedDirectories = @(
    'docs/.cache',
    'docs/.docusaurus',
    'docs/build',
    '_build-staging',
    '.modularpipelines',
    # Root-level SDK installs and agent scratch space.
    '.dotnet',
    '.tmp'
)

function Test-DisposableWorktreePath {
    [CmdletBinding()]
    param([Parameter(Mandatory)][string]$Path)

    # Quoted porcelain paths require Git's escape decoding. Preserve them rather than
    # risk classifying an unusual source path as generated output.
    if ($Path.StartsWith('"', [System.StringComparison]::Ordinal)) { return $false }

    $normalizedPath = $Path -replace '\\', '/'
    # Keep these root-only patterns aligned with .gitignore. Ignore rules alone
    # cannot authorize deleting arbitrary files: ignored source and secrets survive.
    if (-not $normalizedPath.Contains('/') -and
        ($normalizedPath -cmatch '\.(binlog|log|nettrace)$' -or
         $normalizedPath -cmatch '(^|-)(pr-body|review-disposition|review-validation|rebase-validation|comment|issue)\.md$')) {
        return $true
    }
    foreach ($docsGeneratedDirectory in $script:DisposableWorktreeScopedDirectories) {
        if ($normalizedPath -ceq $docsGeneratedDirectory -or
            $normalizedPath.StartsWith("$docsGeneratedDirectory/", [System.StringComparison]::Ordinal)) {
            return $true
        }
    }

    foreach ($segment in ($normalizedPath -split '/')) {
        if ($script:DisposableWorktreeGeneratedDirectories.ContainsKey($segment)) { return $true }
    }

    return $false
}

function Test-IsLinkedWorktree {
    param([Parameter(Mandatory)][string]$Path)

    # A primary checkout has a .git directory. A linked worktree has a .git file,
    # including when reached through a symlink or junction. Requiring that marker
    # avoids path-text comparisons and refuses arbitrary directories fail-closed.
    $marker = Join-Path $Path '.git'
    if (-not (Test-Path -LiteralPath $marker -PathType Leaf)) { return $false }

    $firstLine = Get-Content -LiteralPath $marker -TotalCount 1 -ErrorAction SilentlyContinue
    return $firstLine -match '^gitdir:\s*\S+'
}

function Test-SameNativePath {
    param(
        [Parameter(Mandatory)][string]$Left,
        [Parameter(Mandatory)][string]$Right
    )

    try {
        $leftPath = [IO.Path]::GetFullPath($Left).TrimEnd([char[]]@('/', '\'))
        $rightPath = [IO.Path]::GetFullPath($Right).TrimEnd([char[]]@('/', '\'))
        $comparison = if ($IsWindows) { [StringComparison]::OrdinalIgnoreCase } else { [StringComparison]::Ordinal }
        return [string]::Equals($leftPath, $rightPath, $comparison)
    }
    catch {
        return $false
    }
}

# `git worktree add` copies the source checkout's config.worktree into the new worktree,
# so an agent.lockName marker counts only in the worktree that agent.lockWorktree names.
function Test-WorktreeLockMarkerOwner {
    param([Parameter(Mandatory)][string]$Worktree)

    $owner = git -C $Worktree config --worktree --get agent.lockWorktree 2>$null
    if ($LASTEXITCODE -notin @(0, 1)) { throw 'Cannot inspect worktree ownership marker.' }
    if ([string]::IsNullOrWhiteSpace($owner)) { return $false }
    $top = git -C $Worktree rev-parse --show-toplevel 2>$null
    if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($top)) { throw "Cannot resolve worktree root: $Worktree" }
    return Test-SameNativePath -Left $owner.Trim() -Right $top
}

function Get-PrNumberFromWorktreePath {
    param([Parameter(Mandatory)][string]$Path)

    $leaf = Split-Path -Path $Path -Leaf
    if ($leaf -match '^pr-(?<Number>\d+)(?:-|$)') {
        return [int]$Matches.Number
    }
    return $null
}

function Test-BranchIdentifiesPrNumber {
    param(
        [Parameter(Mandatory)][string]$Branch,
        [Parameter(Mandatory)][int]$PrNumber
    )

    # Review branches use both codex/pr-123-* and codex-pr-123-* forms. Treat
    # slash and hyphen as token boundaries so canonical path + branch identity
    # still supplies two independent signals for the same merged PR.
    return $Branch -match "(?:^|[-/])pr-$PrNumber(?:$|[-/])"
}

function Test-IsAncestorCommit {
    param(
        [Parameter(Mandatory)][string]$RepoPath,
        [Parameter(Mandatory)][string]$Ancestor,
        [Parameter(Mandatory)][string]$Descendant
    )

    # An object git cannot find locally (a merged head never fetched here) exits 128.
    # For cleanup that is "not an ancestor": it may keep a worktree, never remove one.
    git -C $RepoPath merge-base --is-ancestor $Ancestor $Descendant 2>$null
    return $LASTEXITCODE -eq 0
}

function Get-CommitAncestorPredicate {
    param(
        [Parameter(Mandatory)][string]$RepoPath,
        [Parameter(Mandatory)][string]$Descendant
    )

    # GetNewClosure captures variables, but cannot resolve script-local functions.
    # Retain the implementation explicitly without publishing a global helper.
    $testAncestry = ${function:Test-IsAncestorCommit}
    return {
        param($ancestor)
        & $testAncestry -RepoPath $RepoPath -Ancestor $ancestor -Descendant $Descendant
    }.GetNewClosure()
}

function Get-MergedNameReason {
    param(
        [Parameter(Mandatory)][string]$Branch,
        [AllowEmptyCollection()][string[]]$MergedHeadShas = @(),
        [scriptblock]$IsAncestor = { param($sha) $false }
    )

    # A branch name can belong to several merged PRs. The worktree is one of their
    # leftovers only if its HEAD still contains one of those head commits; a fresh branch
    # that reuses the name after a squash merge is cut from main, which never does.
    foreach ($headSha in $MergedHeadShas) {
        if ($headSha -and (& $IsAncestor $headSha)) {
            return "merged PR head branch '$Branch'"
        }
    }
    return $null
}

function Get-MergedAssociationReason {
    param(
        [AllowEmptyCollection()][object[]]$Associations = @(),
        [string]$Branch,
        [scriptblock]$IsAncestor = { param($sha) $false }
    )

    # GitHub's commits/{sha}/pulls association is merge evidence for a detached
    # snapshot, but a named branch that merely points at an already-merged commit is
    # active work: every branch cut from origin/main sits on the latest squash commit
    # until its first commit. A named branch therefore needs a merged PR whose head
    # branch is this branch (branch names are case-sensitive) and whose head commit the
    # worktree still contains; a branch recreated under its old name right after the
    # squash merge sits on the squash commit, which never contains the old head.
    $merged = @($Associations | Where-Object { $_.merged_at })
    if (-not $Branch) {
        if ($merged.Count -gt 0) { return 'merged PR via commit association' }
        return $null
    }

    $own = $merged |
        Where-Object { $_.head.ref -ceq $Branch -and $_.head.sha -and (& $IsAncestor $_.head.sha) } |
        Select-Object -First 1
    if ($own) { return "merged PR #$($own.number) via commit association for branch '$Branch'" }
    return $null
}

function Test-IsCanonicalPrWorktree {
    param(
        [Parameter(Mandatory)][string]$Path,
        [Parameter(Mandatory)][string]$WorktreeRoot,
        [AllowNull()][string]$Branch,
        [switch]$Detached,
        [Parameter(Mandatory)][int]$PrNumber
    )

    $parent = Split-Path -Path $Path -Parent
    if (-not (Test-SameNativePath -Left $parent -Right $WorktreeRoot)) { return $false }
    if ((Get-PrNumberFromWorktreePath -Path $Path) -ne $PrNumber) { return $false }
    if ($Detached) { return $true }
    return $Branch -and (Test-BranchIdentifiesPrNumber -Branch $Branch -PrNumber $PrNumber)
}

function Test-IsDescendantPath {
    param(
        [Parameter(Mandatory)][string]$Path,
        [Parameter(Mandatory)][string]$Parent
    )

    try {
        $pathValue = [IO.Path]::GetFullPath($Path).TrimEnd([char[]]@('/', '\'))
        $parentValue = [IO.Path]::GetFullPath($Parent).TrimEnd([char[]]@('/', '\'))
        $comparison = if ($IsWindows) { [StringComparison]::OrdinalIgnoreCase } else { [StringComparison]::Ordinal }
        return $pathValue.StartsWith($parentValue + [IO.Path]::DirectorySeparatorChar, $comparison)
    }
    catch {
        return $false
    }
}

function Select-MergeCleanupWorktree {
    param(
        [string]$ValidatedWorktree,
        [string]$CurrentBranchWorktree,
        [switch]$WasExplicit,
        [switch]$IdentityValid
    )

    if (-not $ValidatedWorktree -or -not $IdentityValid) { return $null }
    if ($WasExplicit) { return $ValidatedWorktree }
    if (-not $CurrentBranchWorktree) { return $null }
    if (Test-SameNativePath -Left $ValidatedWorktree -Right $CurrentBranchWorktree) {
        return $ValidatedWorktree
    }
    return $null
}

function Get-WorktreeAgentLockNames {
    param([Parameter(Mandatory)][string]$Worktree, [string]$Branch, [switch]$Orphan)

    $names = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    if (-not $Orphan) {
        $extension = git -C $Worktree config --local --bool --get extensions.worktreeConfig 2>$null
        if ($LASTEXITCODE -notin @(0, 1)) { throw 'Cannot inspect worktree configuration.' }
        if ($extension -eq 'true') {
            $marker = git -C $Worktree config --worktree --get agent.lockName 2>$null
            if ($LASTEXITCODE -notin @(0, 1)) { throw 'Cannot inspect worktree ownership marker.' }
            if (-not [string]::IsNullOrWhiteSpace($marker) -and (Test-WorktreeLockMarkerOwner -Worktree $Worktree)) {
                [void]$names.Add($marker.Trim())
            }
        }
    }

    # The canonical path identifies a detached setup checkout before renew writes its marker.
    $leaf = Split-Path -Path $Worktree -Leaf
    if ($leaf -match '^(?<Lock>(?:pr|issue)-\d+)(?:-|$)') {
        [void]$names.Add($Matches.Lock.ToLowerInvariant())
    }
    foreach ($match in [regex]::Matches($Branch, '(?:^|[-/])((?:pr|issue)-\d+)(?=$|[-/])', 'IgnoreCase')) {
        [void]$names.Add($match.Groups[1].Value.ToLowerInvariant())
    }
    return $names | Sort-Object -CaseSensitive
}

function Invoke-WorktreeAgentLock {
    param(
        [Parameter(Mandatory)][string]$ScriptPath,
        [Parameter(Mandatory)][ValidateSet('status', 'acquire', 'release')][string]$Verb,
        [Parameter(Mandatory)][string]$LockName,
        [Parameter(Mandatory)][string]$OwnerId
    )

    # Acquire prints a token. Never forward it, including on a failed cleanup attempt.
    $output = @(& pwsh -NoProfile -File $ScriptPath $Verb -LockName $LockName -OwnerId $OwnerId 2>&1)
    return [pscustomobject]@{
        ExitCode = $LASTEXITCODE
        Status = if ($Verb -eq 'status') { ($output -join "`n").Trim() } else { '' }
    }
}

function Invoke-WithWorktreeCleanupLocks {
    param(
        [Parameter(Mandatory)][string]$RepoPath,
        [Parameter(Mandatory)][string]$Worktree,
        [string]$Branch = '',
        [Parameter(Mandatory)][scriptblock]$Action,
        [switch]$Orphan,
        [switch]$Preview,
        # Identities recorded before Git metadata was lost, for example by a pending removal.
        [string[]]$LockNames = @()
    )

    try {
        $names = @(@(Get-WorktreeAgentLockNames -Worktree $Worktree -Branch $Branch -Orphan:$Orphan) + $LockNames |
            Where-Object { $_ } | Sort-Object -CaseSensitive -Unique)
    }
    catch {
        Write-Host "Preserving worktree: ownership could not be inspected: $Worktree"
        return
    }
    if ($names.Count -eq 0) {
        Write-Host "Preserving worktree: ownership identity is unavailable: $Worktree"
        return
    }
    $agentLocks = Join-Path $RepoPath 'scripts/AgentLocks.ps1'
    if ($names.Count -gt 0 -and -not (Test-Path -LiteralPath $agentLocks -PathType Leaf)) {
        Write-Host "Preserving worktree: canonical lock script is unavailable: $Worktree"
        return
    }

    $owner = "worktree-cleanup-$([Guid]::NewGuid().ToString('N'))"
    $acquired = [Collections.Generic.List[string]]::new()
    try {
        foreach ($name in $names) {
            $verb = if ($Preview) { 'status' } else { 'acquire' }
            try {
                $result = Invoke-WorktreeAgentLock -ScriptPath $agentLocks -Verb $verb -LockName $name -OwnerId $owner
            }
            catch {
                Write-Host "Preserving worktree: lock $name could not be checked: $Worktree"
                return
            }
            if ($result.ExitCode -ne 0 -or ($Preview -and $result.Status -ne 'FREE')) {
                Write-Host "Preserving worktree: lock $name is held or unavailable: $Worktree"
                return
            }
            if (-not $Preview) { $acquired.Add($name) }
        }

        # Keep reservations through deletion, rather than checking FREE then racing a new owner.
        & $Action
    }
    finally {
        for ($index = $acquired.Count - 1; $index -ge 0; $index--) {
            $name = $acquired[$index]
            try {
                $released = Invoke-WorktreeAgentLock -ScriptPath $agentLocks -Verb release -LockName $name -OwnerId $owner
                if ($released.ExitCode -ne 0) { Write-Host "WARNING: could not release cleanup reservation $name." }
            }
            catch { Write-Host "WARNING: could not release cleanup reservation $name." }
        }
    }
}

function Remove-MergedWorktree {
    [CmdletBinding(SupportsShouldProcess)]
    param(
        [Parameter(Mandatory)][string]$Repo,       # a checkout that is NOT the one being removed (main)
        [Parameter(Mandatory)][string]$Worktree,   # path to remove
        [string]$Label = '',                       # e.g. "#1234" for log lines
        [string[]]$LockNames = @(),                # identities a pending retry must reserve
        # The work item is closed: save uncommitted work and a detached HEAD to retained
        # branches instead of preserving the checkout. Unknown ignored files still preserve it.
        [switch]$RetainWork
    )

    if (-not (Test-Path -LiteralPath $Worktree)) {
        if ($WhatIfPreference) {
            Write-Host "sweep: WOULD prune missing worktree registrations for $Repo"
            return
        }
        if ($PSCmdlet.ShouldProcess($Repo, 'Prune missing worktree registrations')) { git -C $Repo worktree prune }
        return
    }

    # Only linked worktrees are valid deletion targets. This rejects the primary
    # checkout even through a filesystem alias before any git or recursive delete.
    if (-not (Test-IsLinkedWorktree -Path $Worktree)) {
        Write-Host "WARNING: refusing to remove primary checkout or non-linked worktree $Label : $Worktree"
        return
    }

    # Preserve source and unknown ignored files; only known generated output is disposable.
    $status = @(git -C $Worktree status --porcelain=v1 --untracked-files=all --ignored=matching 2>$null)
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Preserving worktree $Label : $Worktree (could not inspect worktree status)"
        return
    }
    $work = @(Get-WorktreeUncommittedWork -Status $status)
    # A stash cannot carry ignored files (secrets, local settings), so they always
    # preserve the checkout.
    if ($work.Count -gt 0 -and (-not $RetainWork -or @($work | Where-Object { $_.StartsWith('!! ') }).Count -gt 0)) {
        Write-Host "Preserving dirty worktree $Label : $Worktree (uncommitted work)"
        foreach ($entry in $work) { Write-Host "  $entry" }
        return
    }

    if ($WhatIfPreference) {
        $retained = if ($work.Count -gt 0) { " (saving $($work.Count) uncommitted entr(ies) to a retained branch)" } else { '' }
        Write-Host "sweep: WOULD remove $Worktree -- $Label$retained"
        return
    }
    if (-not $PSCmdlet.ShouldProcess($Worktree, "Remove merged worktree $Label")) { return }

    if ($RetainWork) {
        # Nothing will come back for a closed item's checkout. Keep everything it holds
        # reachable from a branch instead of keeping the directory.
        $name = @($LockNames | Where-Object { $_ })[0]
        if (-not $name) { $name = Split-Path -Path $Worktree -Leaf }
        try {
            if ($work.Count -gt 0) {
                $saved = Save-WorktreeUncommittedWork -Repo $Repo -Worktree $Worktree -Name $name
                Write-Host "Saved uncommitted work of $Worktree to $saved (restore with: git stash apply $saved)"
                $status = @(git -C $Worktree status --porcelain=v1 --untracked-files=all --ignored=matching 2>$null)
                $statusExit = $LASTEXITCODE
                $work = @(Get-WorktreeUncommittedWork -Status $status)
                if ($statusExit -ne 0 -or $work.Count -gt 0) {
                    Write-Host "Preserving dirty worktree $Label : $Worktree (uncommitted work remains after saving)"
                    foreach ($entry in $work) { Write-Host "  $entry" }
                    return
                }
            }
            Save-DetachedWorktreeHead -Repo $Repo -Worktree $Worktree
        }
        catch {
            Write-Host "Preserving worktree $Label : $Worktree (could not retain its work: $_)"
            return
        }
    }

    $pending = Register-PendingWorktreeRemoval -Repo $Repo -Worktree $Worktree -LockNames $LockNames
    if (Remove-WorktreeDirectory -Repo $Repo -Worktree $Worktree -Label $Label) {
        if ($pending) { Remove-Item -LiteralPath $pending -Force -ErrorAction SilentlyContinue }
        Write-Host "Removed worktree $Label : $Worktree"
    } elseif ($pending) {
        Write-Host "WARNING: could not fully remove $Worktree; Remove-MergedWorktrees.ps1 retries the pending removal"
    } else {
        Write-Host "WARNING: could not fully remove $Worktree"
    }
}

# Returns the porcelain status entries that are not disposable generated output.
function Get-WorktreeUncommittedWork {
    [CmdletBinding()]
    param([Parameter(Mandatory)][AllowEmptyCollection()][AllowEmptyString()][string[]]$Status)

    return @($Status | Where-Object {
        if (-not $_) { return $false }
        if ($_ -notmatch '^(\?\?|!!) ') { return $true }
        return -not (Test-DisposableWorktreePath -Path $_.Substring(3))
    })
}

# Saves the tracked and untracked changes of a worktree as a stash commit on the branch
# `retained-worktrees/<name>-wip-<sha>`, leaving the checkout clean. Restore it anywhere
# with `git stash apply <branch>`. Unknown ignored files are not saved, so a caller that
# re-checks the status still preserves them. Throws when nothing could be saved.
function Save-WorktreeUncommittedWork {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string]$Repo,
        [Parameter(Mandatory)][string]$Worktree,
        [Parameter(Mandatory)][string]$Name
    )

    $before = git -C $Repo rev-parse -q --verify refs/stash 2>$null
    git -C $Worktree stash push --include-untracked --quiet -m "retained worktree $Name" 2>$null | Out-Null
    if ($LASTEXITCODE -ne 0) { throw 'git stash failed' }
    $stash = git -C $Repo rev-parse -q --verify refs/stash 2>$null
    if (-not $stash -or $stash -eq $before) { throw 'git stash saved nothing' }
    $branch = "retained-worktrees/$Name-wip-$($stash.Substring(0, 12))"
    git -C $Repo branch $branch $stash 2>$null | Out-Null
    if ($LASTEXITCODE -ne 0) { throw "could not create $branch; the changes remain in git stash list" }
    # The stash stack is shared by every worktree; drop only the entry created here.
    if ((git -C $Repo rev-parse -q --verify 'stash@{0}' 2>$null) -eq $stash) {
        git -C $Repo stash drop --quiet 'stash@{0}' 2>$null | Out-Null
    }
    return $branch
}

# Keeps a detached HEAD reachable from `retained-worktrees/<SHA>` after Git prunes the
# worktree reflog. Never moves an existing branch. Throws when HEAD cannot be retained.
function Save-DetachedWorktreeHead {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string]$Repo,
        [Parameter(Mandatory)][string]$Worktree
    )

    git -C $Worktree symbolic-ref -q HEAD 2>$null | Out-Null
    if ($LASTEXITCODE -eq 0) { return }
    $head = git -C $Worktree rev-parse HEAD 2>$null
    if ($LASTEXITCODE -ne 0) { throw "Cannot resolve HEAD: $Worktree" }
    $branch = "retained-worktrees/$head"
    $existing = git -C $Repo rev-parse -q --verify "refs/heads/$branch" 2>$null
    if ($LASTEXITCODE -ne 0) {
        git -C $Repo branch $branch $head 2>$null | Out-Null
        if ($LASTEXITCODE -ne 0) { throw "Cannot retain detached HEAD: $Worktree" }
    } elseif ($existing -ne $head) {
        throw "Retention branch has moved: $branch"
    }
}

# Git for Windows follows a directory junction while deleting a worktree: it empties the
# target, then fails with "Directory not empty" on the link and leaves a remnant without
# its .git file. Package managers link node_modules packages with junctions, and a junction
# can also point into another checkout or a shared store. Delete each link itself, never its
# target, before any recursive delete. Returns whether no directory link remains.
function Remove-WorktreeDirectoryLinks {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string]$Worktree,
        [string]$Label = ''
    )

    if (-not $IsWindows) { return $true }

    # Get-ChildItem does not recurse through links, so every match is a link to remove.
    $find = { @(Get-ChildItem -LiteralPath $Worktree -Directory -Recurse -Force -Attributes ReparsePoint -ErrorAction SilentlyContinue) }
    foreach ($link in (& $find)) {
        # DirectoryInfo.Delete() without recursion removes the reparse point only.
        try { $link.Delete() } catch { Write-Host "Could not detach directory link $($link.FullName): $_" }
    }

    $remaining = & $find
    if ($remaining.Count -gt 0) {
        Write-Host "WARNING: worktree $Label kept: $($remaining.Count) directory link(s) could not be detached, first $($remaining[0].FullName): $Worktree"
        return $false
    }
    return $true
}

# Deletes an already-vetted worktree directory and prunes its registration. Returns
# whether the directory is gone. Callers must have checked that its content is disposable.
function Remove-WorktreeDirectory {
    param(
        [Parameter(Mandatory)][string]$Repo,
        [Parameter(Mandatory)][string]$Worktree,
        [string]$Label = ''
    )

    if (-not (Remove-WorktreeDirectoryLinks -Worktree $Worktree -Label $Label)) { return $false }

    # Primary path: let git remove it. Double force also clears a stale setup lock and
    # deletes the remnant of an earlier, partially failed removal.
    git -C $Repo -c core.longpaths=true worktree remove --force --force $Worktree 2>$null | Out-Null

    # Fallback for long-path failures and remnants that Git no longer recognizes.
    if (Test-Path -LiteralPath $Worktree) {
        # A failed git delete can leave links behind, so detach again before recursing.
        if (-not (Remove-WorktreeDirectoryLinks -Worktree $Worktree -Label $Label)) { return $false }
        # \\?\ disables Win32 path normalization, so forward slashes (git's output
        # format) are NOT translated — convert to backslashes or the delete no-ops.
        $removalPath = if ($IsWindows) { '\\?\' + ($Worktree -replace '/', '\') } else { $Worktree }
        Remove-Item -LiteralPath $removalPath -Recurse -Force -ErrorAction SilentlyContinue
    }

    git -C $Repo worktree prune 2>$null | Out-Null
    return -not (Test-Path -LiteralPath $Worktree)
}

# AGENT EVIDENCE
# Agents keep logs, reports and PR-body drafts for a work item under
# <main checkout>-evidence/<lock name>/, beside the worktree root, so nothing lands
# loose in the parent directory. Evidence matters only while its item is open.
function Get-AgentEvidenceRoot {
    [CmdletBinding()]
    param([Parameter(Mandatory)][string]$MainCheckout)

    return "$(($MainCheckout -replace '\\', '/').TrimEnd('/'))-evidence"
}

# Removes each top-level entry of the evidence root whose newest write is older than
# the retention window. Returns the number of entries removed (or that would be).
function Remove-StaleAgentEvidence {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string]$Root,
        [Parameter(Mandatory)][int]$RetentionDays,
        # Entries already handled this round, so a dry run does not count them twice.
        [AllowEmptyCollection()][string[]]$Skip = @(),
        [switch]$WhatIf
    )

    if (-not (Test-Path -LiteralPath $Root -PathType Container)) { return 0 }
    $cutoff = [DateTime]::UtcNow.AddDays(-$RetentionDays)
    $removed = 0
    foreach ($entry in (Get-ChildItem -LiteralPath $Root -Force -ErrorAction SilentlyContinue)) {
        if ($entry.Name -cin $Skip) { continue }
        if (($entry.Attributes -band [IO.FileAttributes]::ReparsePoint) -ne 0) {
            Write-Host "sweep: skipping evidence entry that is a link: $($entry.FullName)"
            continue
        }
        $items = @($entry)
        if ($entry.PSIsContainer) {
            $items += @(Get-ChildItem -LiteralPath $entry.FullName -Recurse -Force -ErrorAction SilentlyContinue)
        }
        $newest = ($items | Measure-Object -Property LastWriteTimeUtc -Maximum).Maximum
        if ($newest -ge $cutoff) { continue }
        if (Remove-AgentEvidenceEntry -Entry $entry -Kind 'stale' -WhatIf:$WhatIf) { $removed++ }
    }
    return $removed
}

# Removes the evidence of finished work items: each `pr-<N>` or `issue-<N>` folder whose
# item is closed (merged or not) and that no registered worktree still uses. Evidence of
# an open item survives release, because the merge gate cites archives across units.
# A temporary reservation on the item is held through deletion, so a new owner cannot
# start writing midway; a held or unavailable lock, or an unreadable state, keeps the
# evidence. Returns the names of the entries removed (or that would be).
function Remove-FinishedAgentEvidence {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string]$Root,
        [Parameter(Mandatory)][string]$AgentLocks,
        # Receives the lock name; returns $true only when the item is confirmed closed.
        [Parameter(Mandatory)][scriptblock]$IsClosed,
        [AllowEmptyCollection()][string[]]$ActiveLockNames = @(),
        [switch]$WhatIf
    )

    $removed = [Collections.Generic.List[string]]::new()
    if (-not (Test-Path -LiteralPath $Root -PathType Container)) { return ,$removed.ToArray() }
    if (-not (Test-Path -LiteralPath $AgentLocks -PathType Leaf)) { return ,$removed.ToArray() }
    $active = New-OrdinalStringMap
    foreach ($name in $ActiveLockNames) { if ($name) { $active[$name] = $true } }

    foreach ($entry in (Get-ChildItem -LiteralPath $Root -Directory -Force -ErrorAction SilentlyContinue)) {
        $name = $entry.Name
        if ($name -cnotmatch '^(pr|issue)-\d+$' -or $active.ContainsKey($name)) { continue }
        try { if (-not (& $IsClosed $name)) { continue } }
        catch {
            Write-Host "sweep: could not read the state of $name; keeping its evidence"
            continue
        }

        $owner = "evidence-cleanup-$([Guid]::NewGuid().ToString('N'))"
        $verb = if ($WhatIf) { 'status' } else { 'acquire' }
        try { $lease = Invoke-WorktreeAgentLock -ScriptPath $AgentLocks -Verb $verb -LockName $name -OwnerId $owner }
        catch { $lease = [pscustomobject]@{ ExitCode = 1; Status = '' } }
        if ($lease.ExitCode -ne 0 -or ($WhatIf -and $lease.Status -ne 'FREE')) {
            Write-Host "sweep: keeping evidence of $name (lock held or unavailable)"
            continue
        }
        try {
            if (Remove-AgentEvidenceEntry -Entry $entry -Kind 'finished' -WhatIf:$WhatIf) { $removed.Add($name) }
        }
        finally {
            if (-not $WhatIf) {
                try {
                    $released = Invoke-WorktreeAgentLock -ScriptPath $AgentLocks -Verb release -LockName $name -OwnerId $owner
                    if ($released.ExitCode -ne 0) { Write-Host "WARNING: could not release evidence cleanup reservation $name." }
                }
                catch { Write-Host "WARNING: could not release evidence cleanup reservation $name." }
            }
        }
    }
    return ,$removed.ToArray()
}

# Deletes one top-level evidence entry without following links. Returns whether it is
# gone (or, with -WhatIf, would be removed).
function Remove-AgentEvidenceEntry {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][IO.FileSystemInfo]$Entry,
        [Parameter(Mandatory)][string]$Kind,
        [switch]$WhatIf
    )

    if (($Entry.Attributes -band [IO.FileAttributes]::ReparsePoint) -ne 0) {
        Write-Host "sweep: skipping evidence entry that is a link: $($Entry.FullName)"
        return $false
    }
    if ($WhatIf) {
        Write-Host "sweep: WOULD remove $Kind evidence $($Entry.FullName)"
        return $true
    }
    if ($Entry -is [IO.DirectoryInfo] -and -not (Remove-WorktreeDirectoryLinks -Worktree $Entry.FullName -Label '(evidence)')) { return $false }
    Remove-Item -LiteralPath $Entry.FullName -Recurse -Force -ErrorAction SilentlyContinue
    if (Test-Path -LiteralPath $Entry.FullName) {
        Write-Host "sweep: WARNING could not remove $Kind evidence $($Entry.FullName)"
        return $false
    }
    Write-Host "sweep: removed $Kind evidence $($Entry.FullName)"
    return $true
}

# PENDING REMOVALS
# On Windows a directory cannot be deleted while any process has its current directory
# inside it, and build or test processes can hold files open briefly. Git then deletes
# part of the tree, including the .git file, and fails. The remnant no longer looks like
# a worktree, so neither release nor the merged-worktree sweep would recognize it again.
# Before deleting a vetted checkout, cleanup records it under the common Git directory.
# The record is removed after success, and Complete-PendingWorktreeRemovals retries the
# rest once the blocking process has exited.

function Get-PendingWorktreeRemovalDirectory {
    param([Parameter(Mandatory)][string]$Repo)

    $common = git -C $Repo rev-parse --path-format=absolute --git-common-dir 2>$null
    if ($LASTEXITCODE -ne 0 -or -not $common) { return $null }
    return Join-Path $common.Trim() 'agent-pending-removals'
}

function Register-PendingWorktreeRemoval {
    param(
        [Parameter(Mandatory)][string]$Repo,
        [Parameter(Mandatory)][string]$Worktree,
        [string[]]$LockNames = @()
    )

    $directory = Get-PendingWorktreeRemovalDirectory -Repo $Repo
    if (-not $directory) { return $null }
    $path = [IO.Path]::GetFullPath($Worktree).TrimEnd([char[]]@('/', '\'))
    $gitDirectory = git -C $path rev-parse --absolute-git-dir 2>$null
    if ($LASTEXITCODE -ne 0 -or -not $gitDirectory) { return $null }
    $branch = git -C $path symbolic-ref -q --short HEAD 2>$null
    try { $names = @(Get-WorktreeAgentLockNames -Worktree $path -Branch ([string]$branch)) + $LockNames }
    catch { $names = $LockNames }

    $key = if ($IsWindows) { $path.ToLowerInvariant() } else { $path }
    $hash = [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData([Text.Encoding]::UTF8.GetBytes($key)))
    $record = Join-Path $directory ($hash.Substring(0, 16).ToLowerInvariant() + '.json')
    New-Item -ItemType Directory -Force -Path $directory | Out-Null
    [ordered]@{
        path         = $path
        gitDirectory = [IO.Path]::GetFullPath($gitDirectory.Trim())
        lockNames    = @($names | Where-Object { $_ } | Sort-Object -CaseSensitive -Unique)
        recordedAt   = [DateTimeOffset]::UtcNow.ToString('o')
    } | ConvertTo-Json -Compress | Set-Content -LiteralPath $record -NoNewline
    return $record
}

function Test-HasWorkNewerThan {
    param(
        [Parameter(Mandatory)][string]$Path,
        [Parameter(Mandatory)][DateTimeOffset]$Cutoff
    )

    foreach ($file in (Get-ChildItem -LiteralPath $Path -File -Recurse -Force -ErrorAction SilentlyContinue)) {
        $relative = [IO.Path]::GetRelativePath($Path, $file.FullName)
        if ($relative -eq '.git' -or (Test-DisposableWorktreePath -Path $relative)) { continue }
        if ($file.LastWriteTimeUtc -gt $Cutoff.UtcDateTime) { return $true }
    }
    return $false
}

function Complete-PendingWorktreeRemovals {
    param(
        [Parameter(Mandatory)][string]$Repo,
        [switch]$WhatIf
    )

    $directory = Get-PendingWorktreeRemovalDirectory -Repo $Repo
    if (-not $directory -or -not (Test-Path -LiteralPath $directory)) { return 0 }
    $completed = [ref]0
    foreach ($recordFile in (Get-ChildItem -LiteralPath $directory -File -Filter '*.json')) {
        try {
            $record = Get-Content -LiteralPath $recordFile.FullName -Raw | ConvertFrom-Json
            $path = [string]$record.path
            $recordedAt = [DateTimeOffset]::Parse([string]$record.recordedAt, [Globalization.CultureInfo]::InvariantCulture)
        }
        catch {
            Write-Host "sweep: WARNING unreadable pending removal record: $($recordFile.FullName)"
            continue
        }
        $drop = { if (-not $WhatIf) { Remove-Item -LiteralPath $recordFile.FullName -Force -ErrorAction SilentlyContinue } }

        if (-not $path -or -not (Test-Path -LiteralPath $path)) { & $drop; continue }

        # A new checkout at the same path has its own Git metadata, created after the record.
        $marker = Join-Path $path '.git'
        if (Test-Path -LiteralPath $marker -PathType Leaf) {
            $gitDirectory = ((Get-Content -LiteralPath $marker -TotalCount 1) -replace '^gitdir:\s*', '').Trim()
            if ($gitDirectory -and -not [IO.Path]::IsPathRooted($gitDirectory)) { $gitDirectory = Join-Path $path $gitDirectory }
            if ($gitDirectory -and (Test-Path -LiteralPath $gitDirectory) -and (
                    -not (Test-SameNativePath -Left $gitDirectory -Right ([string]$record.gitDirectory)) -or
                    (Get-Item -LiteralPath $gitDirectory -Force).CreationTimeUtc -gt $recordedAt.UtcDateTime)) {
                Write-Host "sweep: dropping pending removal superseded by a new checkout: $path"
                & $drop
                continue
            }
        }
        elseif (Test-Path -LiteralPath $marker) {
            Write-Host "sweep: dropping pending removal for a standalone repository: $path"
            & $drop
            continue
        }

        # The checkout was vetted before deletion began. Anything written since is new work.
        if (Test-HasWorkNewerThan -Path $path -Cutoff $recordedAt) {
            Write-Host "sweep: WARNING preserving pending removal with files changed after removal began; inspect manually: $path"
            & $drop
            continue
        }

        $null = Invoke-WithWorktreeCleanupLocks -RepoPath $Repo -Worktree $path -Orphan -LockNames @($record.lockNames) -Preview:$WhatIf -Action {
            if ($WhatIf) {
                Write-Host "sweep: WOULD finish pending removal of $path"
                return
            }
            if (Remove-WorktreeDirectory -Repo $Repo -Worktree $path -Label '(pending removal)') {
                & $drop
                $completed.Value++
                Write-Host "sweep: finished pending removal of $path"
            } else {
                Write-Host "sweep: WARNING pending removal is still blocked, retrying next sweep: $path"
            }
        }
    }
    return $completed.Value
}
