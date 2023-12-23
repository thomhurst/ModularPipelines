using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "batch-get-standards-control-associations")]
public record AwsSecurityhubBatchGetStandardsControlAssociationsOptions(
[property: CommandSwitch("--standards-control-association-ids")] string[] StandardsControlAssociationIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}