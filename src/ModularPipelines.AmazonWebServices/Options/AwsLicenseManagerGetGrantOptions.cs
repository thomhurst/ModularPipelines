using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "get-grant")]
public record AwsLicenseManagerGetGrantOptions(
[property: CommandSwitch("--grant-arn")] string GrantArn
) : AwsOptions
{
    [CommandSwitch("--grant-version")]
    public string? GrantVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}