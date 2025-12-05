using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apic", "environment", "create")]
public record AzApicEnvironmentCreateOptions(
[property: CliOption("--environment")] string Environment,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--custom-properties")]
    public string? CustomProperties { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--onboarding")]
    public string? Onboarding { get; set; }

    [CliOption("--server")]
    public string? Server { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }
}