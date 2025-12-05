using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhubstrategy", "get-server-strategies")]
public record AwsMigrationhubstrategyGetServerStrategiesOptions(
[property: CliOption("--server-id")] string ServerId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}