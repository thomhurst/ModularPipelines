using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "get-relational-database-log-events")]
public record AwsLightsailGetRelationalDatabaseLogEventsOptions(
[property: CommandSwitch("--relational-database-name")] string RelationalDatabaseName,
[property: CommandSwitch("--log-stream-name")] string LogStreamName
) : AwsOptions
{
    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }

    [CommandSwitch("--end-time")]
    public long? EndTime { get; set; }

    [CommandSwitch("--page-token")]
    public string? PageToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}