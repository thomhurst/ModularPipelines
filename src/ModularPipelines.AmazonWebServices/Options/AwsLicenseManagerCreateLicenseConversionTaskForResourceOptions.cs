using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "create-license-conversion-task-for-resource")]
public record AwsLicenseManagerCreateLicenseConversionTaskForResourceOptions(
[property: CommandSwitch("--resource-arn")] string ResourceArn,
[property: CommandSwitch("--source-license-context")] string SourceLicenseContext,
[property: CommandSwitch("--destination-license-context")] string DestinationLicenseContext
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}