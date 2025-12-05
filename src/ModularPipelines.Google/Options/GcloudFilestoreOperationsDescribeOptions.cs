using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("filestore", "operations", "describe")]
public record GcloudFilestoreOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Zone
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}