using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "worker-pools", "list")]
public record GcloudBuildsWorkerPoolsListOptions(
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;