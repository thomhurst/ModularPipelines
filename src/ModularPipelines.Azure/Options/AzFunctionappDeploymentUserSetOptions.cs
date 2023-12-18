using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "deployment", "user", "set")]
public record AzFunctionappDeploymentUserSetOptions(
[property: CommandSwitch("--user-name")] string UserName
) : AzOptions
{
    [CommandSwitch("--password")]
    public string? Password { get; set; }
}