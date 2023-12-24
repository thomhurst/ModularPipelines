using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgn", "put-source-server-action")]
public record AwsMgnPutSourceServerActionOptions(
[property: CommandSwitch("--action-id")] string ActionId,
[property: CommandSwitch("--action-name")] string ActionName,
[property: CommandSwitch("--document-identifier")] string DocumentIdentifier,
[property: CommandSwitch("--order")] int Order,
[property: CommandSwitch("--source-server-id")] string SourceServerId
) : AwsOptions
{
    [CommandSwitch("--account-id")]
    public string? AccountId { get; set; }

    [CommandSwitch("--category")]
    public string? Category { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--document-version")]
    public string? DocumentVersion { get; set; }

    [CommandSwitch("--external-parameters")]
    public IEnumerable<KeyValue>? ExternalParameters { get; set; }

    [CommandSwitch("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CommandSwitch("--timeout-seconds")]
    public int? TimeoutSeconds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}