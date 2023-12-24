using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "wait", "addon-deleted")]
public record AwsEksWaitAddonDeletedOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--addon-name")] string AddonName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}