using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// Builds a <see cref="CommandLine"/> from <see cref="CommandLineToolOptions"/>.
/// This is a pure transformation with no side effects.
/// </summary>
/// <remarks>
/// This interface is distinct from <see cref="Builders.ICommandBuilder"/> which is a fluent
/// builder pattern for configuring and executing commands. This interface is specifically
/// for transforming <see cref="CommandLineToolOptions"/> into a <see cref="CommandLine"/> model.
/// </remarks>
public interface ICommandLineBuilder
{
    /// <summary>
    /// Builds a command line from the given options.
    /// </summary>
    /// <param name="options">The options to build from.</param>
    /// <returns>A command line ready for execution.</returns>
    CommandLine Build(CommandLineToolOptions options);
}
