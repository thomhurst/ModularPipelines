using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("signalr", "custom-domain", "update")]
public record AzSignalrCustomDomainUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--signalr-name")] string SignalrName
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--certificate-resource-id")]
    public string? CertificateResourceId { get; set; }

    [CliOption("--domain-name")]
    public string? DomainName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }
}