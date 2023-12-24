using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "start-audit-mitigation-actions-task")]
public record AwsIotStartAuditMitigationActionsTaskOptions(
[property: CommandSwitch("--task-id")] string TaskId,
[property: CommandSwitch("--target")] string Target,
[property: CommandSwitch("--audit-check-to-actions-mapping")] IEnumerable<KeyValue> AuditCheckToActionsMapping
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}