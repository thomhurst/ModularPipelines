using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "list-model-packages")]
public record AwsSagemakerListModelPackagesOptions : AwsOptions
{
    [CliOption("--creation-time-after")]
    public long? CreationTimeAfter { get; set; }

    [CliOption("--creation-time-before")]
    public long? CreationTimeBefore { get; set; }

    [CliOption("--name-contains")]
    public string? NameContains { get; set; }

    [CliOption("--model-approval-status")]
    public string? ModelApprovalStatus { get; set; }

    [CliOption("--model-package-group-name")]
    public string? ModelPackageGroupName { get; set; }

    [CliOption("--model-package-type")]
    public string? ModelPackageType { get; set; }

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