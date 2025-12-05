using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "get-relational-database-log-events")]
public record AwsLightsailGetRelationalDatabaseLogEventsOptions(
[property: CliOption("--relational-database-name")] string RelationalDatabaseName,
[property: CliOption("--log-stream-name")] string LogStreamName
) : AwsOptions
{
    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--page-token")]
    public string? PageToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}