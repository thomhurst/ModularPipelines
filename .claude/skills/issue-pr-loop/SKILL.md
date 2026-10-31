---
name: issue-pr-loop
description: "Run the GitHub issue/PR queue when the user invokes /issue-pr-loop or requests continuous queue work: maintain PRs, claim issues, implement in isolated worktrees, and ship until stopped."
---

# Issue/PR Loop

Read the root agent instructions and [repository guidance](references/repository.md). Keep this workflow identical across repositories; local requirements belong in that reference or their maintained source. Reviewing or editing the skill does not start the loop. Stay within the user's authorized scope.

## Loop and completion

Run unattended. At each iteration, run `pwsh scripts/Remove-MergedWorktrees.ps1` from the shared checkout, survey the queue, then complete one unit in priority order:

1. Fix a red `main`, following [Red main](#red-main).
2. Merge an eligible PR.
3. Fix conflicts, failed CI, or review findings on an open PR and push.
4. Publish finished work stranded on a local `issue-<N>-*` branch that has no PR, or recover valuable changes from a preserved merged-PR worktree, following [recovery guidance](references/recovery.md).
5. Tidy the issue queue: close a parent whose native sub-issues are all closed, and close an issue that `origin/main` already implements or that another issue supersedes.
6. Claim an available issue, implement its full scope, and open a PR. Prefer issues that block other issues.
7. When nothing above is actionable, run a [triage unit](#triage) instead of waiting.

Apply repository-specific PR maintenance rules before choosing repair work; some PR categories require closure and a linked issue instead.

After pushing or deferring, survey again. Pending CI/review means work another item; do not watch, sleep, poll one item, or schedule monitors while work is queueable. When only CI/review remains, periodically survey the whole queue and stay responsive.

Stop on the user's stop/pause, or when a fresh survey finds no queueable issue, recovery candidate, or actionable PR and remaining work requires an external decision/dependency. Pending CI alone is not completion. Continue without blocking questions.

### Resolve, do not defer

Every unit costs a full agent run, so a deferral that changes nothing is wasted work. Before you defer, try to finish the item another way:

- **Blocker first.** When the blocker is an open issue or PR in this repository that you can work, release this item's lock, acquire the blocker's lock, and work the blocker in this unit. Report the result for the blocker. Defer only when the blocker is itself locked, or it needs a human decision or an external dependency.
- **Already implemented.** When `origin/main` already delivers an issue, close it with a comment that names the commit or PR. When an open PR delivers it, make sure the PR body links the issue with `Closes #<N>`, then work the PR instead.
- **Finished parent.** When every native sub-issue of a parent is closed, check the parent's own acceptance criteria against `origin/main`. Close it with evidence, or implement what is left.
- **Stale issue.** When an issue's body no longer matches the code, for example because part of it shipped, update the body so that it describes only the remaining work, then implement that work.
- **Pushed and waiting.** A pushed fix that waits for CI, hosted generation, or review is `pushed fixes to #N` or `opened #N closing #M`, not `deferred`.
- **Waiting on a run.** When the item waits only for a workflow run that you or an earlier unit started, such as a hosted generation, report `waiting #N: run <id>`. The driver returns the item when that run completes. Do not add `on-hold` for a wait that ends by itself.
- **Superseded PR.** Close a PR that another open or merged PR replaces, or an automated PR that is defective or conflicting, and link the replacement. Closed PRs keep their history; an open PR is not a place to keep evidence. Follow the repository rules for closed-PR CI cancellation.
- **Earlier deferral.** When the prompt quotes an earlier deferral reason, start from that blocker. Check whether it still holds and try each route above before you defer the item again for the same reason.
- **Decide and document.** When an item offers options and one of them is reasonable and reversible, pick it, state why in the PR or issue, and continue. Review catches a wrong choice faster than a parked item gets a human answer. Do not invent constraints the repository does not state: code that does not ship in a package (tools, tests, scripts, samples) has no compatibility contract, and a breaking change is allowed on a milestone that permits breaking changes. When acceptance criteria conflict with each other or with the code, rewrite them to the option you chose and say so; do not park the item to ask which criterion wins.

Defer only for a reason that no agent can remove, and name its category in the result: `[human]` for a human decision that no evidence can settle (a break to a shipped public API outside a breaking milestone, a security trade-off, spending money, or a choice the user reserved), `[upstream]` for an upstream release, fix, or date, `[credentials]` for missing credentials or infrastructure access, and `[locked]` for a lock held by another worker. A prerequisite that an agent can work is not a reason to defer: work it, as described in **Blocker first**. The driver gives a deferral without one of these categories a short expiry, so the item comes back soon. Record the blocker once. Make it visible to the driver, so that later units do not re-examine an unchanged item:

- For a prerequisite, add a native `blocked_by` dependency. The driver skips the item until the prerequisite closes.
- For a human decision, add `needs-human` and state the decision, its options, and the option you recommend. Do the work that does not depend on the answer first.
- For an external gate, such as a release date or an upstream fix, add `on-hold` (create the label if needed) and name the condition that removes it. Remove `on-hold` yourself as soon as that condition is met. Never use `on-hold` for CI, reviews, workflow runs, or other issues and PRs in this repository: those end by themselves or can be worked. Before you park an item behind a release, do the parts that do not need the release.

Name every blocker as `#N` in the result. The driver works an open blocker next, and while a blocker carries `needs-human` or `on-hold` it skips the items that wait on it.

Do not post another blocker comment on an item whose blocker is unchanged. Edit your existing comment instead. Start every comment you post with `<!-- issue-pr-loop -->`, so the driver can tell your comments from new information.

### Triage

A triage unit keeps the queue accurate, so other units spend their time on real work. The driver runs one when nothing is actionable, and also every few units. It names a batch of items that no unit audited recently: parked and blocked items first. Without the driver, pick the oldest open issues that carry `needs-human`, `on-hold`, or open `blocked_by` dependencies, then the rest.

For each item, acquire its lock (`issue-<N>` or `pr-<N>`), and skip it when `acquire` exits 3. Then check it against fresh `origin/main` and its linked issues and PRs:

- Close an issue that `origin/main` already implements, or that another issue or PR supersedes. Name the commit, PR, or issue in the closing comment.
- Rewrite the body of an issue that no longer matches the code, so that it describes only the remaining work and has acceptance criteria.
- Add missing native sub-issue and `blocked_by` links, and remove links to closed or unrelated items.
- Remove `on-hold` when its condition is met, for example the release shipped or the run completed. Remove `in-progress` when no open PR, lock holder, or branch backs it.
- For `needs-human`, check whether a comment since the label was added answers the question. When it does, apply the answer, remove the label, and say so.
- Close a superseded, defective, or conflicting automated PR and link its replacement.
- Flatten issue chains. A sub-issue tree deeper than parent and child, or a parent with one open child, hides work from the queue: move the remaining acceptance criteria into one issue and close the rest as superseded. Close a child whose only work is to wait, re-run, or re-validate what another issue or an automated PR already delivers.
- For a parent whose open children are all parked or blocked, check each child's blocker. Unpark the children whose condition is met, and move work that does not need the blocker into an issue that can be claimed now.
- For `needs-human` or `question` with a recommended option that is reasonable and reversible, apply **Decide and document**: take that option, remove the label, and say so.

Make only changes that the evidence supports, and keep a comment short. Do not implement an issue in a triage unit.

When the prompt says that fewer than five issues can be claimed now, also file up to three new issues for concrete defects you find in the repository: failing or flaky tests in recent CI runs, analyzer or build warnings, `TODO` or `FIXME` comments that describe a defect, documentation that contradicts the code, or missing tests for public behavior. Search open and closed issues first so that you do not file a duplicate. Give each issue the evidence (file and line, run, or log line) and acceptance criteria. Do not file issues for style preferences or speculative features.

Report `RESULT: triaged: <summary>`, for example `triaged: closed #12 #15, updated #18, filed #40`. Report `triaged: no changes` when every item was current.

### Discovery

The driver runs a discovery unit when nothing is actionable and every item was audited recently, instead of sleeping. Find real work and file it as issues that a unit can claim now. Look, in this order, at: failed or flaky jobs in recent CI runs on `main` and on PRs; open milestone issues whose acceptance criteria are not yet split into claimable work; analyzer and build warnings; `TODO` and `FIXME` comments that describe a defect; documentation that contradicts the code; and public behavior without tests. Search open and closed issues first so that you do not file a duplicate.

File at most five issues. Each issue states the evidence (file and line, run, or log line), the expected behavior, and acceptance criteria, and is small enough for one PR. Do not file issues for style preferences or speculative features. Report `RESULT: triaged: filed #A #B` or `triaged: no changes`.

### Red main

`main` is red when the latest completed push run of any workflow on `main` failed. A red `main` fails every PR for the same reason, so it comes before PR work. Work it under the `main-ci` lock.

1. Read the failed jobs' logs. Send them to a file and read only the failing lines.
2. Search open issues and PRs for this failure. When an open PR fixes it, release `main-ci` and work that PR. When an open issue tracks it, work that issue.
3. When the failure is infrastructure, such as a lost runner, a network error, or a rate limit, rerun the whole run once with `gh run rerun <id>` and report `reran main run <id>`. Do not rerun a run that was already rerun; treat a repeat failure as real.
4. Otherwise file an issue with the failing run, the failing step, and the error, labelled `bug`. Claim it, fix it in an `issue-<N>-*` worktree, and open a PR that closes it. Never push to `main` directly.
5. When no agent can fix the cause, file the issue with `needs-human` and report `deferred main [human]: reason` that names the issue. The driver re-checks `main` when that issue closes or a new run finishes.

Use commentary while active. Preserve item IDs, worktree, lock identity/script, owned services, and unresolved validation across compaction.

### Single-unit mode

When the invocation asks for single-unit mode (for example "run once" or "single unit"), it overrides the loop above. Run the cleanup script, survey, and complete exactly one unit in priority order. Release every lock you hold and stop owned services. Then print one final line and stop:

```text
RESULT: merged #N | pushed fixes to #N | opened #N closing #M | reran main run <id> | waiting #N: run <id> | deferred #N [category]: reason | deferred main [category]: reason | failed #N: reason | triaged: summary | queue-empty
```

Do not survey again, start a second item, or wait for CI or reviews. When the survey finds nothing actionable now, including when only pending CI or reviews remain, run a [triage unit](#triage) instead. Print `RESULT: queue-empty` only when triage finds every item current and audited. Record anything a later run needs, such as a defer reason, on the issue or PR, not in agent memory. `scripts/Start-IssuePrLoop.ps1` runs this mode repeatedly, with a fresh agent process and context for each unit.

A lock held by another owner is not a unit. When `acquire` exits 3, skip that item and try the next candidate in priority order. Report `deferred` only for an item you acquired and examined.

When the invocation assigns an item and says the loop driver holds its lock, skip the cleanup script, the survey, and `acquire`. The driver already did them, and the lock belongs to the owner variable it set for this run. The assigned item is where the unit starts, not a limit on what it may touch. When moving the item needs action on a linked item (a blocker, a replacement or superseded PR, a duplicate, its parent, or its children), acquire that item's lock and act on it in this unit. Do not start unrelated work. Re-check its state before acting, because the driver's survey is mechanical. Release the lock when done. After a `deferred` or `failed` result the driver records the item's GitHub state, and it skips the item until that state changes. A clear blocker comment on the item is still required, because the driver's record is not visible to people.

### Context and delegation

Model latency grows with context size, and it dominates loop time: tools account for a small fraction of wall time. Keep each unit's context small.

- When the runtime can start subagents or fresh threads, invoking this loop authorizes one per unit. Detect the runtime from the environment: `CLAUDECODE=1` means Claude Code, so use the Agent tool with a `general-purpose` subagent. `CODEX_THREAD_ID` set means Codex, so use its subagent or thread tool. The coordinator surveys, chooses one item, and hands over the item ID, this skill in single-unit mode for that item, the shared checkout, and the canonical lock script. The subagent acquires the lock and does the unit. Require a confirmed `merged #N`, `pushed fixes to #N`, `reran main run <id>`, `waiting #N: run <id>`, `deferred main [category]: reason`, `deferred #N [category]: reason`, `opened #N closing #M`, `failed #N: reason`, or `triaged: summary`; then continue surveying. Keep only queue state in the coordinator, never diffs, logs, or review bodies.
- When a runtime lets you pick reasoning effort, use a lower effort for mechanical units: merges, surveys, conflict-free rebases, and disposition replies. Keep high effort for implementation, conflict resolution in shared code, and CI failure diagnosis.
- Without subagents, drop the previous item's details once it is pushed or deferred. In both modes, send long command output (builds, tests, `gh api` dumps, logs) to a file under the evidence root ([Isolation and ownership](#isolation-and-ownership)) and read only the lines you need. Use `--jq` to select fields.

## Isolation and ownership

Use the shared checkout for surveys/setup. All implementation, validation, checkouts, commits, and pushes run in an isolated worktree. Capture from the shared checkout:

```powershell
$repo = git rev-parse --show-toplevel
$repoSlug = gh repo view --json nameWithOwner --jq .nameWithOwner
$repoName = ($repoSlug -split '/')[-1]
$agentLocks = Join-Path $repo 'scripts/AgentLocks.ps1'
$worktreeRoot = Join-Path (Split-Path $repo -Parent) ($repoName + '-worktrees')
$evidenceRoot = Join-Path (Split-Path $repo -Parent) ($repoName + '-evidence')
```

Verify the repository and intended base; examples use `origin/main`. Use a writable temporary `<repoName>-worktrees` root if needed. Choose a unique worktree path when one already exists.

Write nothing loose beside the repository. Every file an agent produces outside its worktree has one home, so cleanup can find it:

- Evidence (build and test logs, reports, PR and comment body drafts, diffs, validation projects) goes in `<repo-parent>/<repoName>-evidence/<lock-name>/`, for example `$evidenceRoot/issue-1234/build.log`. `scripts/Remove-MergedWorktrees.ps1` deletes an evidence entry once its newest file is older than 7 days (`-EvidenceRetentionDays`).
- Throwaway checkouts and baselines are worktrees under `$worktreeRoot`, never plain copies.
- Downloaded SDKs and CLI tools go in `<repo-parent>/toolchains/<name>-<version>/`, shared by every repository.
- Never write to `<repo-parent>` itself or to the shared checkout.

Acquire the Redis lock before acting on an item: `pr-<N>` for PR work or recovery, `issue-<N>` for new implementation. Always invoke the absolute shared-checkout `$agentLocks` path for every verb; older branch copies can have incompatible token caches.

| Command | Result |
| --- | --- |
| `pwsh $agentLocks acquire -LockName $lockName` | Exit 0 grants ownership; 3 means held, skip; other failures mean defer, never bypass Redis. |
| `pwsh $agentLocks renew -LockName $lockName` | Renew work approaching the two-hour TTL. A `FREE` lock is taken back, for example after a Redis restart; re-fetch the item's state before you push. Exit 4 means another owner holds it: stop this item and do not push. |
| `pwsh $agentLocks status -LockName $lockName` | Read-only `FREE` / `HELD` / `HELD-BY-ME`; verify ownership before pushing or merging. On `FREE`, run `renew` to take the lock back. |
| `pwsh $agentLocks release -LockName $lockName` | Release in cleanup/finally; exit 0 confirms release, 5 means stale ownership, leave the key alone. |

Redis is authoritative: never steal locks based on PIDs/files/inactivity or add another backend. Do not print/manage cached tokens. Codex supplies `CODEX_THREAD_ID`, and `scripts/Start-IssuePrLoop.ps1` sets the lock script's owner variable for each run. In an interactive Claude Code session, pass `-OwnerId $env:CLAUDE_CODE_SESSION_ID` on every verb. Other automation needs the same stable unique `-OwnerId` on every verb. Renew when needed, without periodic heartbeats. Required `renew -Worktree $worktree` after checkout records the path for release cleanup; keep explicit lock names. The issue's `in-progress` label persists independently.

After claiming an issue, create branch/worktree `issue-<N>-<short-desc>` from freshly fetched `origin/main`. For PR fixes, create a detached worktree from `origin/main` with `git worktree add --detach --lock --reason 'PR checkout setup'`, then run `gh pr checkout <N>` with that worktree as `workdir`. Wrap checkout and lock-path registration in `try/finally`: in `finally`, run `git worktree unlock <absolute-path>` for the temporary Git lock created by this setup, including when checkout or registration fails, before the outer Redis release. Report an unlock failure and preserve the path for recovery. The temporary Git lock protects setup from concurrent cleanup; Redis remains the ownership authority. PR directories use `pr-<N>-<description>`; never rename or reuse them for another PR. A separate local review/rebase branch must retain the same `pr-<N>` identity.

Set every mutating tool call's `workdir` explicitly; shell directory changes do not persist. Before the first edit and after checkout changes, compare `git rev-parse --show-toplevel` with the intended worktree's resolved absolute path and inspect `git status --short --branch`. Abort on a mismatch.

Use repository scripts for merged-worktree cleanup; `[gone]` or ancestry alone is insufficient. Preserve dirty, locked, open-PR, harness-managed, and locally divergent worktrees. Inspect untracked source before cleanup: scripts may clear build artifacts. Manually remove abandoned worktrees only after verifying repository/path, ownership, and absence of valuable unpublished work. Do not enable stale/scratch cleanup during surveys.

Stop only services/processes/containers started for this worktree. Never blanket-delete containers/volumes or stop shared lock Redis. Follow local lifecycle guidance; use Aspire only where an AppHost exists.

Immediately after checkout, run `pwsh $agentLocks renew -LockName $lockName -Worktree $worktree` once to register its path. Stop owned processes, save needed evidence under `$evidenceRoot/<lock-name>/`, then release in `finally` from a command whose `workdir` is the shared checkout, never the worktree: on Windows a shell whose current directory is inside the worktree blocks its deletion. Run `Merge-Pr.ps1` the same way. Release removes clean checkouts even for open PRs; branches and detached commits remain available for re-checkout. Never `git worktree lock` a checkout to keep it, except for the temporary setup lock; commit and push work you need. See [local worktree lifecycle](../../../scripts/WorktreeLifecycle.md) for preservation rules and crash behavior.

## Maintain PRs

Survey with `gh pr list --author @me --state open --json number,title,headRefName,headRefOid,mergeable,mergeStateStatus,reviewDecision,statusCheckRollup,isDraft`. Include other authors only when the user's queue scope includes them. Paginate or increase limits so an incomplete first page cannot make the queue appear empty. Include drafts; skip PRs labeled `needs-human` (see [Rework budget](#rework-budget)) or `on-hold`.

Inspect all review surfaces, including pagination and replies: REST `pulls/<N>/reviews`, `pulls/<N>/comments`, `issues/<N>/comments` under `repos/{owner}/{repo}`, and GraphQL `reviewThreads` with IDs, resolution state, bodies, authors, and timestamps.

Never infer a reviewer's surface from its identity: Claude often posts its review as a PR conversation comment (`issues/<N>/comments`), while other agents usually post review threads. Check both surfaces every cycle. Treat a conversation comment that carries findings or a verdict as a review; it has no thread to resolve, so disposition it with one PR comment that quotes or links it.

A `COMMENTED` review body can block with zero unresolved threads. Address outstanding concerns, `CHANGES_REQUESTED`, current inline findings, and unresolved threads. Empty acknowledgments are non-blocking. Assess older findings against the current head and later discussion; uncertain concerns block merging.

| State | Action |
| --- | --- |
| Conflicts | Rebase onto fresh `origin/main`, inspect the diff for lost changes at shared insertion points, validate, and push. Abort and defer conflicts that cannot be resolved confidently. |
| CI failure | Read failed logs and fix the cause. Investigate timing/synchronization failures. At most one rerun for a demonstrated infrastructure problem or diagnosed flake when local rules permit; never rerun for green, skip/delete failing tests, or mask a failure. |
| Stale CI | When failed checks ran on a base far behind `origin/main`, or the failure was an infrastructure problem and the rerun allowance is spent, rebase onto fresh `origin/main` and push for a fresh run instead of deferring. This push is not a rerun and does not spend the allowance; an unchanged head never leaves deferral. Treat a failure that repeats on the fresh base as a real CI failure. |
| Feedback | Verify every finding against code and classify it. **Blocking**: a correctness, concurrency, security, contract, or performance defect, or missing tests for changed behavior. Fix each valid blocking finding in this PR. **Non-blocking**: naming, style, wording, optional refactors, extra tests for unchanged behavior, and "consider" suggestions. Implement a non-blocking suggestion only when it is small and you already push a blocking fix. Otherwise decline it in the disposition with a one-line reason and push nothing for it. Decline a wrong finding with evidence from the code. Move architectural work that is genuinely out of scope to a filed follow-up issue, linked in the reply. "Not done" or "left as is" without a classification, reason, or linked issue is not a disposition. Reply once per review covering each finding, and reply on each thread. Discuss disagreement once; implement reaffirmed blocking feedback. A disposition-only reply on the current head needs no new push, so it does not start another review cycle. |
| Draft | Follow [Draft review lifecycle](#draft-review-lifecycle), including requesting missing reviews and marking ready once approved. |
| Pending | Take another item. After a fix push, allow a subsequent bot review/CI cycle before considering merge in a later iteration. |
| Merge candidate | Confirm every condition below, then invoke the wrapper. |

Resolve every review thread under the PR lock, whether a human or a bot opened it, as soon as it is dispositioned: the fix commit is in the remote PR head and the reply names it, or the reply pushes back on the finding with evidence. Do not wait for another review/CI cycle to resolve a dispositioned thread; that cycle still gates merging. Leave a thread open only while it has no disposition: the fix is not pushed, the item waits on the user, or the reviewer replied after your disposition. On each later review cycle, re-fetch every thread, resolved ones included; a reviewer reply newer than your disposition reopens it: run GraphQL `unresolveReviewThread` and handle it as reaffirmed feedback. Reply to addressed review-body findings and request re-review when an outstanding changes-requested review needs updating; do not dismiss reviews to bypass approval requirements. Use GraphQL `resolveReviewThread`, then re-fetch reviews, threads, checks, and head SHA.

### Draft review lifecycle

Open every PR created by this loop as a draft (`gh pr create --draft`), including recovery and split PRs. Keep it in draft while reviewing and fixing findings, so build/test CI runs once, on an approved head. This lifecycle applies when the loop is invoked; repository guidance can extend it to other agent-created PRs.

1. After creation and each draft push, re-fetch `isDraft` and `headRefOid`. Claude's workflow reviews drafts automatically. Request Codex manually only while `isDraft` is true: post `@codex review` using `gh pr comment <N> --body-file <path>`, starting with `<!-- issue-pr-loop -->` and a separate `<!-- codex-review:<full-head-SHA> -->` marker. Fetch all issue comments first; do not duplicate a pending request or request a review when this head already has a positive Codex verdict. A request is pending for 15 minutes after its `createdAt`; if Codex has posted no review, comment, or thumbs-up for this head by then, the request was dropped. An eyes reaction does not count: Codex often adds one and then never reviews. Post a fresh request with the same markers and keep waiting. Codex silence never ends the unit or the loop; report the PR as waiting (or take another item) and re-check it on the next visit. If a disposition needs reviewer confirmation without a new head, request a follow-up linking that disposition, then wait for its response. Never tag Codex on a ready PR; rely on its automatic review there.
2. Read every review surface (see [Maintain PRs](#maintain-prs)). A successful check, a request, an eyes reaction, silence, or an approval for an older head is not approval. Require completed, positive verdicts from both Claude and Codex for the current head; [repository guidance](references/repository.md) names each reviewer's account and verdict form. A clean `COMMENTED` review with an explicit no-blockers verdict or a clean Codex review receipt identifying this head counts; so does a Codex thumbs-up on this head's request, after verifying the reaction author through the comment reactions API. Honor any formal approval required by repository rules; do not approve your own PR or manufacture reviewer approval.
3. Dispose of findings as in the **Feedback** row, leaving no blocking or undecided findings and no unresolved threads. After a fix changes the head, keep the draft, wait for fresh Claude review, and request Codex once for the new head, re-requesting only after the 15-minute timeout in step 1. Do not reuse the earlier head's approval. Pending review is a wait, not a reason to park or abandon the PR.
4. Under the PR lock, re-fetch the head, draft state, verdicts, threads, and checks. Build/test CI starts only when the PR is ready; skipped or absent draft CI is expected, not a blocker to promotion. When both reviewers approve the same current head and findings are dispositioned, run `gh pr ready <N>` and verify `isDraft` is false and the head is unchanged, then arm auto-merge at once (step 5). Do not mark ready while conflicts remain. If the head changes during promotion, restore draft with `gh pr ready <N> --undo` and review the new head.
5. Arm native auto-merge right after promotion with `pwsh scripts/Enable-AutoMerge.ps1 -Pr <N>`, without waiting for CI, then take another item; GitHub merges once the required checks pass. Marking ready starts CI but does not re-run Claude review, so the draft review of this head satisfies the arming gate. Never arm a draft. Read any automatic reviews that readiness triggers; on a blocking finding, run `gh pr merge <N> --disable-auto` before fixing it. If CI fails, auto-merge holds; fix it as a CI failure. Any push disarms auto-merge, so re-arm only after the new head's reviews and dispositions.

### Rework budget

The loop runs unattended, so keep working a PR until it merges. A PR that keeps getting new heads can still fail to converge, because each push needs a new review and disposition. Before working a PR, count its automated review rounds (the counting command is in [repository guidance](references/repository.md)) and its force-pushes. A PR is over budget when either condition is true:

- It has 20 or more automated review rounds, and the latest review still has a blocking finding.
- It has 10 or more force-pushes (`head_ref_force_pushed` events in `issues/<N>/timeline`) after it first had conflicts.

Do not push more fixes to an over-budget PR. Split it yourself instead:

1. File one native sub-issue of the PR's linked issue per part that can merge on its own (when the linked issue is itself a child, file the parts as siblings under its parent and close it as superseded), as described in [Pick an issue](#pick-an-issue). Each sub-issue names the files it owns, the blocking findings it must resolve, and the PR branch whose commits it can reuse. Put `<!-- split-from-pr #<N> -->` in each sub-issue body.
2. Post one comment on the PR. It lists the open blocking findings, the files that keep conflicting, and the sub-issues.
3. Close the PR without merging and keep its branch. Remove `in-progress` from the linked issue.
4. Release the lock. Later units pick up the sub-issues like any other issue.

Add the `needs-human` label only as a last resort, when either condition is true:

- The PR's linked issue body contains `<!-- split-from-pr #<N> -->`, so the PR is already a split, and it is over budget again.
- The remaining blocker needs a decision that no code or evidence can settle, such as a public API break, an upstream dependency change, or a security trade-off.

Before you add `needs-human`, ensure the label exists. Post one comment that states the decision needed and the options. Release the lock. Later surveys skip the PR while it has `needs-human`, and a human removes the label to return it to the queue.

Keep new PRs small enough to converge. When an issue's change would touch many files across separate subsystems, split it into sub-issues as described in [Pick an issue](#pick-an-issue) instead of opening one large PR.

### Merge gate

After the [Draft review lifecycle](#draft-review-lifecycle), arm native auto-merge with `pwsh scripts/Enable-AutoMerge.ps1 -Pr <N>`. It runs `Assert-PrGreen.ps1 -ForAutoMerge`, which accepts pending or not-yet-started CI but denies drafts, conflicts, failed checks, a current head without a CLEAR Claude verdict, actionable reviews, and unresolved threads, then arms a squash merge pinned to the validated head. GitHub waits only for branch protection's required checks; repository guidance names them. The conditions below apply to an immediate merge, for a PR whose CI already passed while not armed.

Merge requires an open, ready-for-review (not draft) PR; `MERGEABLE` and `CLEAN`; every applicable check present, terminal, and passing (`SUCCESS`, `SKIPPED`, or `NEUTRAL`, with successful legacy commit statuses); no unresolved threads, unaddressed comments, or outstanding review-body concerns; approval when required; and a bot review/CI cycle after the last fix push. Missing expected checks and nonterminal checks block merging. Reassess if the head changes.

Use current repository rules/instructions for approval requirements. Historical merges without approval do not prove approval is optional; inaccessible protection APIs do not prove no protection exists.

```powershell
pwsh scripts/Merge-Pr.ps1 -Pr <N> -Worktree $worktree
```

Omit `-Worktree` only when none exists. The wrapper calls `Assert-PrGreen.ps1`, merges on a passing mechanical gate, and cleans up. Judge review dispositions and approval yourself; the script is necessary but insufficient.

Never merge or arm with `gh pr merge` or `gh pr merge --auto` directly; use these wrappers. Disarming with `gh pr merge <N> --disable-auto` is allowed. Nonzero exit means inspect/defer, not retry blindly. Cleanup warnings after a successful merge do not justify retrying; reconcile ambiguous results with GitHub state. Never delete fork branches through the base repository's `origin`.

## Pick an issue

Ensure `in-progress` exists, then list open, unassigned candidates without that label. Skip `wontfix`, `duplicate`, `question`, `needs-human`, `on-hold`, claimed items, issues with open native sub-issues or open `blocked_by` dependencies, and issues with an open closing PR. Prefer issues that block other issues, then clear, smaller tasks with fewer discussion comments; size alone is not a reason to stop when larger coherent work remains.

Under the issue lock, re-fetch state, assignees, labels, native dependencies, and linked PRs. Skip closed, assigned, or actively claimed items. Work an open blocker instead of a blocked item, and close an already implemented item with evidence, as described in [Resolve, do not defer](#resolve-do-not-defer). An `in-progress` claim with no open PR and no lock holder is stale: look for its branch or worktree and publish that work, or remove the claim and implement the issue. Add `in-progress` and verify the claim before branching. Keep it while the PR is open. Remove only your own claim if abandoning before PR creation; after lost ownership, leave shared claim state untouched and report the loss.

Establish full scope from source, tests, and docs; a missing API may be the requested work. Deliver coherent issues in one PR.

For independent deliverables, inspect existing children (including closed ones) before splitting into native sub-issues and `blocked_by` relationships. Avoid checklist-only tracking and `blocked` labels. Split only into parts that each merge on their own as one PR, and never below one level: a child is not split again. When the issue is already a child, deliver it in one PR, or move what is left into sibling issues of the same parent. Do not file a child whose only work is to wait for, re-run, or re-validate another child's output. Give children acceptance criteria, leave the parent open, remove your parent claim, and implement the first unclaimed child whose prerequisites are closed this iteration. If native links are unavailable, document the blocker.

Relationship writes require numeric REST issue `.id`, not issue numbers or `gh issue view --json id` (GraphQL ID). Retrieve with `gh api "repos/$repoSlug/issues/$childNumber" --jq .id`; link via `POST issues/<parent>/sub_issues -F sub_issue_id=<id>` or `POST issues/<blocked>/dependencies/blocked_by -F issue_id=<prerequisite-id>` under `repos/$repoSlug/`. Verify each write before proceeding.

## Implement and ship

Follow local validation, resource limits, target frameworks, generated-code boundaries, and performance requirements. Use TDD for bugs/behavior changes where feasible; explain exceptions. Run focused and relevant broader checks, including docs/frontend checks when affected. Document unavailable validation; never silently raise guard limits or claim unrun tests passed.

Review the full diff for correctness, reuse, and efficiency (`/simplify` when available, otherwise equivalent review). Preserve performance/contracts and batch related fixes into one push. Use CI for environment-specific validation with limitations documented; avoid arbitrary local debugging retry counts.

- Use non-interactive commit messages (`-m`, `--file`, or `--amend --no-edit`) and disable editors for rebases and continuation: `git -c core.editor=true -c sequence.editor=true rebase ...`.
- Reference the issue in commits. Open draft PRs and follow [Draft review lifecycle](#draft-review-lifecycle). Follow the repository PR template and use `--body-file` for multiline descriptions. Put `Closes #<N>` on its own line only when the full issue is delivered.
- Use ordinary pushes for new commits and `git push --force-with-lease` when rewriting an owned PR branch. Never use bare `--force` or rewrite main. Confirm ownership and the remote PR head before reporting completion.
- Completion requires commit, successful push, PR creation/update, and `gh pr view <N> --json headRefOid` matching local `git rev-parse HEAD`. Push/ownership failure means preserve work, report failure, release ownership, and continue. Keep committed work on its `issue-<N>-*` branch: the driver finds unpublished branches and assigns them for publishing. Release locks in every outcome and stop owned services when no longer needed.

Record unrelated bugs as linked issues only within authorization; finish the original scope.
