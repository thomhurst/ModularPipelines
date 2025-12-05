using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerImageImportOptions : DockerOptions
{
    public DockerImageImportOptions(
        string fileOrUrl
    )
    {
        CommandParts = ["image", "import"];

        FileOrUrl = fileOrUrl;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? FileOrUrl { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Repository { get; set; }

    [CliOption("--change")]
    public virtual string? Change { get; set; }

    [CliOption("--message")]
    public virtual string? Message { get; set; }

    [CliOption("--platform")]
    public virtual string? Platform { get; set; }
}
