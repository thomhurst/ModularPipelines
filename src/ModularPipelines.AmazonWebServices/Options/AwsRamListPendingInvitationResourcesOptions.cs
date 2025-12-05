using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ram", "list-pending-invitation-resources")]
public record AwsRamListPendingInvitationResourcesOptions(
[property: CliOption("--resource-share-invitation-arn")] string ResourceShareInvitationArn
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--resource-region-scope")]
    public string? ResourceRegionScope { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}