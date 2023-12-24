using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "get-license")]
public record AwsLicenseManagerGetLicenseOptions(
[property: CommandSwitch("--license-arn")] string LicenseArn
) : AwsOptions
{
    [CommandSwitch("--license-version")]
    public string? LicenseVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}