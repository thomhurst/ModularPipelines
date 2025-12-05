using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ram", "list-permission-associations")]
public record AwsRamListPermissionAssociationsOptions : AwsOptions
{
    [CliOption("--permission-arn")]
    public string? PermissionArn { get; set; }

    [CliOption("--permission-version")]
    public int? PermissionVersion { get; set; }

    [CliOption("--association-status")]
    public string? AssociationStatus { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--feature-set")]
    public string? FeatureSet { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}