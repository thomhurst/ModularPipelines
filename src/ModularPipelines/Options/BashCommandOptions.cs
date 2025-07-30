using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

/// <summary>
/// Options for executing a bash command.
/// </summary>
/// <param name="Command">The bash command to execute.</param>
[ExcludeFromCodeCoverage]
public record BashCommandOptions([property: CommandSwitch("-c")] string Command) : BashOptions;