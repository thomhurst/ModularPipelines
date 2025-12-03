using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "endpoints", "create")]
public record GcloudAiEndpointsCreateOptions(
[property: CliOption("--display-name")] string DisplayName
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--encryption-kms-key-name")]
    public string? EncryptionKmsKeyName { get; set; }

    [CliOption("--endpoint-id")]
    public string? EndpointId { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--request-response-logging-rate")]
    public string? RequestResponseLoggingRate { get; set; }

    [CliOption("--request-response-logging-table")]
    public string? RequestResponseLoggingTable { get; set; }
}