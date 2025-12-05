using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "delete-fleet-advisor-collector")]
public record AwsDmsDeleteFleetAdvisorCollectorOptions(
[property: CliOption("--collector-referenced-id")] string CollectorReferencedId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}