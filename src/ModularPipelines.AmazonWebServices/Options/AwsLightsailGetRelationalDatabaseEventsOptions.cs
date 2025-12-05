using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "get-relational-database-events")]
public record AwsLightsailGetRelationalDatabaseEventsOptions(
[property: CliOption("--relational-database-name")] string RelationalDatabaseName
) : AwsOptions
{
    [CliOption("--duration-in-minutes")]
    public int? DurationInMinutes { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}