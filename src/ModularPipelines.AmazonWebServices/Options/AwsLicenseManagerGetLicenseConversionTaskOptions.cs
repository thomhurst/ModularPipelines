using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "get-license-conversion-task")]
public record AwsLicenseManagerGetLicenseConversionTaskOptions(
[property: CommandSwitch("--license-conversion-task-id")] string LicenseConversionTaskId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}