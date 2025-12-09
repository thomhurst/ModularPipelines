using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("swf", "deprecate-activity-type")]
public record AwsSwfDeprecateActivityTypeOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--activity-type")] string ActivityType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}