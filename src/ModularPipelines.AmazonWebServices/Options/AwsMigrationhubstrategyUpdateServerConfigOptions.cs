using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhubstrategy", "update-server-config")]
public record AwsMigrationhubstrategyUpdateServerConfigOptions(
[property: CliOption("--server-id")] string ServerId
) : AwsOptions
{
    [CliOption("--strategy-option")]
    public string? StrategyOption { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}