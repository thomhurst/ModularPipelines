using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "project-info", "set-usage-bucket")]
public record GcloudComputeProjectInfoSetUsageBucketOptions(
[property: CommandSwitch("--bucket")] string Bucket,
[property: BooleanCommandSwitch("--no-bucket")] bool NoBucket
) : GcloudOptions
{
    [CommandSwitch("--prefix")]
    public string? Prefix { get; set; }
}