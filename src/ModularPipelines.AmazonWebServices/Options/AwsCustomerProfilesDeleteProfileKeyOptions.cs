using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "delete-profile-key")]
public record AwsCustomerProfilesDeleteProfileKeyOptions(
[property: CliOption("--profile-id")] string ProfileId,
[property: CliOption("--key-name")] string KeyName,
[property: CliOption("--values")] string[] Values,
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}