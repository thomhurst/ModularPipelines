using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock", "list-custom-models")]
public record AwsBedrockListCustomModelsOptions : AwsOptions
{
    [CliOption("--creation-time-before")]
    public long? CreationTimeBefore { get; set; }

    [CliOption("--creation-time-after")]
    public long? CreationTimeAfter { get; set; }

    [CliOption("--name-contains")]
    public string? NameContains { get; set; }

    [CliOption("--base-model-arn-equals")]
    public string? BaseModelArnEquals { get; set; }

    [CliOption("--foundation-model-arn-equals")]
    public string? FoundationModelArnEquals { get; set; }

    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--sort-order")]
    public string? SortOrder { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}