using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "task", "credential", "update")]
public record AzAcrTaskCredentialUpdateOptions(
[property: CommandSwitch("--login-server")] string LoginServer,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--use-identity")]
    public bool? UseIdentity { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }
}