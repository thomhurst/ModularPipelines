using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "operations", "describe")]
public record GcloudAiOperationsDescribeOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--index")]
    public string? Index { get; set; }

    [CommandSwitch("--index-endpoint")]
    public string? IndexEndpoint { get; set; }
}