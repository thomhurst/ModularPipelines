using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "check-in-license")]
public record AwsLicenseManagerCheckInLicenseOptions(
[property: CommandSwitch("--license-consumption-token")] string LicenseConsumptionToken
) : AwsOptions
{
    [CommandSwitch("--beneficiary")]
    public string? Beneficiary { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}