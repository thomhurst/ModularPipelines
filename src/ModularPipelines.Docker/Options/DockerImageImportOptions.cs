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

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? FileOrUrl { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Repository { get; set; }

    [CommandSwitch("--change")]
    public string? Change { get; set; }

    [CommandSwitch("--message")]
    public string? Message { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }
}
