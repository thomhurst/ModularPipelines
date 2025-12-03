using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "worker-pools", "update")]
public record GcloudBuildsWorkerPoolsUpdateOptions(
[property: CliArgument] string WorkerPool,
[property: CliOption("--region")] string Region,
[property: CliOption("--config-from-file")] string ConfigFromFile,
[property: CliOption("--worker-disk-size")] string WorkerDiskSize,
[property: CliOption("--worker-machine-type")] string WorkerMachineType,
[property: CliFlag("--public-egress")] bool PublicEgress,
[property: CliFlag("--no-public-egress")] bool NoPublicEgress
) : GcloudOptions;