using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "create-license-version")]
public record AwsLicenseManagerCreateLicenseVersionOptions(
[property: CommandSwitch("--license-arn")] string LicenseArn,
[property: CommandSwitch("--license-name")] string LicenseName,
[property: CommandSwitch("--product-name")] string ProductName,
[property: CommandSwitch("--issuer")] string Issuer,
[property: CommandSwitch("--home-region")] string HomeRegion,
[property: CommandSwitch("--validity")] string Validity,
[property: CommandSwitch("--entitlements")] string[] Entitlements,
[property: CommandSwitch("--consumption-configuration")] string ConsumptionConfiguration,
[property: CommandSwitch("--status")] string Status,
[property: CommandSwitch("--client-token")] string ClientToken
) : AwsOptions
{
    [CommandSwitch("--license-metadata")]
    public string[]? LicenseMetadata { get; set; }

    [CommandSwitch("--source-version")]
    public string? SourceVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}