---
name: issue-pr-loop
description: "Use when user invokes /issue-pr-loop or asks an agent to autonomously work the GitHub queue: maintain open PRs, merge ready PRs, address reviews, claim unclaimed issues with an in-progress label, work in isolated git worktrees, ship PRs, and repeat until explicitly stopped. If CI/review is pending, move on to another PR or issue instead of idling; never end with only a status recap while queueable work remains."
---

# Issue/PR Loop

Continuous shipping loop. Each iteration: maintain open PRs first, then pick up new work. Run until the user explicitly stops you or there is nothing actionable left. Do not treat a status recap, green queue, or "waiting for CI/review" as loop completion; the next action is always the next iteration.

**Always prefer the next iteration over waiting on the current item.** Once a unit is pushed or blocked, advance to the next queueable PR or issue immediately — do not sleep, poll, watch, or set a monitor/wake-up to babysit what you just pushed. Pending CI and pending review resolve on their own; the next iteration's survey re-checks them. See § Waiting Rules.

Core rule: never send a final wrap-up while loop mode is active unless the user asks to stop/pause or the queue is genuinely empty. Use commentary updates for status, then keep moving.

Fully autonomous: this loop runs while the user is away, so **never ask the user a question and never wait for an answer.** There is no one to reply. When you would otherwise ask — ambiguous scope, an unfamiliar conflict, a risky merge — take the autonomous fallback in § Red Flags (skip, defer, file an issue, or leave a GitHub comment) and continue to the next iteration. Surface concerns as commentary or GitHub comments, never as blocking questions.

## One Iteration Is One Unit Of Work

Each iteration completes exactly one unit, chosen in this priority order:

1. Merge a PR that meets every MERGE signal (Phase 1).
2. Unblock a PR that is CONFLICTS, CI FAILING, or ADDRESS (Phase 1): rebase, fix CI, or address review feedback, then push.
3. Start the best available unclaimed issue when no PR is actionable (Phase 2): claim it with the in-progress label, branch in an isolated worktree, implement, run /simplify, push, and open a PR.

Always survey Phase 1 before Phase 2, and fall through to Phase 2 only when no open PR is actionable. Acquire the work-item lock before touching a PR or issue. When the unit finishes, immediately begin the next iteration; never batch two units into one, and never stop between them.

At the **start of each iteration's survey**, run the worktree cleanup sweep once (cheap — a few `gh` calls): `pwsh scripts/Remove-MergedWorktrees.ps1`. It reclaims disk by removing worktrees whose PRs already merged (including squash-merges by other agents/humans), and is safe — it never touches open-PR, detached, or dirty worktrees. See § Merge Command.

## Completion Contract

A unit of work is **done only when it is committed AND pushed** — never before. This applies whether you run the iteration locally or dispatch it to a subagent.

- Sequence: implement → run focused + broader tests (and docs build when docs changed) → `/simplify` → commit referencing `#<N>` → `git push --force-with-lease` → confirm the PR is opened/updated on GitHub. Only after the push lands is the unit complete.
- Use non-interactive commit-message commands only: `git commit -m "..."`, `git commit --file <path>`, `git commit --amend --no-edit`, or `git commit --amend -m "..."`. Never run bare `git commit` or bare `git commit --amend`; VS Code may open as `$EDITOR` and block the unattended loop.
- A status recap, a green **local** build, or an uncommitted/unpushed edit is **not** completion. If your work is uncommitted or unpushed, the unit is unfinished — finish it.
- Confirm the push actually landed before reporting: `gh pr view <N> --json headRefOid` must match local `git rev-parse HEAD`. A green local build with no matching remote head is a failed unit, not a done one.
- **Before the first file edit each iteration**, run the active-checkout verify block (§ Worktree Isolation) so edits cannot silently land in the shared checkout.
- If you cannot push (unresolvable conflict, repeated CI failure outside your context, lost lock), the unit **failed**: release the Redis lock (`pwsh $agentLocks release -LockName <lock>`), leave a GitHub comment stating the blocker, and report the failure honestly. Never report success for unpushed work.

## Dispatching Iterations

If the user explicitly authorizes subagents, dispatch each iteration to a subagent. The subagent does one unit of work and returns a one-line result. State lives in git and GitHub, so fresh context loses little.

Subagent prompt:

```text
Run one iteration of /issue-pr-loop in <repo path>.
Read this skill file and follow it exactly.
Use <repo path> only for queue survey and worktree creation; make code changes
inside an isolated git worktree. Before your FIRST file edit, run the active-checkout
verify block (§ Worktree Isolation) and abort if cwd is not the isolated worktree;
never edit the shared checkout.
Do exactly ONE of: merge a mergeable PR, address review feedback and push,
or pick the best available unclaimed issue, add in-progress label, branch, implement,
/simplify, push, and open PR. Size is not a blocker: take large coherent issues whole.
If an issue is genuinely too big for one coherent PR, split it into focused child
issues with native sub-issue + blocked_by links and take the first unblocked child.

COMPLETION CONTRACT — you are NOT done until your work is committed AND pushed:
- A unit completes only after `git push --force-with-lease` succeeds and the PR
  exists/updated on GitHub. A green local build is not completion.
- Confirm the push landed: `gh pr view <N> --json headRefOid` matches local
  `git rev-parse HEAD`. Never report a result whose work is uncommitted or unpushed.
- If you cannot push, the unit failed: release the Redis lock, leave a GitHub
  comment with the blocker, and report the failure — do not report success.

Return one line ONLY after the push is confirmed (or the unit has failed):
"merged #N" / "pushed fixes to #N" / "opened #N closing #M" / "failed #N: <reason>".
```

If subagents are not authorized, run the same one-iteration loop locally. After each iteration completes, immediately start another iteration instead of stopping.

## Worktree Isolation

Assume multiple agents may run concurrently. Use the shared repo path only for read-only queue inspection, GitHub CLI calls, and creating worktrees. Before any action that edits files, checks out branches, rebases, commits, tests generated files, or pushes fixes, enter an isolated git worktree.

Create worktrees outside the shared checkout so agents do not overwrite each other's files:

```powershell
$repo = git rev-parse --show-toplevel
$agentLocks = Join-Path $repo 'scripts/AgentLocks.ps1'
$worktreeRoot = Join-Path (Split-Path $repo -Parent) "ModularPipelines-worktrees"
New-Item -ItemType Directory -Force $worktreeRoot | Out-Null
```

Keep `$agentLocks` for the whole iteration and invoke that absolute shared-checkout path for every
lock verb, even after entering a worktree. A PR worktree can contain an older lock script whose
token-cache format is incompatible with the script that acquired the lock.

If the sandbox cannot write to the sibling directory, use `C:\tmp\ModularPipelines-worktrees` instead.

Do not rely on `Set-Location` inside one shell command to affect later tool calls. When using tool calls,
set the tool call `workdir` to `$worktree` for every implementation, test, `gh pr checkout`, commit,
push, and PR command. After creating or entering a worktree, verify the active checkout before any mutable
operation:

```powershell
$actualRoot = [System.IO.Path]::GetFullPath((git rev-parse --show-toplevel))
$expectedRoot = [System.IO.Path]::GetFullPath($worktree)
if (-not [string]::Equals($actualRoot, $expectedRoot, [System.StringComparison]::OrdinalIgnoreCase)) {
  throw "Refusing to edit outside isolated worktree. Actual=$actualRoot Expected=$expectedRoot"
}
git status --short --branch
```

The Redis lock auto-expires on a TTL — there is **no** periodic heartbeat to run. Optionally, once the worktree exists, write the auto-derive marker so later `release` calls from inside it can omit `-LockName` (also points Redis metadata at the real checkout). Skip this for issue branches — `release` already derives `issue-<N>` from the branch name:

```powershell
pwsh $agentLocks renew -LockName $lockName -Worktree $expectedRoot
```

For a new issue branch, claim the issue first, then create the branch and worktree:

```powershell
$branch = "issue-<N>-<short-desc>"
$worktree = Join-Path $worktreeRoot $branch
git -C $repo fetch origin main
git -C $repo worktree add $worktree -b $branch origin/main
Set-Location $worktree
$actualRoot = [System.IO.Path]::GetFullPath((git rev-parse --show-toplevel))
$expectedRoot = [System.IO.Path]::GetFullPath($worktree)
if (-not [string]::Equals($actualRoot, $expectedRoot, [System.StringComparison]::OrdinalIgnoreCase)) {
  throw "Refusing to edit outside isolated worktree. Actual=$actualRoot Expected=$expectedRoot"
}
```

For PR fixes, create a worktree for the PR head branch before editing:

```powershell
$branch = "<headRefName>"
$worktreeName = "pr-<N>-" + ($branch -replace '[\\/:*?"<>| ]','-')
$worktree = Join-Path $worktreeRoot $worktreeName
git -C $repo fetch origin main
git -C $repo worktree add --detach $worktree origin/main
Set-Location $worktree
gh pr checkout <N>
$actualRoot = [System.IO.Path]::GetFullPath((git rev-parse --show-toplevel))
$expectedRoot = [System.IO.Path]::GetFullPath($worktree)
if (-not [string]::Equals($actualRoot, $expectedRoot, [System.StringComparison]::OrdinalIgnoreCase)) {
  throw "PR checkout ran outside isolated worktree. Actual=$actualRoot Expected=$expectedRoot"
}
```

For tool calls, prefer two separate calls for PR work: first create the worktree from the shared repo,
then call `gh pr checkout <N>` with the tool `workdir` set to the worktree path. If `gh pr checkout` is run
from the shared checkout by mistake and no files were modified, immediately return the shared checkout to its
original branch (normally `main`) and continue only inside the worktree.

Do not use existing checkout state as an ownership signal; the local lock decides whether another agent owns a PR or issue. If a worktree path already exists, choose a unique path under `$worktreeRoot` while keeping the same lock. Run all implementation, tests, `/simplify`, commits, and pushes from inside the worktree.

Merged-PR worktrees are removed automatically — `Merge-Pr.ps1` removes the one it just merged, and the per-iteration `Remove-MergedWorktrees.ps1` sweep reaps any others (§ Merge Command). You only remove a worktree by hand when **abandoning an issue** (no PR merged), and only when it has no uncommitted work:

```powershell
git -C $repo worktree remove <worktree-path>
```

## Local Service Lifecycle

ModularPipelines has no Aspire AppHost. Do not run `aspire start`, `aspire stop`, or AppHost cleanup commands in this repo.

Tests that need external services use Docker or repo scripts. Only stop processes or containers you started for the current worktree. Never blanket-delete Docker containers or volumes; another agent or local test run may be using them (the `modularpipelines-agent-locks-redis` container in particular is shared lock infrastructure — never remove it).

## Concurrent Work Locks

When multiple agents run on the same machine, acquire a Redis lock before working on a specific PR or issue. Locks prevent duplicate rebases, duplicate review fixes, duplicate merges, and redundant issue implementation. If a lock exists, treat the item as owned and choose another actionable PR or issue. Redis is the only lock backend; do not use filesystem lock directories or PID checks for ownership.

Important: `$PID` is only the short-lived shell process that created or refreshed the lock. It is **not** the agent lifetime. Never remove a lock merely because the recorded process no longer exists.

Use the canonical shared-checkout script captured in `$agentLocks` — one local Docker Redis lock per work item, token-checked so one agent cannot release another's lock. Never invoke `scripts/AgentLocks.ps1` relative to a PR worktree: its branch may contain an older cache format. The lock **auto-expires on a 2h TTL** (the dead-man's switch if an agent dies); there is **no periodic heartbeat** — just `acquire`, do the work, `release`. **Do not** re-inline the Redis functions; each verb emits one line (or nothing), so a lock cycle costs a few tokens instead of ~100 lines of function bodies. The acquiring token is cached in a temp state file keyed by stable agent identity and lock name — never echo or track the token yourself. Codex supplies `CODEX_THREAD_ID` automatically. Non-Codex/human automation must set one stable, unique `MODULARPIPELINES_AGENT_LOCK_OWNER_ID` for every command from that logical agent, or pass the same `-OwnerId`; missing identity fails closed. Redis stays the sole ownership authority; the state file is only that agent's private token cache.

**Acquire** before touching a PR or issue (`$lockName` = `pr-<N>` or `issue-<N>`). Acquire always needs an explicit name — it runs before the worktree exists, and a PR lock's number is not in the branch:

```powershell
$token = pwsh $agentLocks acquire -LockName $lockName
switch ($LASTEXITCODE) {
  0 { }                    # acquired — proceed
  3 { <# HELD by another agent — skip this item, survey another #> ; return }
  default { <# docker/redis error — skip this item #> ; return }
}
```

**Release** in the `finally`/cleanup of every claimed item — from inside the worktree the name auto-derives, else pass it:

```powershell
pwsh $agentLocks release            # cwd = worktree: name from marker/branch
pwsh $agentLocks release -LockName $lockName   # or explicit
# exit 0 = released; exit 5 (STALE) = token mismatch/expired — do NOT delete or
# overwrite the key; just skip and survey again.
```

**Renew** is **optional and NOT periodic** — call it only for the rare unit that may run past the 2h TTL (re-arms it), or once with `-Worktree` to write the `agent.lockName` marker so a later bare `release` from that worktree resolves a `pr-<N>` lock:

```powershell
pwsh $agentLocks renew -LockName $lockName [-Worktree $expectedRoot]
# exit 4 (LOST): ownership expired and was re-taken — stop work on this item, do not push.
```

When `-LockName` is omitted (release/renew/status), the name resolves from the worktree's `agent.lockName` marker, else from an `issue-<N>-*` branch → `issue-<N>` (so issue work never needs `-Worktree`/`renew`; PR work either passes `-LockName` to `release` or writes the marker once via `renew -Worktree`). `status` prints `FREE` / `HELD` / `HELD-BY-ME` to inspect without mutating. Never assume the lock is gone until `release` exits 0.

Lock names:

| Work | Lock name |
|------|-----------|
| PR merge | `pr-<N>` |
| PR conflict rebase | `pr-<N>` |
| PR review or CI fixes | `pr-<N>` |
| New issue implementation | `issue-<N>` |

If `acquire` exits 3 (HELD), do not wait and do not inspect local files or recorded PIDs as proof of abandonment. Skip that PR or issue and choose another actionable item.

Redis locks expire automatically after the 2h TTL. Do not manually remove another agent's Redis lock. If a lock appears stale, rely on the TTL to expire it and work a different item in the meantime. GitHub's `in-progress` issue label remains the cross-process claim for issues (it outlives the lock, so a slow-but-live agent whose lock expired is still protected there); the Redis lock is the machine-level mutual exclusion for both issues and PRs.

## Phase 1: Maintain Open PRs

Survey:

```powershell
gh pr list --author @me --state open --json number,title,headRefName,mergeable,mergeStateStatus,reviewDecision,statusCheckRollup,isDraft
```

### Fetch All Review Surfaces Per PR

`gh pr list` hides inline comments. Without these calls, PRs with unaddressed feedback look identical to "waiting on reviewer":

```powershell
gh api repos/{owner}/{repo}/pulls/{N}/reviews
gh api repos/{owner}/{repo}/pulls/{N}/comments
gh api repos/{owner}/{repo}/issues/{N}/comments
gh api graphql -f query='query($owner:String!,$repo:String!,$pr:Int!){repository(owner:$owner,name:$repo){pullRequest(number:$pr){reviewThreads(first:100){nodes{isResolved comments(first:1){nodes{body author{login}}}}}}}}' -F owner=... -F repo=... -F pr=N
```

Actionable means any of: (a) a **review** (`pulls/{N}/reviews`) whose `state` is `CHANGES_REQUESTED`, or whose top-level body raises a concern/finding/suggestion — even when `state=COMMENTED` (bots and humans post blocking findings in the review body, not inline; `COMMENTED` is **not** non-blocking); (b) an inline comment with `position != null` and no reply from you; or (c) any unresolved review thread that is not eligible for autonomous resolution below. Check **reviews AND inline threads** — a PR can have zero unresolved threads yet a review body demanding changes. Only an empty/boilerplate review body (e.g. a Codex "will react 👍 if nothing to add" header) is non-blocking. When unsure, treat the review as blocking. This forces ADDRESS, not WAITING.

### Resolve Addressed Review Threads Autonomously

Unresolved bot threads must not strand a green PR. Resolve a thread under the PR lock when all are true:

1. The thread was opened by a bot.
2. You replied with the fix commit or a concrete technical disposition.
3. The current PR head contains that fix.
4. A bot review/CI cycle completed after the reply and did not rebut the disposition; a latest `REVIEW_VERDICT: CLEAR` against the current head is strongest evidence.

Do not resolve a thread with a newer rebuttal, an unaddressed finding, or a human reviewer awaiting a response. Address those first. Once every eligible thread is resolved, re-fetch review/thread/check state and continue to the normal merge gate in the same iteration.

Fetch thread IDs and resolve eligible threads with GitHub GraphQL:

```powershell
$threads = gh api graphql -f query='query($owner:String!,$repo:String!,$pr:Int!){repository(owner:$owner,name:$repo){pullRequest(number:$pr){reviewThreads(first:100){nodes{id isResolved comments(first:100){nodes{body createdAt author{login}}}}}}}}' -F owner=... -F repo=... -F pr=N

gh api graphql -f query='mutation($thread:ID!){resolveReviewThread(input:{threadId:$thread}){thread{id isResolved}}}' -F thread=<thread-id>
```

### State to Action

| State | Required signals | Action |
|-------|------------------|--------|
| MERGE | `MERGEABLE` + `CLEAN` + all `statusCheckRollup` `COMPLETED` and `SUCCESS`/`SKIPPED`/`NEUTRAL` + zero unresolved threads + zero unaddressed inline comments + **no review requesting changes or raising a concern in its body** (check `pulls/{N}/reviews`, not just threads) + (approved or repo does not gate on approval) + bot had a CI cycle since your last fix push | Run the fail-closed gate (§ Merge Command); merge only if `Assert-PrGreen.ps1` exits 0 |
| CONFLICTS | `mergeStateStatus=DIRTY` | `git fetch origin main`; run rebase with editors disabled: `git -c core.editor=true -c sequence.editor=true rebase origin/main`; resolve; continue with `git -c core.editor=true -c sequence.editor=true rebase --continue` so Git does not wait for interactive message editing; `git push --force-with-lease`. If conflicts touch code you cannot resolve confidently, abort the rebase, leave a PR comment noting the blocker, release the lock, and work another item. |
| CI FAILING | Any check `conclusion=FAILURE` | `gh pr checks <n>`; `gh run view <id> --log-failed`; fix the root cause as part of the loop; push. Rerun only if confirmed flaky: `gh run rerun <id> --failed`. |
| ADDRESS | Any actionable review, inline comment, or thread | Apply fixes. Reply per thread — and per blocking review body — with what changed or why not. Push. Do not merge same iteration; bots run on push, so let one CI cycle pass. |
| WAITING | CI green, no new comments since last push | Skip. |

Approval gate heuristic: if recent merged PRs on `main` lacked `reviewDecision=APPROVED`, the repo does not require human approval; green CI and clean threads suffice.

### Waiting Rules

Waiting is not a stopping condition. **Default to the next iteration, never to waiting on the current work item.**

- The instant your current unit is pushed (or blocked), move to the next iteration. Do not stay attached to the item you just pushed to see how its CI lands — the next iteration's Phase 1 survey re-checks it for free.
- If a PR has pending CI/review after your push, leave a short update and inspect the next PR.
- If all PRs are pending or externally blocked, pick a new issue and start a branch.
- **Never wait on, poll, or monitor a pending work item.** Forbidden while any other work is queueable: `Start-Sleep`; watch loops (`gh pr checks --watch`, `gh run watch`); the `Monitor` tool; scheduling a wake-up (`ScheduleWakeup`) to revisit the item; spawning a background task/command just to poll CI; or any sleep-then-recheck loop on the same item. The queue survey at the top of each iteration is the only polling you need.
- Watching/monitoring a pending check is a **last resort only**: no other actionable PR **and** no queueable issue (i.e. you would otherwise hit the Stopping condition). Even then, prefer one survey pass over a blocking watch.
- If a watched check remains pending but new work appears, stop watching and take the new work.
- When a PR becomes green in a later iteration, merge it unless it received your fix push in the current iteration.

### Merge Command — Fail-Closed Gate

Do not rely on branch protection being present, current, or configured for every branch. `scripts/Assert-PrGreen.ps1` is the local fail-closed gate. **Never merge without it passing.**

The guard enforces only the **mechanical** MERGE signals. The judgment-only signals from the MERGE row — approval (or repo-does-not-gate), zero unaddressed comments, and a bot CI cycle since your last fix push — are still yours to confirm before you run it; a passing guard is necessary, not sufficient.

Merge with the single command `scripts/Merge-Pr.ps1 -Pr <n>`. It runs the gate, merges only if the gate passes, then performs best-effort cleanup of that PR's isolated worktree and head branches:

```powershell
pwsh scripts/Merge-Pr.ps1 -Pr <n>
if ($LASTEXITCODE -ne 0) {
  # NOT merged (gate denied or merge failed) — nothing destroyed.
  # Leave a short update, skip the PR, advance to the next iteration.
}
```

`Merge-Pr.ps1` internally: (1) calls the pure `Assert-PrGreen.ps1` gate (re-fetches fresh state; exits 0 only when OPEN + `MERGEABLE` + `CLEAN` + every check terminal-and-passing + no unresolved threads); (2) on exit 0, runs `gh pr merge <n> --squash` without asking `gh` to delete a branch that may still be checked out; (3) finds and removes the clean worktree by the PR's head branch; (4) only after the worktree is gone, deletes the remote and local head branches. If the worktree has uncommitted tracked changes, it and both branches are preserved. Once the merge succeeds, later cleanup failures warn and still exit 0 — never retry an already-completed merge because cleanup was incomplete. The gate remains a separate read-only predicate — do not fold merging into it; `Merge-Pr.ps1` composes it.

Hard rules — no exceptions:

1. **Merge only via `Merge-Pr.ps1`** — never call `gh pr merge` directly. The wrapper runs the `Assert-PrGreen.ps1` gate in the same call and re-fetches fresh state (survey output goes stale within seconds); a direct `gh pr merge` bypasses the gate.
2. **`--auto` is forbidden.** Auto-merge bypasses this loop's local fail-closed gate and can merge later without the current review/thread judgment. `Merge-Pr.ps1` never passes `--auto`; never add it.
3. **Any non-terminal check (`QUEUED`/`IN_PROGRESS`/`PENDING`/`WAITING`) means the PR is not mergeable this iteration.** Skip it; the next iteration's survey re-checks it for free. Do not wait, poll, or auto-merge.
4. **`Merge-Pr.ps1` exiting non-zero is the correct outcome, not an obstacle.** It means the gate denied or the merge failed — nothing was destroyed. Advance to the next item; never retry the merge or work around the gate.

Worktree and branch cleanup are part of `Merge-Pr.ps1`. It removes a clean merged-PR worktree before deleting its remote/local head branches. If tracked changes exist, it preserves the worktree and both branches (untracked build artifacts are cleared). Post-merge cleanup failures are warnings because the merge cannot safely be retried. You do **not** run a separate removal step after merge.

**Per-iteration safety-net sweep.** Once per loop iteration, run `pwsh scripts/Remove-MergedWorktrees.ps1`. It reaps any worktree whose branch has a **merged PR** — including PRs squash-merged by another agent or a human, which `Merge-Pr.ps1` never saw. Detection is via GitHub's merged-PR head branches (`gh pr list --state merged`), the only signal that survives squash/rebase (the branch tip is not an ancestor of `main`, so ancestry checks miss it). It is squash-safe, skips branches with an open PR, skips detached worktrees, and preserves dirty worktrees. A `[gone]` branch is deliberately **not** treated as merged — that also happens to closed-unmerged PRs.

### Other Rules

- Address both blocking and minor suggestions when they improve the code.
- Push back once with reasoning if you disagree; if reaffirmed, implement.
- Resolve addressed bot threads using the deterministic criteria above; never leave a PR permanently blocked only because a bot did not close its own thread.
- Always use `git push --force-with-lease`. Never use bare `--force`.
- Never allow git to open an interactive editor. For rebase, use `-c core.editor=true -c sequence.editor=true` or set `GIT_EDITOR=true` and `GIT_SEQUENCE_EDITOR=true` in the same shell command. For commit-message edits, use `git commit -m`, `git commit --file`, `git commit --amend --no-edit`, or `git commit --amend -m`; do not run editor-driven commit, amend, squash, or rebase flows.

## Phase 2: Pick Up New Work

`in-progress` means another agent has already picked up the issue. Never pick an issue that already has this label.

Ensure the label exists before listing candidates:

```powershell
$inProgressLabel = gh label list --search in-progress --json name --jq '.[] | select(.name == "in-progress") | .name'
if (-not $inProgressLabel) {
  gh label create in-progress --color FBCA04 --description "Picked up by an agent"
}
```

```powershell
gh issue list --state open --search 'no:assignee -label:in-progress' --json number,title,labels,body,comments --limit 50
```

### Easiest-Issue Score

Lowest score wins.

| Modifier | Points |
|----------|--------|
| Clear acceptance criteria / `good-first-issue` / `easy` | +0 |
| Per discussion comment | +1 |
| Body empty or vague | +2 |
| Labeled `complex` / `epic` / `needs-design` / `discussion` | +3 |
| Has open native `blocked_by` dependencies, or otherwise depends on another open issue/PR | +5 |
| Labeled `wontfix` / `duplicate` / `question` | Skip |

For ties, choose any issue with a non-empty body and clear enough scope to ship. Do not deliberate.

This score orders work; it does not define a cutoff. **Size is never a reason to skip, pause, or stop.** If no small or easy issue is available, take the best available larger scoped issue. Avoid only items that are explicitly skipped, externally blocked, already claimed, or impossible to ship safely. A large but coherent issue — one with a single `Closes #<N>` deliverable — is queueable: implement the whole thing in one PR; do not chop a coherent feature into artificial fragments. Only when an issue is genuinely too large for one coherent PR (multiple independent deliverables, cross-cutting work, or no single closing PR) decompose it on GitHub and pick up the first unblocked child — see § Splitting An Oversized Issue below.

### Splitting An Oversized Issue

Do not pause or abandon an issue because it is large. Decide between two paths:

- **Coherent in one PR, even if big:** take it whole. Size alone is never a blocker.
- **Genuinely too large for one PR** — multiple independent deliverables, cross-cutting work, or no single coherent `Closes #<N>`: decompose it on GitHub, then pick up the first unblocked child *this iteration*. Splitting + implementing the first child is the unit of work.

ModularPipelines tracks decomposition with GitHub **native sub-issues** (parent/child) and **dependencies** (`blocked_by` / `blocking`) when available, not markdown checklists as source of truth. Prefer native `blocked_by` relationships for blockage. Repo is `thomhurst/ModularPipelines`. Write calls need the numeric issue `.id` (an integer distinct from `#number`, passed with `-F`, not `-f`).

First, survey existing decomposition so you do not file duplicates — an open-only `gh issue list` hides closed-but-done children and makes a mostly-finished epic look un-split:

```powershell
gh api repos/thomhurst/ModularPipelines/issues/<N>/sub_issues --paginate --jq '.[] | "\(.number) [\(.state)] \(.title)"'
gh api repos/thomhurst/ModularPipelines/issues/<N>/dependencies/blocked_by --jq '[.[].number]'
```

If children already exist, do not re-split — pick up the first unblocked child instead (skip to step 5).

1. Draft 2–5 focused child issues, each shippable in one PR with clear acceptance criteria. Carve along natural seams (per service, per layer, per endpoint) so children are independent where possible.
2. Create each child, then capture its numeric `.id` (`gh issue create` prints the URL, not JSON):

   ```powershell
   $childUrl = gh issue create --title "<focused title>" --body "Part of #<N>.`n`n<scope + acceptance criteria>"
   $childNum = ($childUrl.Trim() -split '/')[-1]
   $childId  = gh issue view $childNum --json id --jq '.id'
   ```

3. Link the child to the parent as a native sub-issue:

   ```powershell
   gh api --method POST repos/thomhurst/ModularPipelines/issues/<N>/sub_issues -F sub_issue_id=$childId
   ```

4. For ordering, mark a child blocked by its prerequisite (use the prerequisite's `.id`):

   ```powershell
   gh api --method POST repos/thomhurst/ModularPipelines/issues/<blockedNum>/dependencies/blocked_by -F issue_id=<blockingId>
   ```

5. Leave the parent open — it auto-tracks children. If you claimed the parent with `in-progress`, remove that label; the parent is now an epic, not directly workable:

   ```powershell
   gh issue edit <N> --remove-label in-progress
   ```

6. Pick up the first **pickup-ready** child — one whose every `blocked_by` target is `closed` (or that has none). Claim it with `in-progress`, branch, and implement this iteration. Children auto-unblock as prerequisite PRs merge and close their issues; later iterations pick up the next unblocked child.

### Claim Before Work

After choosing issue `<N>`, re-fetch it before branching:

```powershell
gh issue view <N> --json number,state,assignees,labels
```

If the issue is closed, assigned, or has `in-progress`, skip it and score the next candidate. Otherwise claim it:

```powershell
gh issue edit <N> --add-label in-progress
gh issue view <N> --json number,state,assignees,labels
```

Do not create the branch until the issue is confirmed open, unassigned, and labeled `in-progress`. Keep the label while a PR is open. If you abandon the issue before opening a PR, remove `in-progress` so it returns to the queue:

```powershell
gh issue edit <N> --remove-label in-progress
```

### Verify Shippable, Cover Full Scope

Search for the API, type, module, tool integration, options class, or pipeline scenario the issue references. If it does not exist, the issue is asking you to build it; that is the true scope.

Use `rg` across source, tests, docs, and tools before estimating scope:

```powershell
rg "<symbol|type|module|options class|extension>" src test docs tools -g "*.cs" -g "*.md" -g "*.json" -g "*.yml"
```

Cover every journey listed. `Closes #<N>` means closes, not "starts addressing." Only defer a journey when it depends on a separate actively-worked issue; link the dependency.

### Implement

1. Create and enter an isolated worktree for `issue-<N>-<short-desc>` using the worktree commands above.
2. Use TDD: failing test first; document why if not feasible.
3. Make the minimal implementation to pass.
4. Run focused tests, broader tests, and `npm run build --prefix docs` for docs changes. Use ModularPipelines' TUnit commands from `CLAUDE.md`: run the relevant `*UnitTests` project with `dotnet run --project <path-to-test-project> --framework net10.0`, and use `--treenode-filter` for focus. For TUnit focused runs, use one `--treenode-filter` pattern per command unless using valid TUnit OR parentheses.
5. Run `/simplify` before opening any code-touching PR.
6. Commit referencing `#<N>`.
7. Push and open a PR with `Closes #<N>` on its own line:

```powershell
@"
## Summary
...

## Test plan
- [ ] ...

Closes #<N>
"@ | gh pr create --title "..." --body-file -
```

## Stopping

Stop only when:

- User explicitly says "stop", "pause", or "enough", or interrupts.
- Queue is genuinely empty: zero queueable unassigned open issues without `in-progress`, and every open PR is blocked on a human decision or another external dependency after all eligible bot threads were resolved autonomously.

Do not stop because of long context, lots already shipped, compute spend, uncertainty about which issue is next, an issue being large or daunting, green PRs after a fix push, pending CI, pending review, or a desire to wrap up. If another issue is queueable, work it instead. Larger scoped issues count as queueable when no smaller unclaimed work is available; if one is too big for a single PR, split it (§ Splitting An Oversized Issue) rather than stopping.

At every "maybe stop" impulse: `gh pr list`, act on actionable PRs, otherwise pick the best available unclaimed issue, claim it with `in-progress`, and branch.

## Red Flags: Defer, Don't Ask

This loop runs unattended. **Never ask the user a question and never wait for a reply** — there is no one to answer. When you hit something you cannot safely complete, take the autonomous fallback below (skip / defer / file an issue / leave a comment), release the lock, and move to the next iteration. Surface the concern in commentary or a GitHub comment, not as a blocking question.

| Situation | Autonomous action |
|-----------|-------------------|
| Conflict resolution needs code understanding outside your context | Abort the rebase, comment the blocker on the PR, release the lock, work another item. |
| Issue scope turns out wildly larger than its title | Do **not** pause. If still one coherent deliverable, take it whole; if genuinely too large for one PR, decompose into linked child issues and pick up the first unblocked child (Phase 2 § Splitting An Oversized Issue). If you cannot find a sensible decomposition, file a comment on the issue describing why, remove `in-progress`, and pick another item. |
| Reviewer reaffirms a change you technically disagree with | Implement it, then note in the thread if it materially weakens the code. |
| CI failure looks like infra or flake and you already reran once | Comment the suspected cause on the PR, skip the PR, and work another item. Do not rerun again or mask it. |
| Fixture or seeder exposes a real bug needing cross-cutting investigation | File a new GitHub issue capturing the bug, link it from the current PR/issue, and continue with the original scope. |

## Gotchas

- Lost code in rebase: when both branches add at the same insertion point, picking one side can silently drop the other. Diff against `origin/main` after rebase.
- Rebase/editor hangs: after resolving conflicts, use `git -c core.editor=true -c sequence.editor=true rebase --continue` or another non-interactive continue command. Avoid any git command that opens VS Code for commit messages, amends, squashes, or rebase TODOs.
- Library code under `src/` must use `ConfigureAwait(false)` on awaited tasks; tests do not need it.
- **Generated options classes are off-limits (CLAUDE.md):** tool options classes (e.g. `GitAddOptions`, `DotNetBuildOptions`, `DockerRunOptions`) are auto-generated by `tools/ModularPipelines.OptionsGenerator/` and will be overwritten. Never hand-edit a generated file; change the generator or add a manual extension file instead.
- `/simplify` and review feedback must respect the generator boundary too — a "simplification" that edits a generated file will be silently reverted on the next generation run; decline it in the thread with that rationale.
- Run `dotnet format` before committing; CI verifies formatting with `dotnet format --verify-no-changes --severity info`.
- The full pipeline (build + tests + coverage) runs via `cd src/ModularPipelines.Build && dotnet run -c Release --framework net10.0`. If Docker is unavailable for tests that need it, state that clearly in the PR test plan.
- Flaky tests indicate real bugs. Do not rerun CI just to get green; investigate timing, synchronization, and shared-state causes.
- Local debugging beyond 2-3 iterations on one test: push to CI and let CI judge.
