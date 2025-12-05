using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("detective", "batch-get-membership-datasources")]
public record AwsDetectiveBatchGetMembershipDatasourcesOptions(
[property: CliOption("--graph-arns")] string[] GraphArns
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}