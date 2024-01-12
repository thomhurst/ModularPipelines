using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "worker-pools", "update")]
public record GcloudBuildsWorkerPoolsUpdateOptions(
[property: PositionalArgument] string WorkerPool,
[property: CommandSwitch("--region")] string Region,
[property: CommandSwitch("--config-from-file")] string ConfigFromFile,
[property: CommandSwitch("--worker-disk-size")] string WorkerDiskSize,
[property: CommandSwitch("--worker-machine-type")] string WorkerMachineType,
[property: BooleanCommandSwitch("--public-egress")] bool PublicEgress,
[property: BooleanCommandSwitch("--no-public-egress")] bool NoPublicEgress
) : GcloudOptions;