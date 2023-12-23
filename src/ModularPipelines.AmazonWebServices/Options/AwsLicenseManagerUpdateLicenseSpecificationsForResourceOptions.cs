using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "update-license-specifications-for-resource")]
public record AwsLicenseManagerUpdateLicenseSpecificationsForResourceOptions(
[property: CommandSwitch("--resource-arn")] string ResourceArn
) : AwsOptions
{
    [CommandSwitch("--add-license-specifications")]
    public string[]? AddLicenseSpecifications { get; set; }

    [CommandSwitch("--remove-license-specifications")]
    public string[]? RemoveLicenseSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}