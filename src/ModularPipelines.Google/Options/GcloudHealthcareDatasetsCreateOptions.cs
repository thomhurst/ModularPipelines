using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "datasets", "create")]
public record GcloudHealthcareDatasetsCreateOptions(
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--time-zone")]
    public string? TimeZone { get; set; }
}