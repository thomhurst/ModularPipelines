using ModularPipelines.Models;

namespace ModularPipelines.Console;

/// <summary>
/// Owns all console output for the pipeline lifecycle.
/// Provides buffering, phase management, and module-aware output routing.
/// </summary>
/// <remarks>
/// <para>
/// <b>Architecture:</b> This coordinator is the single point of control for all console output.
/// It intercepts Console.Out/Error to catch rogue writes and routes them to appropriate buffers.
/// </para>
/// <para>
/// <b>Lifecycle:</b>
/// 1. Install() - Intercepts Console.Out/Error
/// 2. BeginProgressAsync() - Starts progress display phase
/// 3. [Pipeline executes] - All output buffered per-module
/// 4. ProgressSession disposed - Ends progress phase
/// 5. FlushModuleOutput() - Writes buffered output in order
/// 6. WriteResults() - Prints results table
/// 7. WriteExceptions() - Prints deferred exceptions
/// 8. Uninstall() - Restores original console
/// </para>
/// <para>
/// <b>Thread Safety:</b> This class is thread-safe. All methods can be called
/// concurrently from multiple threads.
/// </para>
/// </remarks>
internal interface IConsoleCoordinator : IAsyncDisposable
{
    /// <summary>
    /// Installs the coordinator, intercepting Console.Out/Error.
    /// Must be called before pipeline execution begins.
    /// </summary>
    /// <remarks>
    /// After installation:
    /// - Console.Out/Error are replaced with coordinated writers
    /// - AnsiConsole is configured to write directly to the real console
    /// - All Console.Write* calls are intercepted and routed appropriately
    /// </remarks>
    /// <exception cref="InvalidOperationException">Thrown if already installed.</exception>
    void Install();

    /// <summary>
    /// Begins the progress display phase.
    /// </summary>
    /// <remarks>
    /// During the progress phase:
    /// - Only Spectre.Console (via AnsiConsole) writes directly to console
    /// - All other output is buffered per-module using AsyncLocal context
    /// - The phase ends when the returned session is disposed
    /// </remarks>
    /// <param name="modules">The organized modules for progress tracking.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A progress session that must be disposed to end the progress phase.</returns>
    /// <exception cref="InvalidOperationException">Thrown if progress is already active.</exception>
    Task<IProgressSession> BeginProgressAsync(OrganizedModules modules, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the output buffer for a specific module.
    /// Creates the buffer if it doesn't exist.
    /// </summary>
    /// <param name="moduleType">The module type.</param>
    /// <returns>The module's output buffer.</returns>
    IModuleOutputBuffer GetModuleBuffer(Type moduleType);

    /// <summary>
    /// Gets the buffer for output that occurs outside any module context.
    /// </summary>
    /// <returns>The unattributed output buffer.</returns>
    IModuleOutputBuffer GetUnattributedBuffer();

    /// <summary>
    /// Flushes any remaining unattributed output.
    /// Module output is flushed immediately when modules complete.
    /// </summary>
    void FlushModuleOutput();

    /// <summary>
    /// Writes the pipeline results table.
    /// </summary>
    /// <param name="summary">The pipeline summary to display.</param>
    void WriteResults(PipelineSummary summary);

    /// <summary>
    /// Adds an exception message for deferred output.
    /// </summary>
    /// <param name="message">The formatted exception message.</param>
    void AddDeferredException(string message);

    /// <summary>
    /// Writes any deferred exceptions.
    /// Should be called after WriteResults().
    /// </summary>
    void WriteExceptions();

    /// <summary>
    /// Restores original Console.Out/Error.
    /// Safe to call multiple times.
    /// </summary>
    void Uninstall();
}
