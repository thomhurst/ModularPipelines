using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "project-info", "set-usage-bucket")]
public record GcloudComputeProjectInfoSetUsageBucketOptions(
[property: CliOption("--bucket")] string Bucket,
[property: CliFlag("--no-bucket")] bool NoBucket
) : GcloudOptions
{
    [CliOption("--prefix")]
    public string? Prefix { get; set; }
}