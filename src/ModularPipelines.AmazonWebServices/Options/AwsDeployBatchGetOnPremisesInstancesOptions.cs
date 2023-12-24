using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "batch-get-on-premises-instances")]
public record AwsDeployBatchGetOnPremisesInstancesOptions(
[property: CommandSwitch("--instance-names")] string[] InstanceNames
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}