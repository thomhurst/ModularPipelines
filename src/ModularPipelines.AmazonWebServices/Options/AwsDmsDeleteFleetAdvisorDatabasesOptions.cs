using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "delete-fleet-advisor-databases")]
public record AwsDmsDeleteFleetAdvisorDatabasesOptions(
[property: CliOption("--database-ids")] string[] DatabaseIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}