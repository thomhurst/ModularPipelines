using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("advisor", "configuration", "update")]
public record AzAdvisorConfigurationUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--configuration-name")]
    public string? ConfigurationName { get; set; }

    [CliOption("--exclude")]
    public string? Exclude { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--include")]
    public string? Include { get; set; }

    [CliOption("--low-cpu-threshold")]
    public string? LowCpuThreshold { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }
}