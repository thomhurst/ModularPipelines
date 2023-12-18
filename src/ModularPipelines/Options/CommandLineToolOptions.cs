using System.ComponentModel;

namespace ModularPipelines.Options;

/// <summary>
/// Options for setting the context of a command, the command line tool, and any arguments it needs
/// </summary>
public record CommandLineToolOptions : CommandLineOptions
{
    public CommandLineToolOptions(string tool) : this(tool, null)
    {
    }

    public CommandLineToolOptions(string tool, params string[]? arguments)
    {
        Tool = tool;
        Arguments = arguments;
    }

    public string Tool { get; init; }

    /// <summary>
    /// Used for providing switches and arguments to the tool
    /// </summary>
    public IEnumerable<string>? Arguments { get; init; }

    /// <summary>
    /// If you have an object that has Properties with attributes such as PositionalArgument or CommandSwitch
    /// Then by setting it here, the values will automatically be extrapolated when running the command
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public object? OptionsObject { get; init; }

    /// <summary>
    /// Used for command line tools that support the following syntax: MyTool -Arg1 -Arg2 -- RunSetting1 RunSetting2
    /// </summary>
    public IEnumerable<string>? RunSettings { get; init; }
}