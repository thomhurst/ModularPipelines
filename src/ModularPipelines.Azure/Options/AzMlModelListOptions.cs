using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "model", "list")]
public record AzMlModelListOptions : AzOptions
{
    [CliFlag("--archived-only")]
    public bool? ArchivedOnly { get; set; }

    [CliFlag("--include-archived")]
    public bool? IncludeArchived { get; set; }

    [CliOption("--max-results")]
    public string? MaxResults { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--registry-name")]
    public string? RegistryName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--stage")]
    public string? Stage { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}