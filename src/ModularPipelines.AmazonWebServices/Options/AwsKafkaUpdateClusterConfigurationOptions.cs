using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafka", "update-cluster-configuration")]
public record AwsKafkaUpdateClusterConfigurationOptions(
[property: CommandSwitch("--cluster-arn")] string ClusterArn,
[property: CommandSwitch("--configuration-info")] string ConfigurationInfo,
[property: CommandSwitch("--current-version")] string CurrentVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}