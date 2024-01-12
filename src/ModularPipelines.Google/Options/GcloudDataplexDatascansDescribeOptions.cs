using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "datascans", "describe")]
public record GcloudDataplexDatascansDescribeOptions(
[property: PositionalArgument] string Datascan,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--view")]
    public string? View { get; set; }
}