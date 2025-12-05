using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "list-profile-objects")]
public record AwsCustomerProfilesListProfileObjectsOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--object-type-name")] string ObjectTypeName,
[property: CliOption("--profile-id")] string ProfileId
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--object-filter")]
    public string? ObjectFilter { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}