using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "disassociate-delegate-from-resource")]
public record AwsWorkmailDisassociateDelegateFromResourceOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--entity-id")] string EntityId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}