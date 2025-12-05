using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "add-tags")]
public record GcloudComputeInstancesAddTagsOptions(
[property: CliArgument] string InstanceName,
[property: CliOption("--tags")] string[] Tags
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}