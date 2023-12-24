using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "update-cluster-settings")]
public record AwsEcsUpdateClusterSettingsOptions(
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--settings")] string[] Settings
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}