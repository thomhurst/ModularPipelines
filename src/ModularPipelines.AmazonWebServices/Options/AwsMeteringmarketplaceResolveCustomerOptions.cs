using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("meteringmarketplace", "resolve-customer")]
public record AwsMeteringmarketplaceResolveCustomerOptions(
[property: CliOption("--registration-token")] string RegistrationToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}