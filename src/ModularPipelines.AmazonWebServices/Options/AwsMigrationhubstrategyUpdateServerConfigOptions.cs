using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhubstrategy", "update-server-config")]
public record AwsMigrationhubstrategyUpdateServerConfigOptions(
[property: CommandSwitch("--server-id")] string ServerId
) : AwsOptions
{
    [CommandSwitch("--strategy-option")]
    public string? StrategyOption { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}