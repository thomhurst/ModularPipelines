using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bcm-data-exports", "get-execution")]
public record AwsBcmDataExportsGetExecutionOptions(
[property: CommandSwitch("--execution-id")] string ExecutionId,
[property: CommandSwitch("--export-arn")] string ExportArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}