namespace ModularPipelines.Helpers.Internal;

/// <summary>
/// Service that extracts the raw command parts (subcommands) from an options object.
/// Command parts are determined from CliCommand, CliSubCommand, or CliCommandAlias attributes,
/// or from the CommandParts property on CommandLineToolOptions.
/// </summary>
public interface ICommandPartsProvider
{
    /// <summary>
    /// Gets the raw command parts (subcommands) from an options object.
    /// These parts may include placeholders that need to be replaced.
    /// </summary>
    /// <param name="optionsObject">The options object to extract command parts from.</param>
    /// <returns>A list of command parts, potentially including placeholders.</returns>
    List<string> GetRawCommandParts(object optionsObject);
}
