using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust", "signer", "add")]
[ExcludeFromCodeCoverage]
public record DockerTrustSignerAddOptions : DockerOptions
{
    [CommandSwitch("--key")]
    public string? Key { get; set; }
}
