using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("discovery", "start-batch-delete-configuration-task")]
public record AwsDiscoveryStartBatchDeleteConfigurationTaskOptions(
[property: CliOption("--configuration-type")] string ConfigurationType,
[property: CliOption("--configuration-ids")] string[] ConfigurationIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}