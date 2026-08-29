using Microsoft.Extensions.Logging;
using ModularPipelines.Context.Domains;
using ModularPipelines.Logging;

namespace ModularPipelines.Context;

/// <summary>
/// Base context providing access to all pipeline capabilities organized by domain.
/// </summary>
/// <remarks>
/// <para>
/// This is the foundation interface for all pipeline and module contexts.
/// Capabilities are organized into domain categories for easy discovery:
/// </para>
/// <list type="bullet">
/// <item><term>Shell</term><description>Command execution (CLI, Bash, PowerShell)</description></item>
/// <item><term>Files</term><description>File system operations</description></item>
/// <item><term>Data</term><description>Serialization and encoding</description></item>
/// <item><term>Environment</term><description>System and CI/CD information</description></item>
/// <item><term>Installers</term><description>Package installation</description></item>
/// <item><term>Network</term><description>HTTP and downloads</description></item>
/// <item><term>Security</term><description>Certificates and hashing</description></item>
/// <item><term>Services</term><description>DI and configuration</description></item>
/// <item><term>Tools</term><description>Installed tool integrations</description></item>
/// <item><term>Summary</term><description>Pipeline summary logging (displayed after completion)</description></item>
/// </list>
/// </remarks>
public interface IPipelineContext
{
    /// <summary>
    /// Gets the logger for the current context. Thread-safe.
    /// </summary>
    ILogger Logger { get; }

    /// <summary>
    /// Gets the command execution capabilities.
    /// </summary>
    Domains.IShellContext Shell { get; }

    /// <summary>
    /// Gets the file system operations.
    /// </summary>
    IFilesContext Files { get; }

    /// <summary>
    /// Gets the serialization and encoding capabilities.
    /// </summary>
    IDataContext Data { get; }

    /// <summary>
    /// Gets the environment and system information.
    /// </summary>
    IEnvironmentContext Environment { get; }

    /// <summary>
    /// Gets the software installation capabilities.
    /// </summary>
    IInstallersContext Installers { get; }

    /// <summary>
    /// Gets the HTTP and download operations.
    /// </summary>
    INetworkContext Network { get; }

    /// <summary>
    /// Gets the security operations.
    /// </summary>
    ISecurityContext Security { get; }

    /// <summary>
    /// Gets the dependency injection and configuration capabilities.
    /// </summary>
    IServicesContext Services { get; }

    /// <summary>
    /// Gets the installed tool integrations.
    /// </summary>
    IToolsContext Tools { get; }

    /// <summary>
    /// Gets the summary logger for messages displayed after pipeline completion.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use <see cref="Summary"/> to log important information that should be prominently
    /// displayed after all modules complete. This is ideal for:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Version numbers and build outputs</description></item>
    /// <item><description>Important metrics and statistics</description></item>
    /// <item><description>Deployment URLs and endpoints</description></item>
    /// <item><description>Warnings that need visibility</description></item>
    /// </list>
    /// <para><b>Example usage:</b></para>
    /// <code>
    /// context.Summary.Information("Build completed");
    /// context.Summary.KeyValue("Version", version);
    /// context.Summary.Success("Artifacts", "Published to NuGet");
    /// context.Summary.Warning("Some optional tests were skipped");
    /// </code>
    /// </remarks>
    ISummaryLogger Summary { get; }
}
