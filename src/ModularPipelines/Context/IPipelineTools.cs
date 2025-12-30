using ModularPipelines.Helpers;
using ModularPipelines.Http;

namespace ModularPipelines.Context;

/// <summary>
/// Provides access to command execution and tool helpers.
/// </summary>
/// <remarks>
/// This interface groups command execution, HTTP, download, and installation helpers
/// for better Interface Segregation.
/// </remarks>
public interface IPipelineTools
{
    /// <summary>
    /// Gets helpers for running commands.
    /// </summary>
    /// <remarks>
    /// Provides cross-platform command execution with output capture and logging.
    /// </remarks>
    ICommand Command { get; }

    /// <summary>
    /// Gets helpers for running powershell.
    /// </summary>
    IPowershell Powershell { get; }

    /// <summary>
    /// Gets helpers for executing bash commands.
    /// </summary>
    IBash Bash { get; }

    /// <summary>
    /// Gets helpers for sending HTTP requests.
    /// </summary>
    /// <remarks>
    /// Provides a configured HttpClient with retry policies and logging.
    /// </remarks>
    IHttp Http { get; }

    /// <summary>
    /// Gets helpers for downloading data from the web.
    /// </summary>
    /// <remarks>
    /// Includes progress tracking and verification helpers.
    /// </remarks>
    IDownloader Downloader { get; }

    /// <summary>
    /// Gets helpers for installing applications.
    /// </summary>
    /// <remarks>
    /// Supports package managers like Chocolatey, apt, brew, etc.
    /// </remarks>
    IInstaller Installer { get; }
}
