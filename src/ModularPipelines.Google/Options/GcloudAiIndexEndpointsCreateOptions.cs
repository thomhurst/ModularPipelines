using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "index-endpoints", "create")]
public record GcloudAiIndexEndpointsCreateOptions(
[property: CommandSwitch("--display-name")] string DisplayName
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enable-private-service-connect")]
    public bool? EnablePrivateServiceConnect { get; set; }

    [CommandSwitch("--encryption-kms-key-name")]
    public string? EncryptionKmsKeyName { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--project-allowlist")]
    public string[]? ProjectAllowlist { get; set; }

    [BooleanCommandSwitch("--public-endpoint-enabled")]
    public bool? PublicEndpointEnabled { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}