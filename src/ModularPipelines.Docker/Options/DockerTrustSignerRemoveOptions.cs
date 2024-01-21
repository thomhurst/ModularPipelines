using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerTrustSignerRemoveOptions : DockerOptions
{
    public DockerTrustSignerRemoveOptions(
        string name,
        IEnumerable<string> repository
    )
    {
        CommandParts = ["trust", "signer", "remove"];

        Name = name;

        Repository = repository;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Name { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Repository { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}
