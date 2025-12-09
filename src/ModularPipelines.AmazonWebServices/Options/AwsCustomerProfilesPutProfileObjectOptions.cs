using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "put-profile-object")]
public record AwsCustomerProfilesPutProfileObjectOptions(
[property: CliOption("--object-type-name")] string ObjectTypeName,
[property: CliOption("--object")] string Object,
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}