using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "model", "profile")]
public record AzMlModelProfileOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--base-image")]
    public string? BaseImage { get; set; }

    [CliOption("--base-image-registry")]
    public string? BaseImageRegistry { get; set; }

    [CliOption("--cc")]
    public string? Cc { get; set; }

    [CliOption("--cf")]
    public string? Cf { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--ed")]
    public string? Ed { get; set; }

    [CliOption("--entry-script")]
    public string? EntryScript { get; set; }

    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CliOption("--environment-version")]
    public string? EnvironmentVersion { get; set; }

    [CliOption("--gb")]
    public string? Gb { get; set; }

    [CliOption("--ic")]
    public string? Ic { get; set; }

    [CliOption("--idi")]
    public string? Idi { get; set; }

    [CliOption("--model")]
    public string? Model { get; set; }

    [CliOption("--model-metadata-file")]
    public string? ModelMetadataFile { get; set; }

    [CliOption("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sd")]
    public string? Sd { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CliOption("-v")]
    public string? V { get; set; }
}