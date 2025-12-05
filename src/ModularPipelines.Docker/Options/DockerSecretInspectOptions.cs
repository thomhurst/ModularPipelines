using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerSecretInspectOptions : DockerOptions
{
    public DockerSecretInspectOptions(
        IEnumerable<string> secret
    )
    {
        CommandParts = ["secret", "inspect"];

        Secret = secret;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Secret { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliOption("--pretty")]
    public virtual string? Pretty { get; set; }
}
