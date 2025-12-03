using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "batch-update-standards-control-associations")]
public record AwsSecurityhubBatchUpdateStandardsControlAssociationsOptions(
[property: CliOption("--standards-control-association-updates")] string[] StandardsControlAssociationUpdates
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}