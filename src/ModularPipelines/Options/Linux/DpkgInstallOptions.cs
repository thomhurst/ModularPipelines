using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux;

[ExcludeFromCodeCoverage]
public record DpkgInstallOptions([property: CommandSwitch("-i")] string Path) : CommandLineToolOptions("dpkg");
