using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "datasets", "update")]
public record GcloudHealthcareDatasetsUpdateOptions(
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--time-zone")]
    public string? TimeZone { get; set; }
}