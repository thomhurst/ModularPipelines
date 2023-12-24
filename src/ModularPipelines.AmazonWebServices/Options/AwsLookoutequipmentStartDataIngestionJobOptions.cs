using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "start-data-ingestion-job")]
public record AwsLookoutequipmentStartDataIngestionJobOptions(
[property: CommandSwitch("--dataset-name")] string DatasetName,
[property: CommandSwitch("--ingestion-input-configuration")] string IngestionInputConfiguration,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}