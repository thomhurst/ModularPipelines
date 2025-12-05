using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "update-security-hub-configuration")]
public record AwsSecurityhubUpdateSecurityHubConfigurationOptions : AwsOptions
{
    [CliOption("--control-finding-generator")]
    public string? ControlFindingGenerator { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}