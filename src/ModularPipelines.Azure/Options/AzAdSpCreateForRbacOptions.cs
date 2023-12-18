using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "sp", "create-for-rbac")]
public record AzAdSpCreateForRbacOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
    [CommandSwitch("--cert")]
    public string? Cert { get; set; }

    [BooleanCommandSwitch("--create-cert")]
    public bool? CreateCert { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--json-auth")]
    public bool? JsonAuth { get; set; }

    [CommandSwitch("--keyvault")]
    public string? Keyvault { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--scopes")]
    public string? Scopes { get; set; }

    [CommandSwitch("--years")]
    public string? Years { get; set; }
}

