using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "checkout-borrow-license")]
public record AwsLicenseManagerCheckoutBorrowLicenseOptions(
[property: CommandSwitch("--license-arn")] string LicenseArn,
[property: CommandSwitch("--entitlements")] string[] Entitlements,
[property: CommandSwitch("--digital-signature-method")] string DigitalSignatureMethod,
[property: CommandSwitch("--client-token")] string ClientToken
) : AwsOptions
{
    [CommandSwitch("--node-id")]
    public string? NodeId { get; set; }

    [CommandSwitch("--checkout-metadata")]
    public string[]? CheckoutMetadata { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}