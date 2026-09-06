---
name: issue-pr-loop
description: "Run the GitHub issue/PR queue when the user invokes /issue-pr-loop or requests continuous queue work: maintain PRs, claim issues, implement in isolated worktrees, and ship until stopped."
---

# Issue/PR Loop

Read the root agent instructions and [repository guidance](references/repository.md). Keep this workflow identical across repositories; local requirements belong in that reference or their maintained source. Reviewing or editing the skill does not start the loop. Stay within the user's authorized scope.

## Loop and completion

Run unattended. At each iteration, run `pwsh scripts/Remove-MergedWorktrees.ps1` from the shared checkout, survey the queue, then complete one unit in priority order:

1. Merge an eligible PR.
2. Fix conflicts, failed CI, or review findings on an open PR and push.
3. Recover valuable changes from a preserved merged-PR worktree, following [recovery guidance](references/recovery.md).
4. Claim an available issue, implement its full scope, and open a PR.

After pushing or deferring, survey again. Pending CI/review means work another item; do not watch, sleep, poll one item, or schedule monitors while work is queueable. When only CI/review remains, periodically survey the whole queue and stay responsive.

Stop on the user's stop/pause, or when a fresh survey finds no queueable issue, recovery candidate, or actionable PR and remaining work requires an external decision/dependency. Pending CI alone is not completion. Defer unsafe/blocked items, record the blocker once when authorized, release ownership, and continue without blocking questions. Revisit only when evidence changes.

Use commentary while active. Preserve item IDs, worktree, lock identity/script, owned services, and unresolved validation across compaction.

Delegate only when the user explicitly authorizes subagents: one item/iteration, this skill, shared checkout, and canonical lock script. Require a confirmed `merged #N`, `pushed fixes to #N`, `opened #N closing #M`, or `failed #N: reason`; then continue surveying.

## Isolation and ownership

Use the shared checkout for surveys/setup. All implementation, validation, checkouts, commits, and pushes run in an isolated worktree. Capture from the shared checkout:

```powershell
$repo = git rev-parse --show-toplevel
$repoSlug = gh repo view --json nameWithOwner --jq .nameWithOwner
$repoName = ($repoSlug -split '/')[-1]
$agentLocks = Join-Path $repo 'scripts/AgentLocks.ps1'
$worktreeRoot = Join-Path (Split-Path $repo -Parent) ($repoName + '-worktrees')
```

Verify the repository and intended base; examples use `origin/main`. Use a writable temporary `<repoName>-worktrees` root if needed. Choose a unique worktree path when one already exists.

Acquire the Redis lock before acting on an item: `pr-<N>` for PR work or recovery, `issue-<N>` for new implementation. Always invoke the absolute shared-checkout `$agentLocks` path for every verb; older branch copies can have incompatible token caches.

| Command | Result |
| --- | --- |
| `pwsh $agentLocks acquire -LockName $lockName` | Exit 0 grants ownership; 3 means held, skip; other failures mean defer, never bypass Redis. |
| `pwsh $agentLocks renew -LockName $lockName` | Renew work approaching the two-hour TTL; exit 4 means ownership lost, stop this item and do not push. |
| `pwsh $agentLocks status -LockName $lockName` | Read-only `FREE` / `HELD` / `HELD-BY-ME`; verify ownership before pushing or merging. |
| `pwsh $agentLocks release -LockName $lockName` | Release in cleanup/finally; exit 0 confirms release, 5 means stale ownership, leave the key alone. |

Redis is authoritative: never steal locks based on PIDs/files/inactivity or add another backend. Do not print/manage cached tokens. Codex supplies `CODEX_THREAD_ID`; other automation needs the same stable unique `-OwnerId` on every verb. Renew when needed, without periodic heartbeats. Optional `renew -Worktree $worktree` records metadata; keep explicit lock names. The issue's `in-progress` label persists independently.

After claiming an issue, create branch/worktree `issue-<N>-<short-desc>` from freshly fetched `origin/main`. For PR fixes, create a detached worktree from `origin/main`, then run `gh pr checkout <N>` with that worktree as `workdir`. PR directories use `pr-<N>-<description>`; never rename or reuse them for another PR. A separate local review/rebase branch must retain the same `pr-<N>` identity.

Set every mutating tool call's `workdir` explicitly; shell directory changes do not persist. Before the first edit and after checkout changes, compare `git rev-parse --show-toplevel` with the intended worktree's resolved absolute path and inspect `git status --short --branch`. Abort on a mismatch.

Use repository scripts for merged-worktree cleanup; `[gone]` or ancestry alone is insufficient. Preserve dirty, locked, open-PR, harness-managed, and locally divergent worktrees. Inspect untracked source before cleanup: scripts may clear build artifacts. Manually remove abandoned worktrees only after verifying repository/path, ownership, and absence of valuable unpublished work. Do not enable stale/scratch cleanup during surveys.

Stop only services/processes/containers started for this worktree. Never blanket-delete containers/volumes or stop shared lock Redis. Follow local lifecycle guidance; use Aspire only where an AppHost exists.

## Maintain PRs

Survey with `gh pr list --author @me --state open --json number,title,headRefName,headRefOid,mergeable,mergeStateStatus,reviewDecision,statusCheckRollup,isDraft`. Include other authors only when the user's queue scope includes them. Paginate or increase limits so an incomplete first page cannot make the queue appear empty.

Inspect all review surfaces, including pagination and replies: REST `pulls/<N>/reviews`, `pulls/<N>/comments`, `issues/<N>/comments` under `repos/{owner}/{repo}`, and GraphQL `reviewThreads` with IDs, resolution state, bodies, authors, and timestamps.

A `COMMENTED` review body can block with zero unresolved threads. Address outstanding concerns, `CHANGES_REQUESTED`, current inline findings, and unresolved threads. Empty acknowledgments are non-blocking. Assess older findings against the current head and later discussion; uncertain concerns block merging.

| State | Action |
| --- | --- |
| Conflicts | Rebase onto fresh `origin/main`, inspect the diff for lost changes at shared insertion points, validate, and push. Abort and defer conflicts that cannot be resolved confidently. |
| CI failure | Read failed logs and fix the cause. Investigate timing/synchronization failures. At most one rerun for a demonstrated infrastructure problem or diagnosed flake when local rules permit; never rerun for green, skip/delete failing tests, or mask a failure. |
| Feedback | Verify the finding against code, address useful minor and blocking suggestions, and reply to each finding with the fix or technical disposition. Discuss disagreement once; implement reaffirmed feedback unless it violates repository contracts or performance requirements, in which case document and defer the conflict. |
| Pending | Take another item. After a fix push, allow a subsequent bot review/CI cycle before considering merge in a later iteration. |
| Merge candidate | Confirm every condition below, then invoke the wrapper. |

Resolve a bot-opened thread under the PR lock only after replying with a fix commit or concrete disposition, confirming any promised fix is in the current head, and a subsequent bot review/CI cycle completes without rebuttal. A current-head `REVIEW_VERDICT: CLEAR` is strongest evidence. Do not resolve newer rebuttals, unaddressed findings, or human threads awaiting response. Use GraphQL `resolveReviewThread`, then re-fetch reviews, threads, checks, and head SHA.

### Merge gate

Merge requires an open, ready-for-review PR; `MERGEABLE` and `CLEAN`; every applicable check present, terminal, and passing (`SUCCESS`, `SKIPPED`, or `NEUTRAL`, with successful legacy commit statuses); no unresolved threads, unaddressed comments, or outstanding review-body concerns; approval when required; and a bot review/CI cycle after the last fix push. Missing expected checks and nonterminal checks block merging. Reassess if the head changes.

Use current repository rules/instructions for approval requirements. Historical merges without approval do not prove approval is optional; inaccessible protection APIs do not prove no protection exists.

```powershell
pwsh scripts/Merge-Pr.ps1 -Pr <N> -Worktree $worktree
```

Omit `-Worktree` only when none exists. The wrapper calls `Assert-PrGreen.ps1`, merges on a passing mechanical gate, and cleans up. Judge review dispositions and approval yourself; the script is necessary but insufficient.

Never bypass with direct `gh pr merge` or `--auto`. Nonzero exit means inspect/defer, not retry blindly. Cleanup warnings after a successful merge do not justify retrying; reconcile ambiguous results with GitHub state. Never delete fork branches through the base repository's `origin`.

## Pick an issue

Ensure `in-progress` exists, then list open, unassigned candidates without that label. Skip `wontfix`, `duplicate`, `question`, claimed items, and external blockers. Prefer clear, smaller tasks with fewer discussion comments; size alone is not a reason to stop when larger coherent work remains.

Under the issue lock, re-fetch state, assignees, labels, native dependencies, and linked PRs. Skip closed, assigned, already claimed, blocked, or already implemented items. Add `in-progress` and verify the claim before branching. Keep it while the PR is open. Remove only your own claim if abandoning before PR creation; after lost ownership, leave shared claim state untouched and report the loss.

Establish full scope from source, tests, and docs; a missing API may be the requested work. Deliver coherent issues in one PR.

For independent deliverables, inspect existing children (including closed ones) before splitting into native sub-issues and `blocked_by` relationships. Avoid checklist-only tracking and `blocked` labels. Give children acceptance criteria, leave the parent open, remove your parent claim, and implement the first unclaimed child whose prerequisites are closed this iteration. If native links are unavailable, document the blocker.

Relationship writes require numeric REST issue `.id`, not issue numbers or `gh issue view --json id` (GraphQL ID). Retrieve with `gh api "repos/$repoSlug/issues/$childNumber" --jq .id`; link via `POST issues/<parent>/sub_issues -F sub_issue_id=<id>` or `POST issues/<blocked>/dependencies/blocked_by -F issue_id=<prerequisite-id>` under `repos/$repoSlug/`. Verify each write before proceeding.

## Implement and ship

Follow local validation, resource limits, target frameworks, generated-code boundaries, and performance requirements. Use TDD for bugs/behavior changes where feasible; explain exceptions. Run focused and relevant broader checks, including docs/frontend checks when affected. Document unavailable validation; never silently raise guard limits or claim unrun tests passed.

Review the full diff for correctness, reuse, and efficiency (`/simplify` when available, otherwise equivalent review). Preserve performance/contracts and batch related fixes into one push. Use CI for environment-specific validation with limitations documented; avoid arbitrary local debugging retry counts.

- Use non-interactive commit messages (`-m`, `--file`, or `--amend --no-edit`) and disable editors for rebases and continuation: `git -c core.editor=true -c sequence.editor=true rebase ...`.
- Reference the issue in commits. Create normal ready-for-review PRs unless the user explicitly requests a draft. Follow the repository PR template and use `--body-file` for multiline descriptions. Put `Closes #<N>` on its own line only when the full issue is delivered.
- Use ordinary pushes for new commits and `git push --force-with-lease` when rewriting an owned PR branch. Never use bare `--force` or rewrite main. Confirm ownership and the remote PR head before reporting completion.
- Completion requires commit, successful push, PR creation/update, and `gh pr view <N> --json headRefOid` matching local `git rev-parse HEAD`. Push/ownership failure means preserve work, report failure, release ownership, and continue. Release locks in every outcome and stop owned services when no longer needed.

Record unrelated bugs as linked issues only within authorization; finish the original scope.
