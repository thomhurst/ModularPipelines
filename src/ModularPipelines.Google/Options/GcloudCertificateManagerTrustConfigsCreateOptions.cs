using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("certificate-manager", "trust-configs", "create")]
public record GcloudCertificateManagerTrustConfigsCreateOptions(
[property: PositionalArgument] string TrustConfig,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--trust-store")] string[] TrustStore
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}