using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scvmm", "vm", "guest-agent", "enable")]
public record AzScvmmVmGuestAgentEnableOptions(
[property: CliOption("--password")] string Password,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--username")] string Username,
[property: CliOption("--vm-name")] string VmName
) : AzOptions
{
    [CliOption("--https-proxy")]
    public string? HttpsProxy { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}