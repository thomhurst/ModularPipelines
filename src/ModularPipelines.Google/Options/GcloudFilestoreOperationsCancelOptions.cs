using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("filestore", "operations", "cancel")]
public record GcloudFilestoreOperationsCancelOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Zone
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}