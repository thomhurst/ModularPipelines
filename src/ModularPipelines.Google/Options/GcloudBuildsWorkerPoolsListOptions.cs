using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "worker-pools", "list")]
public record GcloudBuildsWorkerPoolsListOptions(
[property: CliOption("--region")] string Region
) : GcloudOptions;