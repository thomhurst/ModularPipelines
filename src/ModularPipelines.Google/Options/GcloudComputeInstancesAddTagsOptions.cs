using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "add-tags")]
public record GcloudComputeInstancesAddTagsOptions(
[property: PositionalArgument] string InstanceName,
[property: CommandSwitch("--tags")] string[] Tags
) : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}