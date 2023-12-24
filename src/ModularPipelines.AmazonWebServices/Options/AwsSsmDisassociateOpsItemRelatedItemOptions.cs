using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "disassociate-ops-item-related-item")]
public record AwsSsmDisassociateOpsItemRelatedItemOptions(
[property: CommandSwitch("--ops-item-id")] string OpsItemId,
[property: CommandSwitch("--association-id")] string AssociationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}