using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "tags", "holds", "delete")]
public record GcloudResourceManagerTagsHoldsDeleteOptions(
[property: PositionalArgument] string TagHoldName
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}