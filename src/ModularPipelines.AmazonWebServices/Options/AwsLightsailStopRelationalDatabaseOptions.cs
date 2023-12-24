using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "stop-relational-database")]
public record AwsLightsailStopRelationalDatabaseOptions(
[property: CommandSwitch("--relational-database-name")] string RelationalDatabaseName
) : AwsOptions
{
    [CommandSwitch("--relational-database-snapshot-name")]
    public string? RelationalDatabaseSnapshotName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}