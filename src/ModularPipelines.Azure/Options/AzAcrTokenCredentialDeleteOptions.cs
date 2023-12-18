using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "token", "credential", "delete")]
public record AzAcrTokenCredentialDeleteOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [BooleanCommandSwitch("--password1")]
    public bool? Password1 { get; set; }

    [BooleanCommandSwitch("--password2")]
    public bool? Password2 { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}