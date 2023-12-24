using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "update-license-configuration")]
public record AwsLicenseManagerUpdateLicenseConfigurationOptions(
[property: CommandSwitch("--license-configuration-arn")] string LicenseConfigurationArn
) : AwsOptions
{
    [CommandSwitch("--license-configuration-status")]
    public string? LicenseConfigurationStatus { get; set; }

    [CommandSwitch("--license-rules")]
    public string[]? LicenseRules { get; set; }

    [CommandSwitch("--license-count")]
    public long? LicenseCount { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--product-information-list")]
    public string[]? ProductInformationList { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}