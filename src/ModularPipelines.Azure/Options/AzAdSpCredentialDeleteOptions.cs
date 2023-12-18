using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

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