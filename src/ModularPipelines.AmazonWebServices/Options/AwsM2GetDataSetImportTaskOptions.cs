using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("m2", "get-data-set-import-task")]
public record AwsM2GetDataSetImportTaskOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--task-id")] string TaskId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}