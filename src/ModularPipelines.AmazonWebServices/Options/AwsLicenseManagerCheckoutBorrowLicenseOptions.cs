using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "checkout-borrow-license")]
public record AwsLicenseManagerCheckoutBorrowLicenseOptions(
[property: CliOption("--license-arn")] string LicenseArn,
[property: CliOption("--entitlements")] string[] Entitlements,
[property: CliOption("--digital-signature-method")] string DigitalSignatureMethod,
[property: CliOption("--client-token")] string ClientToken
) : AwsOptions
{
    [CliOption("--node-id")]
    public string? NodeId { get; set; }

    [CliOption("--checkout-metadata")]
    public string[]? CheckoutMetadata { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}