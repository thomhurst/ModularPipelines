using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "tags", "bindings", "create")]
public record GcloudResourceManagerTagsBindingsCreateOptions(
[property: CommandSwitch("--parent")] string Parent,
[property: CommandSwitch("--tag-value")] string TagValue
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}