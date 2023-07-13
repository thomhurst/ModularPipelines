using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Mac;

public record MacBrewOptions([property: CommandSwitch("--cask")] string PackageName) : CommandLineToolOptions("brew");