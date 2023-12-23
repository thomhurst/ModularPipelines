using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "create-dataset-import-job")]
public record AwsPersonalizeCreateDatasetImportJobOptions(
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--dataset-arn")] string DatasetArn,
[property: CommandSwitch("--data-source")] string DataSource,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--import-mode")]
    public string? ImportMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}