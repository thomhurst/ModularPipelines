using ModularPipelines.Context.Domains;
using ModularPipelines.Logging;

namespace ModularPipelines.Context;

/// <summary>
/// Base context providing access to all pipeline capabilities organized by domain.
/// </summary>
/// <remarks>
/// <para>
/// This is the foundation interface for all context types in ModularPipelines v2.0.
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
/// </list>
/// </remarks>
public interface IPipelineContext
{
    /// <summary>
    /// Logger for the current context. Thread-safe.
    /// </summary>
    IModuleLogger Logger { get; }

    /// <summary>
    /// Command execution capabilities.
    /// </summary>
    Domains.IShellContext Shell { get; }

    /// <summary>
    /// File system operations.
    /// </summary>
    IFilesContext Files { get; }

    /// <summary>
    /// Serialization and encoding.
    /// </summary>
    IDataContext Data { get; }

    /// <summary>
    /// Environment and system information.
    /// </summary>
    IEnvironmentDomainContext Environment { get; }

    /// <summary>
    /// Software installation.
    /// </summary>
    IInstallersContext Installers { get; }

    /// <summary>
    /// HTTP and download operations.
    /// </summary>
    INetworkContext Network { get; }

    /// <summary>
    /// Security operations.
    /// </summary>
    ISecurityContext Security { get; }

    /// <summary>
    /// Dependency injection and configuration.
    /// </summary>
    IServicesContext Services { get; }
}
