using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "tags", "keys", "create")]
public record GcloudResourceManagerTagsKeysCreateOptions(
[property: PositionalArgument] string ShortName,
[property: PositionalArgument] string Parent
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--purpose")]
    public string? Purpose { get; set; }

    [CommandSwitch("--purpose-data")]
    public string[]? PurposeData { get; set; }
}