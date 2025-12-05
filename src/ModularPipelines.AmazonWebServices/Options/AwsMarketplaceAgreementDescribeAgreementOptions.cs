using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("marketplace-agreement", "describe-agreement")]
public record AwsMarketplaceAgreementDescribeAgreementOptions(
[property: CliOption("--agreement-id")] string AgreementId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}