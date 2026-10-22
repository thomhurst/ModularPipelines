# Local worktree lifecycle

`AgentLocks.ps1 release` removes its recorded checkout before releasing ownership. Register the path once after checkout with `renew -LockName <name> -Worktree <absolute-path>`. Use the shared checkout's current script, and release in `finally` after stopping owned processes and saving evidence under the evidence root ([where agent output belongs](../.claude/skills/issue-pr-loop/SKILL.md#isolation-and-ownership)).

Local branches remain available for `git worktree add <path> <branch>`. Detached commits are retained as `retained-worktrees/<SHA>`. Uncommitted source, unknown ignored files, Git-locked worktrees, the main checkout and foreign repositories are preserved. Known generated output is disposable under `WorktreeCleanup.ps1`. Release prints why a checkout was retained.

Run release with the shared checkout as the working directory. On Windows a process whose current directory is inside the worktree blocks deletion, and so can a build or test process that still holds a file. Git then deletes most of the tree, including its `.git` file, and fails. Cleanup therefore records each vetted checkout under `<git-common-dir>/agent-pending-removals` before deleting it, and removes the record on success. Each `Remove-MergedWorktrees.ps1` sweep retries recorded removals under the same Redis reservations. It drops a record when a new checkout reuses the path, and preserves the directory when files outside known generated output changed after removal began.

A crashed process or an expired Redis TTL cannot run release. `Remove-MergedWorktrees.ps1` remains the conservative fallback for merged work. It also unlocks a `PR checkout setup` or `Issue checkout setup` Git lock that is older than the two-hour Redis TTL. No GitHub Actions job manages local disk.

The sweep reserves canonical Redis item locks before deleting an eligible worktree. It checks identities from the worktree marker, canonical directory name, and branch name. A marker counts only in the worktree its `agent.lockWorktree` path names, because `git worktree add` copies the source checkout's markers. Active or unverifiable ownership preserves the checkout. Reservations use the shared checkout's `AgentLocks.ps1` and remain held through removal. `-WhatIf` only reads lock status. These checks do not replace the existing merge-evidence and source-preservation guards.

The same reservation protects orphan directories with dangling or missing Git metadata. Their canonical directory names supply the lock identity; an orphan without a recoverable identity is preserved for manual inspection.

Create detached PR setup checkouts with `git worktree add --detach --lock --reason 'PR checkout setup' ...`. Wrap `gh pr checkout` and lock-path registration in `try/finally`, unlocking that setup's temporary Git lock in `finally` before releasing Redis ownership, even if either setup step fails. Report an unlock failure and retain the path for recovery. This temporary Git lock protects setup from older cleanup scripts; Redis remains the ownership authority.

Run regression tests locally with PowerShell 7, Git and Docker:

```powershell
pwsh scripts/Test-ReleasedWorktreeCleanup.ps1
pwsh scripts/Test-MergedWorktreeOwnership.ps1
```
