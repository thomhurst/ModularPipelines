using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("detective", "get-investigation")]
public record AwsDetectiveGetInvestigationOptions(
[property: CliOption("--graph-arn")] string GraphArn,
[property: CliOption("--investigation-id")] string InvestigationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}