using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "worker-pools", "delete")]
public record GcloudBuildsWorkerPoolsDeleteOptions(
[property: PositionalArgument] string WorkerPool,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;