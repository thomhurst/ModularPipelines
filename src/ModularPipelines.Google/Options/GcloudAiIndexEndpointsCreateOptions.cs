using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "index-endpoints", "create")]
public record GcloudAiIndexEndpointsCreateOptions(
[property: CliOption("--display-name")] string DisplayName
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable-private-service-connect")]
    public bool? EnablePrivateServiceConnect { get; set; }

    [CliOption("--encryption-kms-key-name")]
    public string? EncryptionKmsKeyName { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--project-allowlist")]
    public string[]? ProjectAllowlist { get; set; }

    [CliFlag("--public-endpoint-enabled")]
    public bool? PublicEndpointEnabled { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}