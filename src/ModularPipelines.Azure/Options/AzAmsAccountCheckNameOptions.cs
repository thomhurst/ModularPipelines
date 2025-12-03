using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ams", "account", "check-name")]
public record AzAmsAccountCheckNameOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}