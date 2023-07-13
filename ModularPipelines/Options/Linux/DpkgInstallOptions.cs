using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux;

public record DpkgInstallOptions([property: CommandSwitch("-i")] string Path) : CommandLineToolOptions("dpkg");