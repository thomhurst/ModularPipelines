using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerSecretCreateOptions : DockerOptions
{
    public DockerSecretCreateOptions(
        string secret
    )
    {
        CommandParts = ["secret", "create"];

        Secret = secret;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Secret { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? File { get; set; }

    [CliOption("--driver")]
    public virtual string? Driver { get; set; }

    [CliOption("--label")]
    public virtual string? Label { get; set; }

    [CliOption("--template-driver")]
    public virtual string? TemplateDriver { get; set; }
}
