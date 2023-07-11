using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("help")]
public record GitHelpOptions : GitOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--no-external-commands")]
    public bool? NoExternalCommands { get; set; }

    [BooleanCommandSwitch("--no-aliases")]
    public bool? NoAliases { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("--c")]
    public bool? C { get; set; }

    [BooleanCommandSwitch("--config")]
    public bool? Config { get; set; }

    [BooleanCommandSwitch("--guides")]
    public bool? Guides { get; set; }

    [BooleanCommandSwitch("--user-interfaces")]
    public bool? UserInterfaces { get; set; }

    [BooleanCommandSwitch("--developer-interfaces")]
    public bool? DeveloperInterfaces { get; set; }

    [BooleanCommandSwitch("--info")]
    public bool? Info { get; set; }

    [BooleanCommandSwitch("--man")]
    public bool? Man { get; set; }

    [BooleanCommandSwitch("--web")]
    public bool? Web { get; set; }

}