using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apic", "api", "version", "create")]
public record AzApicApiVersionCreateOptions(
[property: CliOption("--api")] string Api,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--lifecycle-stage")]
    public string? LifecycleStage { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }
}