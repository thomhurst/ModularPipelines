# Agent work-item locks

`AgentLocks.ps1` uses Redis as the sole authority for work-item ownership. All workers
that can update the same repository must use the same Redis deployment. The default
`local` mode uses a Docker Redis container on the worker's own machine, so it
coordinates only workers on that machine. Workers on several machines must all opt
into `shared` mode, which requires an explicit endpoint and never falls back to Docker
or another backend. It connects directly from PowerShell 7; Docker is not required on
shared workers.

## Shared deployment

Shared mode is opt-in. Provision one durable Redis 7 instance reachable by every worker. Use one stable DNS
name and port on all workers. Configure append-only persistence and a `noeviction`
policy. Do not independently provision one Redis instance per machine, use a
load-balanced endpoint backed by independent servers, or switch endpoints while
workers are active. Redis asynchronous failover or loss of acknowledged writes can
lose leases; stop workers before planned failover and reconcile ownership after an
unexpected loss. This is a lease protocol, not fencing at the GitHub API.

Configure every worker's environment before starting `Start-IssuePrLoop.ps1`:

```powershell
$env:MODULARPIPELINES_AGENT_REDIS_MODE = 'shared'
$env:MODULARPIPELINES_AGENT_REDIS_ENDPOINT = 'rediss://locks.example.internal:6379'
$env:MODULARPIPELINES_AGENT_REDIS_USERNAME = 'agent-workers' # optional Redis ACL user
# Supply MODULARPIPELINES_AGENT_REDIS_PASSWORD through the worker's secret environment.
pwsh scripts/AgentLocks.ps1 ping
```

The endpoint accepts `redis://host:port` or `rediss://host:port` (default port 6379).
It uses database 0 and does not accept credentials, a database path, query parameters,
or fragments in the URI. `rediss` validates the server certificate against the worker's
normal trust store and verifies its hostname. There is no certificate-validation bypass.
Use `redis` only on a trusted, isolated network. Restrict network access to workers;
do not expose an unauthenticated Redis port to the Internet. Credentials travel in
the connection, never in process arguments or diagnostics. Authentication and server
errors fail closed, as do connect, TLS, read and write failures. Network operations
have five-second timeouts and do not retry against a different backend.

The optional `MODULARPIPELINES_AGENT_REDIS_MODE` defaults to `local`. In shared mode a
missing or invalid endpoint exits 2 before accessing Redis or Docker. `ping` is the startup check;
it must succeed before assigning work. All verbs, including deferral recording and
batch lookup, use the same configured backend. Connection failures exit 1; status
never reports `FREE` when the backend is unavailable. Existing `HELD` (3), `LOST` (4),
and `STALE` (5) results and token-checked Lua operations remain unchanged.

The Redis keys remain under `modularpipelines:agent-lock:*`,
`modularpipelines:agent-lock-meta:*`, and `modularpipelines:agent-defer:*`. Reserve this
namespace for this repository. Grant the worker ACL access to these keys and the
commands used by the script (`PING`, `AUTH`, `EVAL`, `GET`, `SET`, `EXPIRE`, `DEL`).
Token caches remain local to each worker and owner. Shared-mode caches are additionally
scoped to the normalized endpoint, so a token from local mode or another endpoint
cannot authorize renewal or release. Keep endpoint spelling consistent across verbs.
Workers must have distinct owner IDs or independent token-cache filesystems.

## Local mode (default)

For development or workers confined to one machine and one Docker daemon, leave both
variables unset:

```powershell
pwsh scripts/AgentLocks.ps1 ping
```

This uses the `modularpipelines-agent-locks-redis` container and its append-only
volume. It provides exclusion only among callers using that Docker daemon. It does
**not** coordinate separate machines or Docker contexts. Never combine local workers
with shared workers for the same repository. An endpoint without
`MODULARPIPELINES_AGENT_REDIS_MODE=shared` is rejected rather than silently ignored.
Read-only status does not create or start local infrastructure. Other verbs retain
the existing container initialization behavior. Do not stop a shared lock container
as part of worktree cleanup.

## Migration

Stop assignment and drain every active worker before upgrading the canonical script
on every machine. Release existing locks using the old script and backend first;
never copy cached tokens, switch a running unit's endpoint, or flush live Redis keys.
Configure the common shared endpoint, verify `ping` from each machine, and perform
a contention check with a fresh test lock: acquire on one worker, verify acquisition
on another returns `HELD` with exit 3, then release on the owner. Only then resume
assignment. Every verb must invoke the canonical shared-checkout script, including
from older worktrees. Workers still in local mode must not run alongside shared
workers.

## Regression checks

```powershell
pwsh -NoProfile -File scripts/Test-AgentLockSharedRedis.ps1
pwsh -NoProfile -File scripts/Test-AgentLockStatus.ps1
pwsh -NoProfile -File scripts/Test-AgentLockTokenOwnership.ps1
pwsh -NoProfile -File scripts/Test-AgentLocks.ps1
pwsh -NoProfile -File scripts/Test-ReleasedWorktreeCleanup.ps1
```

The shared suite starts its own authenticated Docker Redis on a random loopback
port and removes only that fixture. Two subprocess workers use independent cache
directories and reject every Docker command, exercising the same remote transport
used across machines. It checks contention, ownership loss, token-checked renewal
and release, backend cache isolation, Unicode replies, authentication failures and
unavailability. It does not require or modify a deployed coordination service.
The older suites explicitly select local mode for their Docker fixtures.
