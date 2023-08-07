using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

public record BashCommandOptions([property: CommandSwitch("-c")] string Command) : BashOptions;