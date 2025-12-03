using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("serial-console", "disable")]
public record AzSerialConsoleDisableOptions : AzOptions;