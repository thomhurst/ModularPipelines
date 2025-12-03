using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "list-inference-components")]
public record AwsSagemakerListInferenceComponentsOptions : AwsOptions
{
    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--sort-order")]
    public string? SortOrder { get; set; }

    [CliOption("--name-contains")]
    public string? NameContains { get; set; }

    [CliOption("--creation-time-before")]
    public long? CreationTimeBefore { get; set; }

    [CliOption("--creation-time-after")]
    public long? CreationTimeAfter { get; set; }

    [CliOption("--last-modified-time-before")]
    public long? LastModifiedTimeBefore { get; set; }

    [CliOption("--last-modified-time-after")]
    public long? LastModifiedTimeAfter { get; set; }

    [CliOption("--status-equals")]
    public string? StatusEquals { get; set; }

    [CliOption("--endpoint-name-equals")]
    public string? EndpointNameEquals { get; set; }

    [CliOption("--variant-name-equals")]
    public string? VariantNameEquals { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}