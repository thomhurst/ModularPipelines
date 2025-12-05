using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("detective", "start-investigation")]
public record AwsDetectiveStartInvestigationOptions(
[property: CliOption("--graph-arn")] string GraphArn,
[property: CliOption("--entity-arn")] string EntityArn,
[property: CliOption("--scope-start-time")] long ScopeStartTime,
[property: CliOption("--scope-end-time")] long ScopeEndTime
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}