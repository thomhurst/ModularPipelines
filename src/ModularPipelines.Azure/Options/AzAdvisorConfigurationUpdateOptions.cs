using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("advisor", "configuration", "update")]
public record AzAdvisorConfigurationUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--configuration-name")]
    public string? ConfigurationName { get; set; }

    [CommandSwitch("--exclude")]
    public string? Exclude { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--include")]
    public string? Include { get; set; }

    [CommandSwitch("--low-cpu-threshold")]
    public string? LowCpuThreshold { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }
}