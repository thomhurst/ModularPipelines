using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "add-tags-to-on-premises-instances")]
public record AwsDeployAddTagsToOnPremisesInstancesOptions(
[property: CommandSwitch("--tags")] string[] Tags,
[property: CommandSwitch("--instance-names")] string[] InstanceNames
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}