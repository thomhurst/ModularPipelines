namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// The operating-system family used to generate a CLI integration in automation.
/// </summary>
public enum CliGenerationPlatform
{
    /// <summary>
    /// Generate on a Linux runner.
    /// </summary>
    Linux,

    /// <summary>
    /// Generate on a Windows runner.
    /// </summary>
    Windows,
}
