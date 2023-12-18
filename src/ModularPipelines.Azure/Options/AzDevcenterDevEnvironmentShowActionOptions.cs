using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "dev", "environment", "show-action")]
public record AzDevcenterDevEnvironmentShowActionOptions(
[property: CommandSwitch("--action-name")] string ActionName,
[property: CommandSwitch("--environment-name")] string EnvironmentName,
[property: CommandSwitch("--project")] string Project
) : AzOptions
{
    [CommandSwitch("--dev-center")]
    public string? DevCenter { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [CommandSwitch("--user-id")]
    public string? UserId { get; set; }
}