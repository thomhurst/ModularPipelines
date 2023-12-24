using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-cluster")]
public record AwsSagemakerCreateClusterOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--instance-groups")] string[] InstanceGroups
) : AwsOptions
{
    [CommandSwitch("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}