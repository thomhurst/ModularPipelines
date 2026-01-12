using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Options.Windows;

/// <summary>
/// Options for running EXE installer packages.
/// </summary>
[ExcludeFromCodeCoverage]
public record ExeInstallerOptions : WindowsInstallerOptionsBase
{
    /// <summary>
    /// Creates options for the specified EXE installer.
    /// </summary>
    /// <param name="exePath">The path to the EXE installer.</param>
    public ExeInstallerOptions(string exePath)
    {
        Tool = exePath;
    }
}
