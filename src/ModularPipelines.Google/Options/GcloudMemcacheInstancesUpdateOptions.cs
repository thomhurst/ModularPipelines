using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memcache", "instances", "update")]
public record GcloudMemcacheInstancesUpdateOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Region,
[property: CliOption("--parameters")] IEnumerable<KeyValue> Parameters,
[property: CliFlag("listen-backlog")] bool ListenBacklog,
[property: CliFlag("disable-flush-all")] bool DisableFlushAll,
[property: CliFlag("max-item-size")] bool MaxItemSize,
[property: CliFlag("slab-min-size")] bool SlabMinSize,
[property: CliFlag("slab-growth-factor")] bool SlabGrowthFactor,
[property: CliFlag("protocol")] bool Protocol,
[property: CliFlag("disable-cas")] bool DisableCas,
[property: CliFlag("disable-evictions")] bool DisableEvictions,
[property: CliFlag("max-reqs-per-event")] bool MaxReqsPerEvent,
[property: CliFlag("track-sizes")] bool TrackSizes,
[property: CliFlag("worker-logbuf-size")] bool WorkerLogbufSize,
[property: CliFlag("watcher-logbuf-size")] bool WatcherLogbufSize,
[property: CliFlag("lru-crawler")] bool LruCrawler,
[property: CliFlag("idle-timeout")] bool IdleTimeout,
[property: CliFlag("lru-maintainer")] bool LruMaintainer,
[property: CliFlag("maxconns-fast")] bool MaxconnsFast,
[property: CliFlag("hash-algorithm")] bool HashAlgorithm,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--labels")] IEnumerable<KeyValue> Labels,
[property: CliOption("--node-count")] string NodeCount,
[property: CliFlag("--maintenance-window-any")] bool MaintenanceWindowAny,
[property: CliOption("--maintenance-window-day")] string MaintenanceWindowDay,
[property: CliOption("--maintenance-window-duration")] string MaintenanceWindowDuration,
[property: CliOption("--maintenance-window-start-time")] string MaintenanceWindowStartTime
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}