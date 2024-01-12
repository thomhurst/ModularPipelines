using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "tags", "update")]
public record GcloudArtifactsTagsUpdateOptions : GcloudOptions
{
    public GcloudArtifactsTagsUpdateOptions(
        string tag,
        string location,
        string package,
        string repository,
        string version
    )
    {
        Tag = tag;
        Location = location;
        Package = package;
        Repository = repository;
        Version = version;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Tag { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Location { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Package { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Repository { get; set; }

    [CommandSwitch("--version")]
    public new string Version { get; set; }
}
