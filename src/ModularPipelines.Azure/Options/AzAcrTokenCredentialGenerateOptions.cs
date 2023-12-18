using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "token", "credential", "generate")]
public record AzAcrTokenCredentialGenerateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [CommandSwitch("--expiration")]
    public string? Expiration { get; set; }

    [CommandSwitch("--expiration-in-days")]
    public string? ExpirationInDays { get; set; }

    [BooleanCommandSwitch("--password1")]
    public bool? Password1 { get; set; }

    [BooleanCommandSwitch("--password2")]
    public bool? Password2 { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

