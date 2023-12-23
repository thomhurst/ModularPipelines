using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "extend-license-consumption")]
public record AwsLicenseManagerExtendLicenseConsumptionOptions(
[property: CommandSwitch("--license-consumption-token")] string LicenseConsumptionToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}