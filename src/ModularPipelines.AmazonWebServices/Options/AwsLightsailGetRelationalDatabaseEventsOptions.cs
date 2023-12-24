using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "get-relational-database-events")]
public record AwsLightsailGetRelationalDatabaseEventsOptions(
[property: CommandSwitch("--relational-database-name")] string RelationalDatabaseName
) : AwsOptions
{
    [CommandSwitch("--duration-in-minutes")]
    public int? DurationInMinutes { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}