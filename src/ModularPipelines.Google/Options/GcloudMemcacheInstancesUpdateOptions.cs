using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("memcache", "instances", "update")]
public record GcloudMemcacheInstancesUpdateOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--parameters")] IEnumerable<KeyValue> Parameters,
[property: BooleanCommandSwitch("listen-backlog")] bool ListenBacklog,
[property: BooleanCommandSwitch("disable-flush-all")] bool DisableFlushAll,
[property: BooleanCommandSwitch("max-item-size")] bool MaxItemSize,
[property: BooleanCommandSwitch("slab-min-size")] bool SlabMinSize,
[property: BooleanCommandSwitch("slab-growth-factor")] bool SlabGrowthFactor,
[property: BooleanCommandSwitch("protocol")] bool Protocol,
[property: BooleanCommandSwitch("disable-cas")] bool DisableCas,
[property: BooleanCommandSwitch("disable-evictions")] bool DisableEvictions,
[property: BooleanCommandSwitch("max-reqs-per-event")] bool MaxReqsPerEvent,
[property: BooleanCommandSwitch("track-sizes")] bool TrackSizes,
[property: BooleanCommandSwitch("worker-logbuf-size")] bool WorkerLogbufSize,
[property: BooleanCommandSwitch("watcher-logbuf-size")] bool WatcherLogbufSize,
[property: BooleanCommandSwitch("lru-crawler")] bool LruCrawler,
[property: BooleanCommandSwitch("idle-timeout")] bool IdleTimeout,
[property: BooleanCommandSwitch("lru-maintainer")] bool LruMaintainer,
[property: BooleanCommandSwitch("maxconns-fast")] bool MaxconnsFast,
[property: BooleanCommandSwitch("hash-algorithm")] bool HashAlgorithm,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--labels")] IEnumerable<KeyValue> Labels,
[property: CommandSwitch("--node-count")] string NodeCount,
[property: BooleanCommandSwitch("--maintenance-window-any")] bool MaintenanceWindowAny,
[property: CommandSwitch("--maintenance-window-day")] string MaintenanceWindowDay,
[property: CommandSwitch("--maintenance-window-duration")] string MaintenanceWindowDuration,
[property: CommandSwitch("--maintenance-window-start-time")] string MaintenanceWindowStartTime
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}