using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerImageImportOptions : DockerOptions
{
    public DockerImageImportOptions(
        string fileUrl
    )
    {
        CommandParts = ["image", "import"];

        FileUrl = fileUrl;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? FileUrl { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Repository { get; set; }

    [CommandSwitch("--change")]
    public string? Change { get; set; }

    [CommandSwitch("--message")]
    public string? Message { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }
}
