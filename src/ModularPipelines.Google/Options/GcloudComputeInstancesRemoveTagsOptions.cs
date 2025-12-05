using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "remove-tags")]
public record GcloudComputeInstancesRemoveTagsOptions(
[property: CliArgument] string InstanceName,
[property: CliFlag("--all")] bool All,
[property: CliOption("--tags")] string[] Tags
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}