using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "delete-profile-object-type")]
public record AwsCustomerProfilesDeleteProfileObjectTypeOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--object-type-name")] string ObjectTypeName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}