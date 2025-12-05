using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("hdinsight", "monitor", "enable")]
public record AzHdinsightMonitorEnableOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace")] string Workspace
) : AzOptions
{
    [CliFlag("--no-validation-timeout")]
    public bool? NoValidationTimeout { get; set; }

    [CliOption("--primary-key")]
    public string? PrimaryKey { get; set; }
}