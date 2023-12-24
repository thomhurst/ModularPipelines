using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-cluster")]
public record AwsSagemakerUpdateClusterOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--instance-groups")] string[] InstanceGroups
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}