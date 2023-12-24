using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "create-import-job")]
public record AwsSesv2CreateImportJobOptions(
[property: CommandSwitch("--import-destination")] string ImportDestination,
[property: CommandSwitch("--import-data-source")] string ImportDataSource
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}