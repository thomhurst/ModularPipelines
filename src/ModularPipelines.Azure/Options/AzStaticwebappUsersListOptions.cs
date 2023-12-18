using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("staticwebapp", "users", "list")]
public record AzStaticwebappUsersListOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--authentication-provider")]
    public string? AuthenticationProvider { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}