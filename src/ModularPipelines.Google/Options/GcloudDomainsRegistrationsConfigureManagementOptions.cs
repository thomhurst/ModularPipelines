using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("domains", "registrations", "configure", "management")]
public record GcloudDomainsRegistrationsConfigureManagementOptions(
[property: CliArgument] string Registration
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--preferred-renewal-method")]
    public string? PreferredRenewalMethod { get; set; }

    [CliOption("--transfer-lock-state")]
    public string? TransferLockState { get; set; }
}