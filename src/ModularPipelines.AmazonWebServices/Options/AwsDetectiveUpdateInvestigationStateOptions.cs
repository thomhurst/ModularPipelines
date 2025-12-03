using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("detective", "update-investigation-state")]
public record AwsDetectiveUpdateInvestigationStateOptions(
[property: CliOption("--graph-arn")] string GraphArn,
[property: CliOption("--investigation-id")] string InvestigationId,
[property: CliOption("--state")] string State
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}