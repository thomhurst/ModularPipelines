using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "create-export-job")]
public record AwsSesv2CreateExportJobOptions(
[property: CommandSwitch("--export-data-source")] string ExportDataSource,
[property: CommandSwitch("--export-destination")] string ExportDestination
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}