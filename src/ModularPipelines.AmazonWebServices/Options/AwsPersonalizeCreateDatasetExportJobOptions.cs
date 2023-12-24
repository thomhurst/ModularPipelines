using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "create-dataset-export-job")]
public record AwsPersonalizeCreateDatasetExportJobOptions(
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--dataset-arn")] string DatasetArn,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--job-output")] string JobOutput
) : AwsOptions
{
    [CommandSwitch("--ingestion-mode")]
    public string? IngestionMode { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}