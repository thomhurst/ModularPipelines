using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bcm-data-exports", "get-execution")]
public record AwsBcmDataExportsGetExecutionOptions(
[property: CliOption("--execution-id")] string ExecutionId,
[property: CliOption("--export-arn")] string ExportArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}