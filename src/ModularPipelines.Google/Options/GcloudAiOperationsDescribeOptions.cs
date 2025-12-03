using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "operations", "describe")]
public record GcloudAiOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--index")]
    public string? Index { get; set; }

    [CliOption("--index-endpoint")]
    public string? IndexEndpoint { get; set; }
}