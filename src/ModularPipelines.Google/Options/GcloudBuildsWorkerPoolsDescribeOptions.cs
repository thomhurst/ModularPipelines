using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "worker-pools", "describe")]
public record GcloudBuildsWorkerPoolsDescribeOptions(
[property: PositionalArgument] string WorkerPool,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;