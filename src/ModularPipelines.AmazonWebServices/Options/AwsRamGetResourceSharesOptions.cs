using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ram", "get-resource-shares")]
public record AwsRamGetResourceSharesOptions(
[property: CliOption("--resource-owner")] string ResourceOwner
) : AwsOptions
{
    [CliOption("--resource-share-arns")]
    public string[]? ResourceShareArns { get; set; }

    [CliOption("--resource-share-status")]
    public string? ResourceShareStatus { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--tag-filters")]
    public string[]? TagFilters { get; set; }

    [CliOption("--permission-arn")]
    public string? PermissionArn { get; set; }

    [CliOption("--permission-version")]
    public int? PermissionVersion { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}