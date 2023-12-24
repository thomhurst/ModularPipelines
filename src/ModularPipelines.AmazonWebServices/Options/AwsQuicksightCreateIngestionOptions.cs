using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "create-ingestion")]
public record AwsQuicksightCreateIngestionOptions(
[property: CommandSwitch("--data-set-id")] string DataSetId,
[property: CommandSwitch("--ingestion-id")] string IngestionId,
[property: CommandSwitch("--aws-account-id")] string AwsAccountId
) : AwsOptions
{
    [CommandSwitch("--ingestion-type")]
    public string? IngestionType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}