using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "sp", "credential", "delete")]
public record AzAdSpCredentialDeleteOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--key-id")] string KeyId
) : AzOptions
{
    [BooleanCommandSwitch("--cert")]
    public bool? Cert { get; set; }
}

