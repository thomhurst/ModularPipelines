using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Options;

/// <summary>
/// Options for executing a Windows batch file.
/// </summary>
/// <param name="FilePath">The path to the batch file.</param>
[ExcludeFromCodeCoverage]
public record CmdFileOptions(string FilePath) : CommandLineToolOptions;
