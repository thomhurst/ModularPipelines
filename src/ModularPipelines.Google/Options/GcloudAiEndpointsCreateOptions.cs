using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "endpoints", "create")]
public record GcloudAiEndpointsCreateOptions(
[property: CommandSwitch("--display-name")] string DisplayName
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--encryption-kms-key-name")]
    public string? EncryptionKmsKeyName { get; set; }

    [CommandSwitch("--endpoint-id")]
    public string? EndpointId { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--request-response-logging-rate")]
    public string? RequestResponseLoggingRate { get; set; }

    [CommandSwitch("--request-response-logging-table")]
    public string? RequestResponseLoggingTable { get; set; }
}