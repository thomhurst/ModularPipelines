using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr", "describe-images")]
public record AwsEcrDescribeImagesOptions(
[property: CliOption("--repository-name")] string RepositoryName
) : AwsOptions
{
    [CliOption("--registry-id")]
    public string? RegistryId { get; set; }

    [CliOption("--image-ids")]
    public string[]? ImageIds { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}