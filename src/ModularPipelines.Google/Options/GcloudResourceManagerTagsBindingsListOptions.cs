using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "tags", "bindings", "list")]
public record GcloudResourceManagerTagsBindingsListOptions(
[property: CommandSwitch("--parent")] string Parent
) : GcloudOptions
{
    [BooleanCommandSwitch("--effective")]
    public bool? Effective { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}