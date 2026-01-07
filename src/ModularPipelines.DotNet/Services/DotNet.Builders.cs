using ModularPipelines.DotNet.Builders;
using ModularPipelines.DotNet.Options;

namespace ModularPipelines.DotNet.Services;

/// <summary>
/// Fluent builder implementations for DotNet.
/// </summary>
internal partial class DotNet
{
    /// <inheritdoc />
    public IDotNetBuildBuilder BuildBuilder() => new DotNetBuildBuilder(_command);

    /// <inheritdoc />
    public IDotNetBuildBuilder BuildBuilder(DotNetBuildOptions options) => new DotNetBuildBuilder(_command, options);
}
