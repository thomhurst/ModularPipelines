using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("discovery", "start-batch-delete-configuration-task")]
public record AwsDiscoveryStartBatchDeleteConfigurationTaskOptions(
[property: CommandSwitch("--configuration-type")] string ConfigurationType,
[property: CommandSwitch("--configuration-ids")] string[] ConfigurationIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}