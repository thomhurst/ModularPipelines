using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apic", "environment", "create")]
public record AzApicEnvironmentCreateOptions(
[property: CommandSwitch("--environment")] string Environment,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [CommandSwitch("--custom-properties")]
    public string? CustomProperties { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--kind")]
    public string? Kind { get; set; }

    [CommandSwitch("--onboarding")]
    public string? Onboarding { get; set; }

    [CommandSwitch("--server")]
    public string? Server { get; set; }

    [CommandSwitch("--title")]
    public string? Title { get; set; }

    [CommandSwitch("--workspace")]
    public string? Workspace { get; set; }
}