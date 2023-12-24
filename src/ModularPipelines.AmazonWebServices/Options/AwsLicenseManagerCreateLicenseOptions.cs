using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "create-license")]
public record AwsLicenseManagerCreateLicenseOptions(
[property: CommandSwitch("--license-name")] string LicenseName,
[property: CommandSwitch("--product-name")] string ProductName,
[property: CommandSwitch("--product-sku")] string ProductSku,
[property: CommandSwitch("--issuer")] string Issuer,
[property: CommandSwitch("--home-region")] string HomeRegion,
[property: CommandSwitch("--validity")] string Validity,
[property: CommandSwitch("--entitlements")] string[] Entitlements,
[property: CommandSwitch("--beneficiary")] string Beneficiary,
[property: CommandSwitch("--consumption-configuration")] string ConsumptionConfiguration,
[property: CommandSwitch("--client-token")] string ClientToken
) : AwsOptions
{
    [CommandSwitch("--license-metadata")]
    public string[]? LicenseMetadata { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}