using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerConfigCreateOptions : DockerOptions
{
    public DockerConfigCreateOptions(
        string config,
        string file
    )
    {
        CommandParts = ["config", "create"];

        CreateConfig = config;

        File = file;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? CreateConfig { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? File { get; set; }

    [CliOption("--label")]
    public virtual string? Label { get; set; }

    [CliOption("--template-driver")]
    public virtual string? TemplateDriver { get; set; }
}
