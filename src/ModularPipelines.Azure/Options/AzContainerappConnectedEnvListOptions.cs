using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "connected-env", "list")]
public record AzContainerappConnectedEnvListOptions : AzOptions
{
    [CliOption("--custom-location")]
    public string? CustomLocation { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}