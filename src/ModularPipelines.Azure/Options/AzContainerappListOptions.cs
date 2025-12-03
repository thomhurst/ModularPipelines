using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "list")]
public record AzContainerappListOptions : AzOptions
{
    [CliOption("--environment")]
    public string? Environment { get; set; }

    [CliOption("--environment-type")]
    public string? EnvironmentType { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}