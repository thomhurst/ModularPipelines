using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Options.Windows;

/// <summary>
/// Options for running EXE installer packages.
/// </summary>
[ExcludeFromCodeCoverage]
public record ExeInstallerOptions(string ExePath) : WindowsInstallerOptionsBase(ExePath);
