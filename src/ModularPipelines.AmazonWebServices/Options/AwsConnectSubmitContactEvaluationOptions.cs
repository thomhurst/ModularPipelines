using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "submit-contact-evaluation")]
public record AwsConnectSubmitContactEvaluationOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--evaluation-id")] string EvaluationId
) : AwsOptions
{
    [CommandSwitch("--answers")]
    public IEnumerable<KeyValue>? Answers { get; set; }

    [CommandSwitch("--notes")]
    public IEnumerable<KeyValue>? Notes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}