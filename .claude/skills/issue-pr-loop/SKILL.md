---
name: issue-pr-loop
description: "Use for /issue-pr-loop or requests to autonomously work the GitHub queue: maintain and merge PRs, then claim issues and ship fixes until stopped or no actionable work remains."
---

# Issue/PR Loop

Follow [CLAUDE.md](../../../CLAUDE.md) for build limits, tests, formatting, and generated-code constraints.

## Loop and completion

Run unattended until the user stops/pauses you or no actionable work remains. Defer ambiguous or unsafe items with a concise GitHub comment or issue; release their locks and continue without waiting for user input.

At each iteration, run `pwsh scripts/Remove-MergedWorktrees.ps1`, survey open PRs, and complete one unit in this order:

1. Merge an eligible PR.
2. Fix conflicts, CI failures, or review feedback on an open PR and push.
3. Recover valuable tracked changes from a dirty merged-PR worktree into a follow-up PR.
4. Claim an available issue, implement its full scope, and open a PR.

After pushing or deferring an item, start the next iteration. Pending CI/review is a reason to work another item. Do not watch, poll, sleep, or schedule monitors while other work is queueable. Before stopping, survey PRs, dirty recovery candidates, and unclaimed issues again.

Code work completes only after validation, commit, successful `git push --force-with-lease`, and confirmation that the PR's `headRefOid` matches local `git rev-parse HEAD`. If pushing fails, report failure and the blocker; never report unpushed work as complete. Keep progress in commentary while the loop is active.

Use non-interactive commits (`-m`, `--file`, or `--amend --no-edit`) and rebases (`git -c core.editor=true -c sequence.editor=true rebase ...`). Never use bare `--force`.

Only dispatch iterations to subagents when the user explicitly authorizes them. Each subagent follows this skill, completes one unit, and returns the PR number and outcome; the parent continues the loop.

## Ownership and worktrees

Use the shared checkout only for queue inspection and worktree creation. All edits, branch checkouts, rebases, validation, commits, and pushes run in an isolated worktree.

Capture these paths from the shared checkout:

```powershell
$repo = git rev-parse --show-toplevel
$agentLocks = Join-Path $repo 'scripts/AgentLocks.ps1'
$worktreeRoot = Join-Path (Split-Path $repo -Parent) 'ModularPipelines-worktrees'
```

Use `C:\tmp\ModularPipelines-worktrees` if the sibling directory is unavailable. If a worktree path exists, choose a unique path; existing checkout state does not prove ownership.

Acquire a Redis lock through the absolute shared-checkout `$agentLocks` path before acting on any item: `pwsh $agentLocks acquire -LockName $lockName`. Use `pr-<N>` for PR work and dirty recovery, or `issue-<N>` for new issues. Exit `0` grants ownership; `3` means held; other errors mean skip. Always use this same script path: old worktrees may have incompatible token-cache formats.

- Redis is the ownership authority. Never steal locks based on PIDs, local files, or apparent inactivity.
- Locks expire after two hours; no periodic heartbeat is needed. Renew only for work approaching expiry or to record a worktree: `pwsh $agentLocks renew -LockName $lockName -Worktree $worktree`. Exit `4` means ownership lost: stop work and do not push.
- Codex uses `CODEX_THREAD_ID`. Other automation needs one stable, unique `MODULARPIPELINES_AGENT_LOCK_OWNER_ID` or `-OwnerId` across commands. The script privately caches tokens; do not print or manage them yourself.
- Release in cleanup for every outcome: `pwsh $agentLocks release -LockName $lockName`. Exit `5` means stale ownership; do not delete or overwrite another owner's key.
- Only stop services/containers started for this worktree. Never remove the shared `modularpipelines-agent-locks-redis` container.

After claiming a new issue, fetch `origin/main` and create branch/worktree `issue-<N>-<short-desc>` from it. For PR fixes, create a detached worktree from `origin/main`, then run `gh pr checkout <N>` there.

Name PR worktree directories `pr-<N>-<description>`; retain that identity and never reuse them for another PR. If using a separate local review branch, include the same `pr-<N>` identity in its name so cleanup can recognize it.

Set every tool call's `workdir` explicitly; `Set-Location` does not persist across calls. Before the first mutable operation, verify the checkout from that workdir:

```powershell
$actualRoot = [System.IO.Path]::GetFullPath((git rev-parse --show-toplevel))
$expectedRoot = [System.IO.Path]::GetFullPath($worktree)
if (-not [string]::Equals($actualRoot, $expectedRoot, [System.StringComparison]::OrdinalIgnoreCase)) {
    throw "Refusing to edit outside isolated worktree: $actualRoot"
}
git status --short --branch
```

Let the merge and cleanup scripts remove merged worktrees. Manually remove an abandoned issue worktree only when it has no uncommitted work.

## Maintain PRs

Survey with `gh pr list --author @me --state open --json number,title,headRefName,mergeable,mergeStateStatus,reviewDecision,statusCheckRollup,isDraft`.

For each PR, inspect all review surfaces, including paginated results: REST `pulls/<N>/reviews`, `pulls/<N>/comments`, `issues/<N>/comments`, and GraphQL `reviewThreads`. Review bodies can contain blocking findings even with state `COMMENTED` and zero unresolved threads. Treat uncertain concerns as blocking; empty or boilerplate acknowledgments are not findings.

| State | Action |
| --- | --- |
| Conflicts | Rebase on fresh `origin/main` with editors disabled; inspect the diff for dropped changes, validate, and push. Abort and defer conflicts you cannot resolve confidently. |
| CI failure | Inspect failed logs and fix the cause. Rerun only a confirmed infrastructure failure or flake, at most once; investigate flaky tests rather than repeatedly rerunning for green. |
| Feedback | Address useful minor and blocking suggestions; reply to each thread and blocking review body with the fix or technical disposition. Push back once if warranted; implement if reaffirmed. |
| Pending CI/review | Move to another item. A fix push needs a subsequent bot review/CI cycle before merge. |
| Merge candidate | Verify all conditions below, then use the merge wrapper. |

Resolve a bot-opened thread under the PR lock only when you replied with a fix commit or concrete disposition, the current head contains any promised fix, and a subsequent bot review/CI cycle completed without rebuttal. A current-head `REVIEW_VERDICT: CLEAR` is strongest evidence. Do not resolve unaddressed findings, newer rebuttals, or human threads awaiting a response. Use GraphQL `resolveReviewThread`, then re-fetch review and check state.

Merge requires: OPEN, `MERGEABLE`, `CLEAN`, every check terminal and passing (`SUCCESS`/`SKIPPED`/`NEUTRAL`), no unresolved threads or unaddressed comments/review concerns, approval when required, and a bot cycle since the last fix push. Check repository approval requirements; recent merged PRs can provide supporting evidence.

Merge only through `pwsh scripts/Merge-Pr.ps1 -Pr <N> -Worktree $worktree`; omit `-Worktree` only when none exists. It re-fetches state through `Assert-PrGreen.ps1`, squash-merges, and cleans up the validated worktree and branches. You still assess review dispositions, approval, and the bot cycle.

Never bypass a denied gate with direct `gh pr merge` or `--auto`. Nonzero exit means defer. Successful merge with cleanup warnings is still merged; do not retry it. The cleanup scripts preserve dirty, locked, open-PR, and harness-managed worktrees; a missing remote branch alone does not prove a merge.

## Recover dirty merged worktrees

Treat `Preserving dirty worktree` output as a recovery candidate after actionable PRs:

1. Verify the original merged PR identity and acquire `pr-<original-N>`. If either fails, leave the worktree untouched.
2. Compare the full tracked diff with the original PR and fresh `origin/main`. Recover coherent, unique, valuable changes. Remove obsolete or generated churn only after proving no unique source/test change would be lost.
3. Preserve mixed, unclear, sensitive, or untestable changes. Search for an existing recovery issue before filing one with the path, branch, original PR, and non-sensitive summary.
4. Before any destructive operation, snapshot the exact edits on a local branch/commit. Transplant only that diff into a separate worktree from current `origin/main`; publishing the old squash-merged branch may replay merged commits.
5. Remove already-merged changes, split unrelated edits, and regenerate options from generator/source changes. Validate and simplify; publish only an explainable semantic diff.
6. Open a ready-for-review PR with `Follow-up to #<original-N>`, explaining recovered intent. Confirm the remote head before removing the old worktree and snapshot branch. Release the lock in every outcome.

## Pick up issues

Ensure `in-progress` exists, then list candidates with `gh issue list --state open --search 'no:assignee -label:in-progress' --json number,title,labels,body,comments --limit 50`; paginate when needed.

Prefer clear, small, unblocked issues. Skip assigned, claimed, externally blocked, `wontfix`, `duplicate`, or `question` items. Size alone is not a blocker: take a coherent feature whole, including missing APIs needed to satisfy the issue.

Under the issue lock, re-fetch state/assignees/labels, add `in-progress`, then verify it is open, unassigned, and claimed before branching. Keep the label while its PR is open; remove it if abandoning the issue before opening a PR.

If an issue contains multiple independent deliverables too large for one PR, inspect existing children (including closed ones) before splitting. Use native GitHub sub-issues and `blocked_by` dependencies, keep the parent open, remove its claim, and implement the first unclaimed child whose prerequisites are closed. Native relationship writes need the numeric REST issue `.id`, not the GraphQL node ID or issue number.

Implement the complete acceptance criteria. Use TDD where feasible, documenting exceptions; run relevant validation and formatting from `CLAUDE.md`, plus broader tests where affected. After 2–3 unsuccessful local debugging iterations on one test, push with the limitation documented for CI. Report unavailable Docker or guard resource limits in the PR.

Run `/simplify` before code PRs (or an equivalent simplification review if unavailable), respecting generated-code boundaries. Commit referencing `#<N>` and open a ready-for-review PR with `Closes #<N>` on its own line, using `--body-file` for multiline text. Only use `Closes` when the full scope is delivered; link any separately worked dependency.

If an unrelated bug needs cross-cutting investigation, file and link a separate issue, then finish the original scope. For other blockers, document the reason, release ownership, and advance.
