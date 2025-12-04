using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("offure", "hyperv", "run-as-account", "show")]
public record AzOffazureHypervRunAsAccountShowOptions : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--site-name")]
    public string? SiteName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}