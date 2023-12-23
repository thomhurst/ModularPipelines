using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "create-batch-import-job")]
public record AwsFrauddetectorCreateBatchImportJobOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--input-path")] string InputPath,
[property: CommandSwitch("--output-path")] string OutputPath,
[property: CommandSwitch("--event-type-name")] string EventTypeName,
[property: CommandSwitch("--iam-role-arn")] string IamRoleArn
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}