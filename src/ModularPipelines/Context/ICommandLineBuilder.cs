using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// Builds a <see cref="CommandLine"/> from <see cref="CommandLineToolOptions"/>.
/// This is a pure transformation with no side effects.
/// </summary>
/// <remarks>
/// This interface specifically transforms <see cref="CommandLineToolOptions"/> into a
/// <see cref="CommandLine"/> model.
/// </remarks>
internal interface ICommandLineBuilder
{
    /// <summary>
    /// Builds a command line from the given options.
    /// </summary>
    /// <param name="options">The options to build from.</param>
    /// <returns>A command line ready for execution.</returns>
    CommandLine Build(CommandLineToolOptions options);
}
