using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "remove-tags")]
public record GcloudComputeInstancesRemoveTagsOptions(
[property: PositionalArgument] string InstanceName,
[property: BooleanCommandSwitch("--all")] bool All,
[property: CommandSwitch("--tags")] string[] Tags
) : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}