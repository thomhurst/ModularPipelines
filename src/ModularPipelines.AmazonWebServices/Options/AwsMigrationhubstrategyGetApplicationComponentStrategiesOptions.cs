using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhubstrategy", "get-application-component-strategies")]
public record AwsMigrationhubstrategyGetApplicationComponentStrategiesOptions(
[property: CommandSwitch("--application-component-id")] string ApplicationComponentId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}