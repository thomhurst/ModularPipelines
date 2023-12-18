using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "sp", "credential", "list")]
public record AzAdSpCredentialListOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
    [BooleanCommandSwitch("--cert")]
    public bool? Cert { get; set; }
}