using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("logicapp", "deployment", "source", "config-zip")]
public record AzLogicappDeploymentSourceConfigZipOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--src")] string Src
) : AzOptions
{
    [CliFlag("--build-remote")]
    public bool? BuildRemote { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}