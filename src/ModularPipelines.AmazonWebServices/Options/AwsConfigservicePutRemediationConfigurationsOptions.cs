using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "put-remediation-configurations")]
public record AwsConfigservicePutRemediationConfigurationsOptions(
[property: CliOption("--remediation-configurations")] string[] RemediationConfigurations
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}