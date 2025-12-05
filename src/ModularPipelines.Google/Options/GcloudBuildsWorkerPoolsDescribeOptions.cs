using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "worker-pools", "describe")]
public record GcloudBuildsWorkerPoolsDescribeOptions(
[property: CliArgument] string WorkerPool,
[property: CliOption("--region")] string Region
) : GcloudOptions;