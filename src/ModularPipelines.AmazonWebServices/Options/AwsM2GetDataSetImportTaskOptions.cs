using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("m2", "get-data-set-import-task")]
public record AwsM2GetDataSetImportTaskOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--task-id")] string TaskId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}