using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("osis", "update-pipeline")]
public record AwsOsisUpdatePipelineOptions(
[property: CliOption("--pipeline-name")] string PipelineName
) : AwsOptions
{
    [CliOption("--min-units")]
    public int? MinUnits { get; set; }

    [CliOption("--max-units")]
    public int? MaxUnits { get; set; }

    [CliOption("--pipeline-configuration-body")]
    public string? PipelineConfigurationBody { get; set; }

    [CliOption("--log-publishing-options")]
    public string? LogPublishingOptions { get; set; }

    [CliOption("--buffer-options")]
    public string? BufferOptions { get; set; }

    [CliOption("--encryption-at-rest-options")]
    public string? EncryptionAtRestOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}