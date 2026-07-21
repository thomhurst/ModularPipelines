$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'AssertPrGreenReviewHeuristics.ps1')

$cases = @(
    @{
        Name = 'blocks correctness heading'
        Body = @'
## Review

### Correctness / Design — pool resize drops cached items
This needs a fix.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks numbered finding heading'
        Body = @'
## Review

### 1. `MetadataManager.cs` — shared MetadataManager leaks a background loop
Failure scenario follows.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks before-merge action'
        Body = @'
## Review

No correctness or security issues found.

Two things worth addressing before merge:
- Missing direct pool coverage.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks before-merging action'
        Body = @'
## Review

This should be addressed before merging.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks prior-to-merge action'
        Body = @'
## Review

This must be fixed prior to merge.
'@
        Blocks = $true
    },
    @{
        Name = 'allows no-issue review'
        Body = @'
## Review

**Correctness:** No issues. No correctness or security issues found.

Scope check passed.
'@
        Blocks = $false
    },
    @{
        Name = 'allows explicit clear verdict marker'
        Body = @'
## Review

### Correctness
Legacy prose can mention scary words without blocking when the review posts the
machine-readable all-clear marker.

<!-- REVIEW_VERDICT: CLEAR -->
'@
        Blocks = $false
    },
    @{
        Name = 'blocks explicit blocking verdict marker'
        Body = @'
## Review

No issues found.

<!-- REVIEW_VERDICT: BLOCKING -->
'@
        Blocks = $true
    },
    @{
        Name = 'allows positive category section headings'
        Body = @'
## Review

### Correctness
No bugs found. This is a faithful refactor.

### Design / architecture
No design issues found. This is a genuine improvement, not just churn.

### Zero-allocation / hot path
No concerns.

### Test coverage
Existing tests cover the behavior.

No blocking issues.
'@
        Blocks = $false
    },
    @{
        Name = 'allows explanatory correctness section with later all-clear'
        Body = @'
## Review

### Correctness

Traced the capping logic end-to-end across the batch enumerators and pending fetch state.

- Boundary case is handled correctly when the cap lands on the final record.
- Partial-batch break resumes from the same fetch without skipping or duplicating records.
- Metrics and position flushing are idempotent under the new per-sub-batch completion.

No issues found with cancellation, disposal, or thread-safety (all mutated state is only ever touched from the single sequential consume-loop, consistent with the existing "not thread-safe by design" contract).

### Zero-allocation / hot path

No new allocations found.
'@
        Blocks = $false
    },
    @{
        Name = 'allows positive category heading verdict'
        Body = @"
## Review

### Correctness $([char]0x2014) looks right
- The core fix is correct.

No blocking issues found.
"@
        Blocks = $false
    },
    @{
        Name = 'allows verified category heading parenthetical'
        Body = @'
## Review

### Correctness (verified against code, not just description)
- Confirmed the fix runs outside the lock.

No blocking issues found.
'@
        Blocks = $false
    },
    @{
        Name = 'allows positive category section heading with colon'
        Body = @'
## Review

### Correctness:
No issues found.
'@
        Blocks = $false
    },
    @{
        Name = 'allows positive correctness section with sound fix'
        Body = @'
## Review

### Correctness
The core fix is sound: the guard now checks the right invariant.

No blocking issues.
'@
        Blocks = $false
    },
    @{
        Name = 'allows positive category heading verdict with colon'
        Body = @'
## Review

### Correctness: No issues found.
'@
        Blocks = $false
    },
    @{
        Name = 'allows positive category heading bare verdict'
        Body = @'
## Review

### Correctness / Security No bugs found.
'@
        Blocks = $false
    },
    @{
        Name = 'allows positive design architecture paragraph'
        Body = @'
## Review

### Design / Architecture
Genuine improvement, not just churn: this removes duplicate shim code.

No blocking issues.
'@
        Blocks = $false
    },
    @{
        Name = 'allows positive design conventions paragraph'
        Body = @'
## Review

### Design / CLAUDE.md conventions
- Fix is scoped to only the paths that needed it.

No blocking issues.
'@
        Blocks = $false
    },
    @{
        Name = 'allows positive design compliance paragraph'
        Body = @'
## Review

### Design / CLAUDE.md compliance
- Fix is scoped to only the paths that needed it.

No blocking issues.
'@
        Blocks = $false
    },
    @{
        Name = 'allows allocation-free design compliance paragraph'
        Body = @'
## Review

### Design / CLAUDE.md compliance
`ValidateReadableLength` is a small, allocation-free helper reused consistently.

No outstanding issues.
'@
        Blocks = $false
    },
    @{
        Name = 'allows clean unsafe and allocation wording'
        Body = @'
## Review

### Correctness
Looks good. Verified the unsafe pointer arithmetic in KafkaProtocolReader does not allocate.

### Design / CLAUDE.md compliance
This is still allocation-free and keeps the existing unsafe fast path unchanged.
'@
        Blocks = $false
    },
    @{
        Name = 'allows positive api surface category with no-issues verdict'
        Body = @'
## Review

### Design / API surface
- Block size default is updated consistently in the codec, extension, and benchmark files.
- finally/ArrayPool.Return removal is safe because the pooled buffer path is gone.

No issues to flag. This is a clean, well-tested, appropriately-scoped perf change.
'@
        Blocks = $false
    },
    @{
        Name = 'allows correctness verification bullets with global no-actionable verdict'
        Body = @'
## Review

### Correctness
- The new threshold defaults to the prior value, so existing call sites retain their behavior — verified no behavior change there.
- Threshold conversion is correct and consistent with the existing recording path.

### Tests
- The regression test exercises the reproduced range.

No actionable issues found.
'@
        Blocks = $false
    },
    @{
        Name = 'blocks api surface category defect despite no-issues verdict'
        Body = @'
## Review

### Design / API surface
- No issues found.
- However, this introduces a race condition when two callers close concurrently.

No issues to flag.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks security category despite global no-issues verdict'
        Body = @'
## Review

### Security
- The new admin endpoint builds the SQL query by concatenating the raw username parameter directly into the WHERE clause before executing it.

No issues to flag.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks correctness category despite global no-issues verdict'
        Body = @'
## Review

### Correctness
- The retry counter is decremented twice per attempt, so this exhausts retries roughly twice as fast as configured.

No issues to flag.
'@
        Blocks = $true
    },
    @{
        Name = 'allows category heading with previously flagged bugs verified fixed'
        Body = @'
## Review

### Correctness — both previously-flagged bugs verified fixed

The prior cache-key and fallback findings are now resolved on the current diff.

No new or unresolved actionable issues found in the current diff.
'@
        Blocks = $false
    },
    @{
        Name = 'allows resolved prior findings heading with itemized verification'
        Body = @'
## Review

### Correctness — both previously-flagged bugs verified fixed

**1. Dead `lastException` / unreachable fallback** — the exception filter now catches the final retriable exception, the loop completes, and `lastException` is genuinely thrown afterward. Confirmed correct.

**2. Cache key ignored `normalize`** — `_idBySchemaCache` is now keyed by `(subject, schema, normalize)`, and the effective normalize flag is threaded consistently. Confirmed correct.

Both fixes match what the prior reviews reported and were independently re-verified against the current diff.
'@
        Blocks = $false
    },
    @{
        Name = 'blocks category heading with previously flagged bugs still not fixed'
        Body = @'
## Review

### Correctness — both previously-flagged bugs still not fixed

The prior findings remain open.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks prior finding section with continuation defect'
        Body = @'
## Review

### Correctness

The prior fallback finding was verified as fixed, but a similar flaw exists in the retry path too.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks prior finding heading with continuation defect'
        Body = @'
## Review

### Correctness - prior fallback finding verified, but a similar flaw exists in the retry path
'@
        Blocks = $true
    },
    @{
        Name = 'blocks previously flagged heading with continuation defect'
        Body = @'
## Review

### Correctness - both previously-flagged bugs verified fixed, but a related flaw exists in the retry path
'@
        Blocks = $true
    },
    @{
        Name = 'allows design conventions with benign race note'
        Body = @'
## Review

### Design / CLAUDE.md conventions

- `ConfigureAwait(false)` is applied consistently on every new `await` in `src/`.
- `_activeBaseUriIndex` uses `Volatile.Read`/`Volatile.Write`; worst case under a race is an extra failover hop, not corrupted state.
- Non-blocking: minor ergonomics wart, not correctness-affecting.

### Security

No concerns.

No new or unresolved actionable issues found in the current diff.
'@
        Blocks = $false
    },
    @{
        Name = 'blocks positive phrase followed by incorrect scope'
        Body = @'
## Review

### Correctness - fix is scoped incorrectly, still misses the edge case for empty batches
'@
        Blocks = $true
    },
    @{
        Name = 'blocks genuine improvement followed by race'
        Body = @'
## Review

### Design / Architecture
Genuine improvement in the abstraction, but this introduces a new race condition when two threads call Dispose concurrently.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks sound fix followed by leak'
        Body = @'
## Review

### Correctness
The core fix is sound for the happy path, but it leaks a socket on the cancellation branch.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks allocation-free helper followed by unsafe text'
        Body = @'
## Review

### Design / CLAUDE.md compliance
Nice allocation-free helper, but it is not thread-safe and will corrupt the buffer under concurrent access.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks not-a-regression heading with real bug'
        Body = @'
## Review

### Not a regression, but this is a real bug that leaks memory
'@
        Blocks = $true
    },
    @{
        Name = 'blocks no-bugs heading followed by though deadlocks'
        Body = @'
## Review

### Correctness - no bugs found, though the retry path deadlocks under high load.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks no-bugs paragraph followed by exception'
        Body = @'
## Review

### Correctness
No bugs found. That said, this will throw a NullReferenceException whenever the batch is empty.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks no-bugs paragraph followed by leak without connector'
        Body = @'
## Review

### Correctness
No bugs found.
This leaks a socket on the retry path.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks looks-good paragraph followed by deadlock without connector'
        Body = @'
## Review

### Correctness
Looks good.

The retry path deadlocks under high load.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks wrong after positive opener'
        Body = @'
## Review

### Correctness
No issues found, however this uses the wrong offset for negative timestamps.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks use-after-free after positive opener'
        Body = @'
## Review

### Correctness
No issues found. This creates a use-after-free when the callback runs late.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks double free after positive opener'
        Body = @'
## Review

### Correctness
No bugs found. The cleanup path can double free the native handle.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks infinite loop after positive opener'
        Body = @'
## Review

### Correctness
Looks good. The parser enters an infinite loop on empty varints.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks silently drops messages after positive opener'
        Body = @'
## Review

### Correctness
No concerns. The producer silently drops messages after a retriable timeout.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks data loss after positive opener'
        Body = @'
## Review

### Correctness
No problems found. This creates data loss when compaction is enabled.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks looks-good heading with actionable paragraph'
        Body = @'
## Review

### Correctness - looks good
This introduces a deadlock when Flush and Dispose run concurrently, since both take the same lock without a timeout.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks verified heading followed by blocker text'
        Body = @'
## Review

### Correctness - verified, still broken and leaks a connection on every retry
'@
        Blocks = $true
    },
    @{
        Name = 'blocks confirmed heading followed by security vulnerability'
        Body = @'
## Review

### Correctness - confirmed, but this exposes a SQL injection vulnerability in the query builder.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks no-problems paragraph with hardcoded token'
        Body = @'
## Review

### Correctness
No problems found here, although this uses a hardcoded, guessable session token.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks confirmed correct with stack overflow'
        Body = @'
## Review

### Correctness - confirmed correct, but there is a stack overflow with deeply nested input.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks no-issues heading with hangs forever'
        Body = @'
## Review

### Correctness - no issues found, however this hangs forever if the queue is empty.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks allocation noun in positive design section'
        Body = @'
## Review

### Design
Genuine improvement, not just churn. However this violates the zero-allocation rule with a per-message array allocation.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks multiline bullet after no-bugs opener'
        Body = @'
## Review

### Correctness
- No bugs found.
- However, this hangs forever if the queue is empty.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks later paragraph after positive opener'
        Body = @'
## Review

### Design / Architecture
Genuine improvement, not just churn.

But this introduces a new race condition when two threads call Dispose concurrently.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks vulnerable adjective'
        Body = @'
## Review

### Correctness
No bugs found, but this code is vulnerable to a timing attack.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks regression after positive opener'
        Body = @'
## Review

### Correctness
No concerns here, but this is a regression from the previous behavior.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks duplicated logic after positive opener'
        Body = @'
## Review

### Correctness
No bugs found, but the retry logic is now duplicated across three call sites.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks swallowed exception after positive opener'
        Body = @'
## Review

### Correctness
No issues found, though this silently swallows the exception.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks positive heading verdict followed by design risk'
        Body = @'
## Review

### Correctness - Looks good, though this is a design risk for concurrent readers.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks positive heading verdict followed by release blocker'
        Body = @'
## Review

### Correctness - Looks good, but this is a blocker for release.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks positive heading verdict followed by coverage gap'
        Body = @'
## Review

### Correctness - No issues found, but this leaves a test coverage gap.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks positive paragraph followed by FIXME crash'
        Body = @'
## Review

### Correctness
Looks good overall and the refactor is clean.
FIXME this crashes on empty input.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks trailing no-concerns after finding'
        Body = @'
## Review

### Correctness
This skips validation entirely for negative offsets, no concerns.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks leaking inflection after positive opener'
        Body = @'
## Review

### Correctness
No bugs found, though the connection pool is leaking sockets under load.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks allocate base verb after positive opener'
        Body = @'
## Review

### Correctness
No bugs found, but this will allocate per message in the hot path.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks hanging inflection after positive opener'
        Body = @'
## Review

### Correctness
No bugs found, but the shutdown path is hanging under cancellation.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks non-idempotent positive opener finding'
        Body = @'
## Review

### Correctness
Looks good, but the retry logic is not idempotent and can double-charge.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks never-disposed positive opener finding'
        Body = @'
## Review

### Correctness
Looks good, but the connection is never disposed.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks throws-exception positive opener finding'
        Body = @'
## Review

### Correctness
No issues found, but this throws an exception on empty input.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks credential-exposure positive opener finding'
        Body = @'
## Review

### Correctness
Looks good, but this exposes credentials in the log output.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks stale-cache positive opener finding'
        Body = @'
## Review

### Correctness
No bugs found. The cache entry can go stale and never refresh.
'@
        Blocks = $true
    },
    @{
        Name = 'allows live protocol clean review shape'
        Body = @'
## Review

### Correctness / Security
No bugs found. `ValidateReadableLength` is invoked before every allocation site touched by this PR, including branches that previously had no bounds check at all. Confirmed the boundary condition is correct and there is no overflow risk.

### Design / CLAUDE.md compliance
`ValidateReadableLength` is a small, allocation-free helper reused consistently across every call site.
'@
        Blocks = $false
    },
    @{
        Name = 'allows live admin compliance clean review shape'
        Body = @'
## Review

### CLAUDE.md compliance
- `ConfigureAwait(false)` is used consistently on all new/modified `await` calls in `src/`.
- This is admin/control-plane code, not a hot path per the project's performance guidelines, so the zero-allocation rules don't apply here — no concerns.
'@
        Blocks = $false
    },
    @{
        Name = 'blocks category parenthetical finding'
        Body = @'
## Review

### Correctness (pool resize drops cached items)
This needs a fix.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks category bare finding'
        Body = @'
## Review

### Correctness / Security leaks buffers
This needs a fix.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks design conventions finding'
        Body = @'
## Review

### Design / CLAUDE.md conventions
This introduces a broad abstraction that should be fixed.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks category section heading with finding body'
        Body = @'
## Review

### Correctness
`MetadataManager.RefreshAsync` leaks the cached `Task` when the request is
cancelled, causing a slow unbounded memory leak in long-running consumers.

### Design
The retry loop duplicates logic already present in `BackoffPolicy` and
should be consolidated.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks category section when all-clear is not first paragraph'
        Body = @'
## Review

### Correctness
This leaks cached tasks under cancellation.

No issues found after that.
'@
        Blocks = $true
    },
    @{
        Name = 'allows optional follow-up'
        Body = @'
## Review

No issues found.

Minor/optional, not blocking:
- Worth a follow-up later, but not required for this PR.
'@
        Blocks = $false
    },
    @{
        Name = 'allows not a regression heading'
        Body = @'
## Review

### Not a regression, just noting scope

This matches pre-PR behavior.
'@
        Blocks = $false
    },
    @{
        Name = 'allows numbered non-blocking heading'
        Body = @'
## Review

### 1. Non-blocking: rename variable
This is optional.
'@
        Blocks = $false
    },
    @{
        Name = 'allows previously flagged issues verified fixed heading'
        Body = @'
## Review

### Previously-flagged issues - verified fixed

The prior findings are now resolved.
'@
        Blocks = $false
    },
    @{
        Name = 'allows fix resolves issue heading'
        Body = @'
## Review

### Fix in `62546f5` resolves the concurrency issue

The new commit avoids the unsafe queue path.
'@
        Blocks = $false
    },
    @{
        Name = 'allows merge correctness verified clean heading'
        Body = @'
## Review

### Merge correctness — verified clean

Conflict resolution kept both feature sets.
'@
        Blocks = $false
    },
    @{
        Name = 'allows regression fixed in commit heading'
        Body = @'
## Review

### Self-triggering `UnknownTopicOrPartition` regression (from review #1) - fixed in `bb0f437f`

The regression is covered.
'@
        Blocks = $false
    },
    @{
        Name = 'allows test coverage gap closed heading'
        Body = @'
## Review

### Test coverage — gap closed

The missing tests are now present.
'@
        Blocks = $false
    },
    @{
        Name = 'blocks test coverage gap closed heading with trailing vulnerability'
        Body = @'
## Review

### Test coverage gap closed. Also this endpoint is vulnerable to SQL injection
'@
        Blocks = $true
    },
    @{
        Name = 'blocks test coverage gap closed heading with trailing defect'
        Body = @'
## Review

### Test coverage gap closed - deadlock still present in flush path
'@
        Blocks = $true
    },
    @{
        Name = 'blocks previously flagged issue still not fixed'
        Body = @'
## Review

### Previously-flagged issue - still not fixed

The finding remains open.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks fix still not resolving issue heading'
        Body = @'
## Review

### Fix still does not resolve the concurrency issue

The finding remains open.
'@
        Blocks = $true
    },
    @{
        Name = 'blocks previously flagged concerns not yet resolved'
        Body = @'
## Review

### Previously flagged concerns, not yet resolved

The finding remains open.
'@
        Blocks = $true
    }
)

$cases += @{
    Name = 'blocks long resolved-like heading without catastrophic backtracking'
    Body = "## Review`n`n### $(('x' * 54000)) issue fixed"
    Blocks = $true
}

$positivePrefixes = @(
    'No bugs found.'
    'The core fix is sound:'
    'Looks good.'
    'Verified against code.'
)
$actionableContinuations = @(
    'Though the retry path deadlocks under high load.'
    'That said, this throws a NullReferenceException when the batch is empty.'
    'But the hot path boxes the partition key per record.'
    'However, the new index math has an off-by-one error.'
)

for ($prefixIndex = 0; $prefixIndex -lt $positivePrefixes.Count; $prefixIndex++) {
    for ($continuationIndex = 0; $continuationIndex -lt $actionableContinuations.Count; $continuationIndex++) {
        $cases += @{
            Name = "blocks generated positive prefix $prefixIndex with actionable continuation $continuationIndex"
            Body = @"
## Review

### Correctness
$($positivePrefixes[$prefixIndex]) $($actionableContinuations[$continuationIndex])
"@
            Blocks = $true
        }
    }
}

foreach ($case in $cases) {
    $reason = Get-ActionableReviewBodyReason -Body $case.Body
    $blocked = -not [string]::IsNullOrWhiteSpace($reason)
    if ($blocked -ne $case.Blocks) {
        throw "Case '$($case.Name)' expected Blocks=$($case.Blocks), got Blocks=$blocked reason='$reason'"
    }
}

$staleReviewCases = @(
    @{
        Name = 'ignores stale Claude review after current-head review check'
        Review = [pscustomobject]@{
            submittedAt = '2026-07-06T01:13:33Z'
            author = [pscustomobject]@{ login = 'claude' }
        }
        HeadCommitCommittedAt = '2026-07-06T01:39:24Z'
        Checks = @([pscustomobject]@{
            name = 'claude-review'
            status = 'COMPLETED'
            conclusion = 'SUCCESS'
            completedAt = '2026-07-06T01:40:48Z'
        })
        Ignored = $true
    },
    @{
        Name = 'keeps stale Claude review before current-head review check'
        Review = [pscustomobject]@{
            submittedAt = '2026-07-06T01:13:33Z'
            author = [pscustomobject]@{ login = 'claude' }
        }
        HeadCommitCommittedAt = '2026-07-06T01:39:24Z'
        Checks = @([pscustomobject]@{
            name = 'claude-review'
            status = 'COMPLETED'
            conclusion = 'SUCCESS'
            completedAt = '2026-07-06T01:20:00Z'
        })
        Ignored = $false
    },
    @{
        Name = 'keeps human stale review'
        Review = [pscustomobject]@{
            submittedAt = '2026-07-06T01:13:33Z'
            author = [pscustomobject]@{ login = 'alice' }
        }
        HeadCommitCommittedAt = '2026-07-06T01:39:24Z'
        Checks = @([pscustomobject]@{
            name = 'claude-review'
            status = 'COMPLETED'
            conclusion = 'SUCCESS'
            completedAt = '2026-07-06T01:40:48Z'
        })
        Ignored = $false
    },
    @{
        Name = 'keeps current Claude review'
        Review = [pscustomobject]@{
            submittedAt = '2026-07-06T01:41:00Z'
            author = [pscustomobject]@{ login = 'claude[bot]' }
        }
        HeadCommitCommittedAt = '2026-07-06T01:39:24Z'
        Checks = @([pscustomobject]@{
            name = 'claude-review'
            status = 'COMPLETED'
            conclusion = 'SUCCESS'
            completedAt = '2026-07-06T01:40:48Z'
        })
        Ignored = $false
    }
)

foreach ($case in $staleReviewCases) {
    $ignored = Test-StaleBotReviewCanBeIgnored -Review $case.Review -HeadCommitCommittedAt $case.HeadCommitCommittedAt -Checks $case.Checks
    if ($ignored -ne $case.Ignored) {
        throw "Case '$($case.Name)' expected Ignored=$($case.Ignored), got Ignored=$ignored"
    }
}

Write-Host "OK review heuristic tests passed ($($cases.Count) body cases, $($staleReviewCases.Count) stale review cases)."
