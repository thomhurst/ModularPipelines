using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "identity", "assign")]
public record AzDtIdentityAssignOptions(
[property: CommandSwitch("--dt-name")] string DtName
) : AzOptions
{
    [BooleanCommandSwitch("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CommandSwitch("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--scopes")]
    public string? Scopes { get; set; }
}