using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("filestore", "operations", "describe")]
public record GcloudFilestoreOperationsDescribeOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Zone
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}