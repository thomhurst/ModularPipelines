using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "update-security-hub-configuration")]
public record AwsSecurityhubUpdateSecurityHubConfigurationOptions : AwsOptions
{
    [CommandSwitch("--control-finding-generator")]
    public string? ControlFindingGenerator { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}