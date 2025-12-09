using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "delete-profile-object")]
public record AwsCustomerProfilesDeleteProfileObjectOptions(
[property: CliOption("--profile-id")] string ProfileId,
[property: CliOption("--profile-object-unique-key")] string ProfileObjectUniqueKey,
[property: CliOption("--object-type-name")] string ObjectTypeName,
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}