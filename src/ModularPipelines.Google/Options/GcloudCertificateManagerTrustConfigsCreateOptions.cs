using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "trust-configs", "create")]
public record GcloudCertificateManagerTrustConfigsCreateOptions(
[property: CliArgument] string TrustConfig,
[property: CliArgument] string Location,
[property: CliOption("--trust-store")] string[] TrustStore
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}