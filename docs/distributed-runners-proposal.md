# Distributed GitHub Actions Runners — Proposal

## Problem

Run a C# application across multiple GitHub Actions runners concurrently using a matrix strategy, where one instance becomes the master and the others become workers. The master delegates work and orchestrates everything, parallelising tasks and speeding up execution.

## Core Challenge

GitHub-hosted runners are isolated, ephemeral VMs with no shared network. There is no built-in discovery mechanism or runner-to-runner communication.

## Recommended Approach: Upstash Redis as Coordination Layer

Use a free Upstash Redis instance as a message broker between runners. This removes the need for direct network connectivity, tunnels, or VPNs.

### Why Upstash Redis

- **Free tier**: 10,000 commands/day, 256MB storage — more than enough for CI coordination
- **Serverless**: no VM to manage, always on, no idle cost
- **REST API**: works without a Redis client library (just HTTP calls)
- **Standard Redis protocol**: also works with `StackExchange.Redis` if preferred
- Setup: sign up at upstash.com, create a database, store connection string as GitHub secret

### Architecture

```
┌────────────────────────────────────────────────┐
│  GitHub Actions Matrix                         │
│                                                │
│  Runner 0 (master)                             │
│    ├── Pushes work to Redis queue              │
│    ├── Reads results from Redis                │
│    └── Publishes completion signal             │
│                                                │
│  Runner 1..N (workers)                         │
│    ├── Pop work from Redis queue               │
│    ├── Execute work                            │
│    └── Push results back to Redis              │
│                                                │
│           ▼           ▲                        │
│     ┌─────────────────────┐                    │
│     │   Upstash Redis     │                    │
│     │   (free tier)       │                    │
│     │                     │                    │
│     │  work:queue         │                    │
│     │  results:queue      │                    │
│     │  master:status      │                    │
│     └─────────────────────┘                    │
└────────────────────────────────────────────────┘
```

### Benefits Over Tunnel-Based Approaches

- No tunnel setup or URL sharing needed
- Redis handles all coordination
- Workers and master don't need direct connectivity
- If a worker dies, its work item stays in the queue — another worker can pick it up
- Built-in pub/sub if real-time signaling is needed

### GitHub Actions Workflow

```yaml
jobs:
  pipeline:
    strategy:
      matrix:
        instance: [0, 1, 2, 3]
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'

      - name: Run
        env:
          UPSTASH_REDIS_URL: ${{ secrets.UPSTASH_REDIS_URL }}
          UPSTASH_REDIS_TOKEN: ${{ secrets.UPSTASH_REDIS_TOKEN }}
        run: |
          dotnet run --project src/YourApp -- \
            --instance=${{ matrix.instance }} \
            --total=4
```

### C# Implementation — Role Selection

```csharp
var instance = int.Parse(args["--instance"]);
if (instance == 0)
    await RunAsMaster(totalWorkers);
else
    await RunAsWorker();
```

### C# Implementation — Using StackExchange.Redis

```csharp
// Using StackExchange.Redis (works with Upstash)
var redis = ConnectionMultiplexer.Connect(connectionString);
var db = redis.GetDatabase();

// Master registers itself
await db.StringSetAsync("master:address", "ready");

// Master publishes work via a list
await db.ListRightPushAsync("work:queue", serializedWorkItem);

// Workers pop work
var work = await db.ListLeftPopAsync("work:queue");

// Workers push results back
await db.ListRightPushAsync("results:queue", serializedResult);
```

### C# Implementation — Using Upstash REST API (No Redis Client Needed)

```csharp
var http = new HttpClient();
http.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", upstashToken);

// SET
await http.PostAsync($"{upstashUrl}/set/master:address/ready", null);

// GET
var response = await http.GetStringAsync($"{upstashUrl}/get/master:address");

// LPUSH (add work)
await http.PostAsync($"{upstashUrl}/lpush/work:queue/{serializedWorkItem}", null);

// RPOP (take work)
var work = await http.GetStringAsync($"{upstashUrl}/rpop/work:queue");
```

## Alternative Free Approaches

### 1. Tailscale Mesh VPN (Free for personal use)

- Free plan: up to 100 devices
- [tailscale/github-action](https://github.com/tailscale/github-action) sets it up in CI
- All runners join the same tailnet and get full IP connectivity
- Communicate via gRPC, HTTP, raw TCP, etc.
- Requires a Tailscale account + OAuth client

### 2. Cloudflare Quick Tunnel (Free, zero accounts needed)

- `cloudflared tunnel --url http://localhost:5000` gives a `*.trycloudflare.com` URL
- No Cloudflare account needed for quick tunnels
- Master creates tunnel, shares URL via GitHub Actions Cache or `gh variable`
- Workers connect to that URL
- Drawback: need to coordinate URL sharing between matrix jobs

### 3. GitHub Actions Cache as Coordination (Free, built-in)

- 10 GB free per repo
- Master writes address/work to cache keys, workers poll for them
- High latency (~2-5s per operation), but workable for coarse-grained coordination
- No external accounts needed at all

### 4. Other Free Redis Hosts

| Provider | Free Tier | Notes |
|----------|-----------|-------|
| Redis Cloud | 30MB, 1 database | Standard Redis protocol |
| Aiven | Small instance | Valkey (Redis-compatible) |
| Railway | $5 credit/month | One-click Redis deploy |
| Render | 25MB | Expires after 90 days |

## Important Caveats

1. **Matrix jobs don't start simultaneously** — GitHub may queue some runners if capacity is tight. Workers need to handle waiting for the master, and the master needs to handle workers arriving at different times.

2. **Runner reliability** — GitHub-hosted runners can be preempted. Build in health checks, timeouts, and retry logic. Design work items to be idempotent so they can be safely re-queued.

3. **6-hour job timeout** — GitHub Actions jobs have a maximum runtime of 6 hours. Plan workloads accordingly.

4. **Secrets management** — Store Upstash credentials (or Tailscale OAuth tokens) as GitHub repository secrets. Never hardcode them.

## GitHub Secrets Needed

For the recommended Upstash approach, only two secrets:
- `UPSTASH_REDIS_URL` — the REST API URL from Upstash dashboard
- `UPSTASH_REDIS_TOKEN` — the REST API token from Upstash dashboard
