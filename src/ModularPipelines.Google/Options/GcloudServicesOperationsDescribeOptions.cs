using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("services", "operations", "describe")]
public record GcloudServicesOperationsDescribeOptions(
[property: CliArgument] string Operation
) : GcloudOptions
{
    [CliOption("--full")]
    public string? Full { get; set; }
}