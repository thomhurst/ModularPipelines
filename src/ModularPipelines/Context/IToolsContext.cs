using System.ComponentModel;

namespace ModularPipelines.Context;

/// <summary>
/// Provides discoverable access to installed tool integrations.
/// </summary>
public interface IToolsContext
{
    /// <summary>
    /// Resolves a tool integration from the pipeline service provider.
    /// </summary>
    /// <typeparam name="T">The tool integration type.</typeparam>
    /// <returns>The registered tool integration.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown with integration-assembly and registration guidance when the integration
    /// is not registered.
    /// </exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    T Get<T>()
        where T : class;
}
