using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "datastore", "upload")]
public record AzMlDatastoreUploadOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--src-path")] string SrcPath
) : AzOptions
{
    [CliOption("--hide-progress")]
    public string? HideProgress { get; set; }

    [CliOption("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CliOption("--overwrite")]
    public string? Overwrite { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--target-path")]
    public string? TargetPath { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}