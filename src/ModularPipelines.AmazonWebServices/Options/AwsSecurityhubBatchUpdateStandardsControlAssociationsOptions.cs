using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "batch-update-standards-control-associations")]
public record AwsSecurityhubBatchUpdateStandardsControlAssociationsOptions(
[property: CommandSwitch("--standards-control-association-updates")] string[] StandardsControlAssociationUpdates
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}