using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("discovery", "describe-configurations")]
public record AwsDiscoveryDescribeConfigurationsOptions(
[property: CommandSwitch("--configuration-ids")] string[] ConfigurationIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}