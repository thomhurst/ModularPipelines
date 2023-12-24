using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "checkout-license")]
public record AwsLicenseManagerCheckoutLicenseOptions(
[property: CommandSwitch("--product-sku")] string ProductSku,
[property: CommandSwitch("--checkout-type")] string CheckoutType,
[property: CommandSwitch("--key-fingerprint")] string KeyFingerprint,
[property: CommandSwitch("--entitlements")] string[] Entitlements,
[property: CommandSwitch("--client-token")] string ClientToken
) : AwsOptions
{
    [CommandSwitch("--beneficiary")]
    public string? Beneficiary { get; set; }

    [CommandSwitch("--node-id")]
    public string? NodeId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}