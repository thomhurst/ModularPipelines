using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "checkout-license")]
public record AwsLicenseManagerCheckoutLicenseOptions(
[property: CliOption("--product-sku")] string ProductSku,
[property: CliOption("--checkout-type")] string CheckoutType,
[property: CliOption("--key-fingerprint")] string KeyFingerprint,
[property: CliOption("--entitlements")] string[] Entitlements,
[property: CliOption("--client-token")] string ClientToken
) : AwsOptions
{
    [CliOption("--beneficiary")]
    public string? Beneficiary { get; set; }

    [CliOption("--node-id")]
    public string? NodeId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}