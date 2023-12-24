using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("machinelearning", "update-data-source")]
public record AwsMachinelearningUpdateDataSourceOptions(
[property: CommandSwitch("--data-source-id")] string DataSourceId,
[property: CommandSwitch("--data-source-name")] string DataSourceName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}