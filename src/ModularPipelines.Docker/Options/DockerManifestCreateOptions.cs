using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("manifest", "create")]
[ExcludeFromCodeCoverage]
public record DockerManifestCreateOptions : DockerOptions
{
    [CommandSwitch("--amend")]
    public string? Amend { get; set; }

    [CommandSwitch("--insecure")]
    public string? Insecure { get; set; }
}
