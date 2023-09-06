using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public record BashCommandOptions([property: CommandSwitch("-c")] string Command) : BashOptions;
