using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apic", "api", "definition", "create")]
public record AzApicApiDefinitionCreateOptions(
[property: CliOption("--api")] string Api,
[property: CliOption("--definition")] string Definition,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service,
[property: CliOption("--version")] string Version
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }
}