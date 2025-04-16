using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "wait")]
[ExcludeFromCodeCoverage]
public record DockerComposeWaitOptions : DockerOptions
{
    [CommandSwitch("--down-project")]
    public virtual string? DownProject { get; set; }
}
