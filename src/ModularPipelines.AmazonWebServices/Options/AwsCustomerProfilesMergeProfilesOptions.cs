using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "merge-profiles")]
public record AwsCustomerProfilesMergeProfilesOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--main-profile-id")] string MainProfileId,
[property: CliOption("--profile-ids-to-be-merged")] string[] ProfileIdsToBeMerged
) : AwsOptions
{
    [CliOption("--field-source-profile-ids")]
    public string? FieldSourceProfileIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}