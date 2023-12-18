using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "credential", "reset")]
public record AzAdAppCredentialResetOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
    [BooleanCommandSwitch("--append")]
    public bool? Append { get; set; }

    [CommandSwitch("--cert")]
    public string? Cert { get; set; }

    [BooleanCommandSwitch("--create-cert")]
    public bool? CreateCert { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--end-date")]
    public string? EndDate { get; set; }

    [CommandSwitch("--keyvault")]
    public string? Keyvault { get; set; }

    [CommandSwitch("--years")]
    public string? Years { get; set; }
}