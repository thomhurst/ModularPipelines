using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("services", "operations", "describe")]
public record GcloudServicesOperationsDescribeOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions
{
    [CommandSwitch("--full")]
    public string? Full { get; set; }
}