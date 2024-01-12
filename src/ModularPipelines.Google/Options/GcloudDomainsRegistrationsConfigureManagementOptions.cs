using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("domains", "registrations", "configure", "management")]
public record GcloudDomainsRegistrationsConfigureManagementOptions(
[property: PositionalArgument] string Registration
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--preferred-renewal-method")]
    public string? PreferredRenewalMethod { get; set; }

    [CommandSwitch("--transfer-lock-state")]
    public string? TransferLockState { get; set; }
}