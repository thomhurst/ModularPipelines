using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("osis", "create-pipeline")]
public record AwsOsisCreatePipelineOptions(
[property: CliOption("--pipeline-name")] string PipelineName,
[property: CliOption("--min-units")] int MinUnits,
[property: CliOption("--max-units")] int MaxUnits,
[property: CliOption("--pipeline-configuration-body")] string PipelineConfigurationBody
) : AwsOptions
{
    [CliOption("--log-publishing-options")]
    public string? LogPublishingOptions { get; set; }

    [CliOption("--vpc-options")]
    public string? VpcOptions { get; set; }

    [CliOption("--buffer-options")]
    public string? BufferOptions { get; set; }

    [CliOption("--encryption-at-rest-options")]
    public string? EncryptionAtRestOptions { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}