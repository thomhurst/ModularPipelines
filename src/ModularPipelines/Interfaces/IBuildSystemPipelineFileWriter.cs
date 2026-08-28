using ModularPipelines.Context;

namespace ModularPipelines.Interfaces;

/// <summary>
/// Interface for writing pipeline files for different build systems.
/// </summary>
internal interface IBuildSystemPipelineFileWriter
{
    /// <summary>
    /// Writes the pipeline file for the build system.
    /// </summary>
    /// <param name="pipelineHookContext">The pipeline hook context containing information needed to write the pipeline file.</param>
    /// <returns>A task representing the asynchronous write operation.</returns>
    Task WriteAsync(IPipelineContext pipelineHookContext);
}
