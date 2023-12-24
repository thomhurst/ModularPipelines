using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "associate-ops-item-related-item")]
public record AwsSsmAssociateOpsItemRelatedItemOptions(
[property: CommandSwitch("--ops-item-id")] string OpsItemId,
[property: CommandSwitch("--association-type")] string AssociationType,
[property: CommandSwitch("--resource-type")] string ResourceType,
[property: CommandSwitch("--resource-uri")] string ResourceUri
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}