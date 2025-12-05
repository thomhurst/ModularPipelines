using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "dataset", "show")]
public record AzMlDatasetShowOptions : AzOptions
{
    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}