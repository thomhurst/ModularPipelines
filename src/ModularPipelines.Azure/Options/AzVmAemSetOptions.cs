using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "aem", "set")]
public record AzVmAemSetOptions : AzOptions
{
    [CliFlag("--debug-extension")]
    public bool? DebugExtension { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--install-new-extension")]
    public bool? InstallNewExtension { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--proxy-uri")]
    public string? ProxyUri { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--set-access-to-individual-resources")]
    public bool? SetAccessToIndividualResources { get; set; }

    [CliFlag("--skip-storage-analytics")]
    public bool? SkipStorageAnalytics { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}