using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "content", "describe")]
public record GcloudDataplexContentDescribeOptions(
[property: PositionalArgument] string Content,
[property: PositionalArgument] string Lake,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--view")]
    public string? View { get; set; }
}