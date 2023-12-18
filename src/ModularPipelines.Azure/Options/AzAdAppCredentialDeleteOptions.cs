using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "credential", "delete")]
public record AzAdAppCredentialDeleteOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--key-id")] string KeyId
) : AzOptions
{
    [BooleanCommandSwitch("--cert")]
    public bool? Cert { get; set; }
}