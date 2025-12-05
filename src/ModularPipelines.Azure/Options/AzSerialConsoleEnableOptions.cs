using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("serial-console", "enable")]
public record AzSerialConsoleEnableOptions : AzOptions;