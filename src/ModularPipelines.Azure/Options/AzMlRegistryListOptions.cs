using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "registry", "list")]
public record AzMlRegistryListOptions : AzOptions
{
    [CliOption("--max-results")]
    public string? MaxResults { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}