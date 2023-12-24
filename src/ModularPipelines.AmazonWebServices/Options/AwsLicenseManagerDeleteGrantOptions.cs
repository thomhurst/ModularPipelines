using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "delete-grant")]
public record AwsLicenseManagerDeleteGrantOptions(
[property: CommandSwitch("--grant-arn")] string GrantArn,
[property: CommandSwitch("--grant-version")] string GrantVersion
) : AwsOptions
{
    [CommandSwitch("--status-reason")]
    public string? StatusReason { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}