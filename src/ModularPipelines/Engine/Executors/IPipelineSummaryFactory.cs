using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

/// <summary>
/// Factory for creating <see cref="PipelineSummary"/> instances with all required dependencies.
/// </summary>
internal interface IPipelineSummaryFactory
{
    /// <summary>
    /// Creates a new <see cref="PipelineSummary"/> with the specified parameters.
    /// </summary>
    PipelineSummary Create(
        IReadOnlyList<IModule> allModules,
        TimeSpan totalDuration,
        DateTimeOffset start,
        DateTimeOffset end);
}
