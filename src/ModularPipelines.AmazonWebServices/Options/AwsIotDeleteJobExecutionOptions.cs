using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "delete-job-execution")]
public record AwsIotDeleteJobExecutionOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--thing-name")] string ThingName,
[property: CommandSwitch("--execution-number")] long ExecutionNumber
) : AwsOptions
{
    [CommandSwitch("--namespace-id")]
    public string? NamespaceId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}