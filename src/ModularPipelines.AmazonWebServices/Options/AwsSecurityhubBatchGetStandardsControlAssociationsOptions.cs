using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "batch-get-standards-control-associations")]
public record AwsSecurityhubBatchGetStandardsControlAssociationsOptions(
[property: CliOption("--standards-control-association-ids")] string[] StandardsControlAssociationIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}