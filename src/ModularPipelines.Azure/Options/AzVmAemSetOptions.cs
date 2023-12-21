using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "aem", "set")]
public record AzVmAemSetOptions : AzOptions
{
    [BooleanCommandSwitch("--debug-extension")]
    public bool? DebugExtension { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--install-new-extension")]
    public bool? InstallNewExtension { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--proxy-uri")]
    public string? ProxyUri { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--set-access-to-individual-resources")]
    public bool? SetAccessToIndividualResources { get; set; }

    [BooleanCommandSwitch("--skip-storage-analytics")]
    public bool? SkipStorageAnalytics { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}