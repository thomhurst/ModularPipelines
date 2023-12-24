using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("launch-wizard", "create-deployment")]
public record AwsLaunchWizardCreateDeploymentOptions(
[property: CommandSwitch("--deployment-pattern-name")] string DeploymentPatternName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--specifications")] IEnumerable<KeyValue> Specifications,
[property: CommandSwitch("--workload-name")] string WorkloadName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}