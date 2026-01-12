using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows;

/// <summary>
/// Options for running MSI installer packages via msiexec.exe.
/// </summary>
[ExcludeFromCodeCoverage]
[CliTool("msiexec.exe")]
public record MsiInstallerOptions([property: CliOption("/package")] string MsiPath) : WindowsInstallerOptionsBase;
