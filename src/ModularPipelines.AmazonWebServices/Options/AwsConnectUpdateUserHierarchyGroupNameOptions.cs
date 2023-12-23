using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-user-hierarchy-group-name")]
public record AwsConnectUpdateUserHierarchyGroupNameOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--hierarchy-group-id")] string HierarchyGroupId,
[property: CommandSwitch("--instance-id")] string InstanceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}