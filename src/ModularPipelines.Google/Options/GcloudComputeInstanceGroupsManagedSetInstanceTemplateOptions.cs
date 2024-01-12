using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-groups", "managed", "set-instance-template")]
public record GcloudComputeInstanceGroupsManagedSetInstanceTemplateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--template")] string Template
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}