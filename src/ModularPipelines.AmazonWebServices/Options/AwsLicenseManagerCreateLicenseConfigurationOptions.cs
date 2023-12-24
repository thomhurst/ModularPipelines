using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "create-license-configuration")]
public record AwsLicenseManagerCreateLicenseConfigurationOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--license-counting-type")] string LicenseCountingType,
[property: CommandSwitch("--license-count")] long LicenseCount
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--license-rules")]
    public string[]? LicenseRules { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--product-information-list")]
    public string[]? ProductInformationList { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}