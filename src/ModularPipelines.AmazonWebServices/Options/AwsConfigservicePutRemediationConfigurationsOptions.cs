using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "put-remediation-configurations")]
public record AwsConfigservicePutRemediationConfigurationsOptions(
[property: CommandSwitch("--remediation-configurations")] string[] RemediationConfigurations
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}