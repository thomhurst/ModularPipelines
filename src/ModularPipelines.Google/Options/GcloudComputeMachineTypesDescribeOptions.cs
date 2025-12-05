using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "machine-types", "describe")]
public record GcloudComputeMachineTypesDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}