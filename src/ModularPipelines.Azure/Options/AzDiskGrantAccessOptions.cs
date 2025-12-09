using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("disk", "grant-access")]
public record AzDiskGrantAccessOptions(
[property: CliOption("--duration-in-seconds")] string DurationInSeconds
) : AzOptions
{
    [CliOption("--access-level")]
    public string? AccessLevel { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--secure-vm-guest-state-sas")]
    public string? SecureVmGuestStateSas { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}