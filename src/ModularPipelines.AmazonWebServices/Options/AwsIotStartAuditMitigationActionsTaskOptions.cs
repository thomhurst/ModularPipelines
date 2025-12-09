using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "start-audit-mitigation-actions-task")]
public record AwsIotStartAuditMitigationActionsTaskOptions(
[property: CliOption("--task-id")] string TaskId,
[property: CliOption("--target")] string Target,
[property: CliOption("--audit-check-to-actions-mapping")] IEnumerable<KeyValue> AuditCheckToActionsMapping
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}