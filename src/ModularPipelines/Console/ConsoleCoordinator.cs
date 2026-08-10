using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using MEL.Spectre;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Spectre.Console;

namespace ModularPipelines.Console;

/// <summary>
/// Central implementation that owns all console output.
/// </summary>
/// <remarks>
/// <para>
/// <b>Architecture:</b> This coordinator is the single point of control for all console output.
/// It intercepts Console.Out/Error to catch rogue writes and routes them to appropriate buffers.
/// Spectre.Console is configured to write directly to the real console for progress rendering.
/// </para>
/// <para>
/// <b>Thread Safety:</b> This class is thread-safe. All methods can be called
/// concurrently from multiple threads.
/// </para>
/// <para>
/// <b>IProgressDisplay:</b> Implements IProgressDisplay to integrate with existing notification
/// system. The ProgressPrinter forwards notifications here, which are delegated to the active session.
/// </para>
/// </remarks>
[ExcludeFromCodeCoverage]
internal class ConsoleCoordinator : IConsoleCoordinator, IProgressDisplay
{
    private const int DefaultCiConsoleWidth = 160;

    private readonly IBuildSystemFormatterProvider _formatterProvider;
    private readonly IResultsPrinter _resultsPrinter;
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly ISecretProvider _secretProvider;
    private readonly IOptions<PipelineOptions> _options;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IBuildSystemDetector _buildSystemDetector;
    private readonly IServiceProvider _serviceProvider;
    private readonly object _phaseLock = new();
    private readonly ConcurrentDictionary<Type, ModuleOutputBuffer> _moduleBuffers = new();
    private readonly ModuleOutputBuffer _unattributedBuffer;
    private readonly ConcurrentQueue<string> _deferredExceptions = new();
    private readonly IOutputCoordinator _outputCoordinator;
    private readonly ISpectreConsoleLoggerControl _loggerControl;
    private readonly INonSpectreLoggerFactory _nonSpectreLoggerFactory;
    private readonly IAnsiConsole _ansiConsole;
    private TextWriter? _originalConsoleOut;
    private TextWriter? _originalConsoleError;
    private IAnsiConsole? _originalAnsiConsole;
    private CoordinatedTextWriter? _coordinatedOut;
    private CoordinatedTextWriter? _coordinatedError;
    private volatile bool _isProgressActive;
    private bool _isInstalled;
    private IProgressSession? _activeSession;
    private ILogger? _outputLogger;

    public ConsoleCoordinator(
        IBuildSystemFormatterProvider formatterProvider,
        IResultsPrinter resultsPrinter,
        ISecretObfuscator secretObfuscator,
        ISecretProvider secretProvider,
        IOptions<PipelineOptions> options,
        ILoggerFactory loggerFactory,
        IBuildSystemDetector buildSystemDetector,
        IServiceProvider serviceProvider,
        IOutputCoordinator outputCoordinator,
        ISpectreConsoleLoggerControl loggerControl,
        INonSpectreLoggerFactory nonSpectreLoggerFactory,
        IAnsiConsole ansiConsole)
    {
        _formatterProvider = formatterProvider;
        _resultsPrinter = resultsPrinter;
        _secretObfuscator = secretObfuscator;
        _secretProvider = secretProvider;
        _options = options;
        _loggerFactory = loggerFactory;
        _buildSystemDetector = buildSystemDetector;
        _serviceProvider = serviceProvider;
        _outputCoordinator = outputCoordinator;
        _loggerControl = loggerControl;
        _nonSpectreLoggerFactory = nonSpectreLoggerFactory;
        _ansiConsole = ansiConsole;
        _unattributedBuffer = new ModuleOutputBuffer(
            "Pipeline",
            typeof(void),
            _options.Value.Console.ModuleOutputFlushThreshold,
            RequestThresholdFlush,
            isSpectreEnabled: logLevel => _loggerControl.WouldRender(
                OutputLoggerCategories.Pipeline,
                logLevel),
            // Unattributed logs do not represent pipeline completion status.
            showSuccessMarker: false);
    }

    /// <inheritdoc />
    public void Install()
    {
        lock (_phaseLock)
        {
            if (_isInstalled)
            {
                throw new InvalidOperationException("ConsoleCoordinator is already installed.");
            }

            // Save original streams before any modifications
            var originalOut = System.Console.Out;
            var originalError = System.Console.Error;
            var originalAnsi = AnsiConsole.Console;

            try
            {
                _originalConsoleOut = originalOut;
                _originalConsoleError = originalError;
                _originalAnsiConsole = originalAnsi;

                // Configure Spectre.Console to use the REAL console directly
                // This bypasses our interception for progress rendering
                var configuredAnsiConsole = UsesGlobalAnsiConsole
                    ? AnsiConsole.Create(
                        CreateAnsiConsoleSettings(_originalConsoleOut, _buildSystemDetector.IsKnownBuildAgent))
                    : _ansiConsole;
                configuredAnsiConsole.Profile.Capabilities.Interactive = _options.Value.Console.ShowProgress;
                if (UsesGlobalAnsiConsole)
                {
                    AnsiConsole.Console = configuredAnsiConsole;
                }

                // Configure console width for CI environments
                // Spectre.Console defaults to 80 characters when it can't detect terminal width,
                // which is common in CI environments where output is redirected
                ConfigureConsoleWidth();

                // Create logger for structured output during flush
                _outputLogger = _loggerFactory.CreateLogger(OutputLoggerCategories.Pipeline);

                // Install our intercepting writers
                // Buffer module output while the coordinated output phase is active. Flush paths
                // write to the captured real console, so unrelated module output remains buffered.
                _coordinatedOut = new CoordinatedTextWriter(
                    this,
                    _originalConsoleOut,
                    () => _isProgressActive,
                    _secretObfuscator,
                    _secretProvider);

                _coordinatedError = new CoordinatedTextWriter(
                    this,
                    _originalConsoleError,
                    () => _isProgressActive,
                    _secretObfuscator,
                    _secretProvider,
                    isError: true);

                System.Console.SetOut(_coordinatedOut);
                System.Console.SetError(_coordinatedError);

                _isInstalled = true;
            }
            catch
            {
                // Restore original streams on failure
                System.Console.SetOut(originalOut);
                System.Console.SetError(originalError);
                if (UsesGlobalAnsiConsole)
                {
                    AnsiConsole.Console = originalAnsi;
                }

                _originalConsoleOut = null;
                _originalConsoleError = null;
                _originalAnsiConsole = null;
                _coordinatedOut = null;
                _coordinatedError = null;

                throw;
            }
        }
    }

    /// <inheritdoc />
    public void Uninstall()
    {
        lock (_phaseLock)
        {
            if (!_isInstalled)
            {
                return;
            }

            // Flush any remaining buffered content
            _coordinatedOut?.Flush();
            _coordinatedError?.Flush();

            // Restore original streams
            if (_originalConsoleOut != null)
            {
                System.Console.SetOut(_originalConsoleOut);
            }

            if (_originalConsoleError != null)
            {
                System.Console.SetError(_originalConsoleError);
            }

            if (UsesGlobalAnsiConsole && _originalAnsiConsole != null)
            {
                AnsiConsole.Console = _originalAnsiConsole;
            }

            _originalConsoleOut = null;
            _originalConsoleError = null;
            _originalAnsiConsole = null;
            _coordinatedOut = null;
            _coordinatedError = null;
            _isInstalled = false;
        }
    }

    /// <inheritdoc />
    public void EnableOutputBuffering()
    {
        lock (_phaseLock)
        {
            if (_isProgressActive)
            {
                return; // Already enabled
            }

            _isProgressActive = true;

            // CRITICAL: Set OutputCoordinator's progress state inside the lock
            // to prevent race conditions where a module completes between
            // _isProgressActive = true and OutputCoordinator being notified
            _outputCoordinator.SetProgressActive(true);
        }
    }

    /// <inheritdoc />
    public async Task<IProgressSession> BeginProgressAsync(
        OrganizedModules modules,
        CancellationToken cancellationToken)
    {
        if (!_options.Value.Console.ShowProgress)
        {
            // Return a no-op session if progress is disabled
            // Explicitly ensure deferred output is disabled for consistency
            _outputCoordinator.SetProgressActive(false);
            _activeSession = new NoOpProgressSession();
            return _activeSession;
        }

        ProgressSession session;

        lock (_phaseLock)
        {
            // Check if a session is already active (not just buffering enabled)
            if (_activeSession != null)
            {
                throw new InvalidOperationException("Progress session is already active.");
            }

            // Enable buffering if not already enabled via EnableOutputBuffering()
            if (!_isProgressActive)
            {
                _isProgressActive = true;
                _outputCoordinator.SetProgressActive(true);
            }

            session = new ProgressSession(
                this,
                modules,
                _options,
                _loggerFactory,
                _ansiConsole,
                cancellationToken);

            // Wire up the progress controller for output coordination
            _outputCoordinator.SetProgressController(session);

            _activeSession = session;
        }

        // Start the progress display
        await session.StartAsync().ConfigureAwait(false);

        return session;
    }

    /// <inheritdoc />
    public IModuleOutputBuffer GetModuleBuffer(Type moduleType)
    {
        return _moduleBuffers.GetOrAdd(
            moduleType,
            t => new ModuleOutputBuffer(
                t.Name,
                t,
                _options.Value.Console.ModuleOutputFlushThreshold,
                RequestThresholdFlush,
                isSpectreEnabled: logLevel => _loggerControl.WouldRender(
                    OutputLoggerCategories.ForModule(t),
                    logLevel),
                showFailureHeaderWithoutOutput: !_options.Value.Console.PrintResults,
                outputExcerptMaximumBytes: _options.Value.RunReport.IncludeModuleOutput
                    ? _options.Value.RunReport.MaxOutputBytesPerModule
                    : 0,
                outputExcerptSecretObfuscator: _secretObfuscator,
                outputExcerptSecretProvider: _secretProvider,
                outputExcerptLogger: _loggerFactory.CreateLogger<ModuleOutputExcerptBuffer>()));
    }

    /// <inheritdoc />
    public ModuleOutputExcerpt? GetModuleOutputExcerpt(Type moduleType) =>
        _moduleBuffers.TryGetValue(moduleType, out var buffer)
            ? buffer.GetOutputExcerpt()
            : null;

    /// <inheritdoc />
    public IModuleOutputBuffer GetUnattributedBuffer() => _unattributedBuffer;

    /// <inheritdoc />
    public Task<IReadOnlyList<IModuleOutputBuffer>> FlushPendingWritesAsync()
    {
        return FlushPendingWritesAsync(OutputFlushKind.Complete);
    }

    /// <inheritdoc />
    public async Task FlushInProgressModuleOutputAsync(CancellationToken cancellationToken = default)
    {
        var newlyPopulatedBuffers = await FlushPendingWritesAsync(OutputFlushKind.Incremental).ConfigureAwait(false);

        foreach (var buffer in newlyPopulatedBuffers.Where(buffer => buffer.IsComplete))
        {
            await FlushBufferSafelyAsync(buffer, OutputFlushKind.Complete, cancellationToken)
                .ConfigureAwait(false);
        }

        var buffers = _moduleBuffers.Values
            .Where(buffer => !buffer.IsComplete && buffer.HasOutput)
            .Cast<IModuleOutputBuffer>()
            .ToArray();

        foreach (var buffer in buffers)
        {
            await FlushBufferSafelyAsync(buffer, OutputFlushKind.Incremental, cancellationToken)
                .ConfigureAwait(false);
        }

        if (_unattributedBuffer.HasOutput)
        {
            await FlushBufferSafelyAsync(
                    _unattributedBuffer,
                    OutputFlushKind.Incremental,
                    cancellationToken)
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    public async Task FlushModuleOutputAsync()
    {
        // Output is now flushed immediately when modules complete.
        // This method remains for API compatibility but only flushes
        // unattributed output (pipeline-level logs).
        if (_originalConsoleOut == null)
        {
            return; // Not installed, nothing to flush
        }

        var formatter = _formatterProvider.GetFormatter();

        // Flush unattributed output (if any)
        if (_unattributedBuffer.HasOutput)
        {
            var unattributedLogger = _outputLogger
                                     ?? _loggerFactory.CreateLogger(OutputLoggerCategories.Pipeline);
            await _unattributedBuffer
                .FlushToAsync(
                    _originalConsoleOut,
                    formatter,
                    unattributedLogger,
                    _loggerControl,
                    OutputFlushKind.Complete,
                    fallbackLoggers: _nonSpectreLoggerFactory.CreateLoggers(
                        OutputLoggerCategories.Pipeline))
                .ConfigureAwait(false);

            if (_unattributedBuffer.HasStructuredDeliveryRetries)
            {
                await _unattributedBuffer
                    .FlushToAsync(
                        _originalConsoleOut,
                        formatter,
                        unattributedLogger,
                        _loggerControl,
                        OutputFlushKind.Complete,
                        fallbackLoggers: _nonSpectreLoggerFactory.CreateLoggers(
                            OutputLoggerCategories.Pipeline))
                    .ConfigureAwait(false);
            }
        }
    }

    /// <inheritdoc />
    public void WriteResults(PipelineSummary summary)
    {
        // Results printer uses AnsiConsole which we've configured
        // to write directly to the real console
        _resultsPrinter.PrintResults(summary);
    }

    /// <inheritdoc />
    public void AddDeferredException(string message)
    {
        _deferredExceptions.Enqueue(message);
    }

    /// <inheritdoc />
    public void WriteExceptions()
    {
        if (_deferredExceptions.IsEmpty)
        {
            return;
        }

        var messages = new List<string>();
        while (_deferredExceptions.TryDequeue(out var message))
        {
            messages.Add(message);
        }

        if (messages.Count == 0)
        {
            return;
        }

        // Write using AnsiConsole (goes to real console)
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold yellow]\u26a0[/] [bold red]Deferred Exceptions[/]");
        AnsiConsole.WriteLine();

        foreach (var message in messages)
        {
            AnsiConsole.WriteLine(message);
        }

        AnsiConsole.WriteLine();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        // Flush any buffered module output before uninstalling
        // This ensures logs are written even when pipeline isn't executed (e.g., in tests)
        if (_isInstalled)
        {
            try
            {
                await FlushModuleOutputAsync().ConfigureAwait(false);
            }
            catch (InvalidOperationException)
            {
                // Ignore if not installed - FlushModuleOutputAsync requires installation
            }
        }

        Uninstall();
        await Task.CompletedTask;
    }

    #region IProgressDisplay Implementation

    /// <summary>
    /// Implements IProgressDisplay.RunAsync for integration with existing notification system.
    /// This is called by ProgressPrinter when the old code path is used.
    /// </summary>
    async Task IProgressDisplay.RunAsync(OrganizedModules organizedModules, CancellationToken cancellationToken)
    {
        // Install if not already installed (for backward compatibility)
        if (!_isInstalled)
        {
            Install();
        }

        await using var session = await BeginProgressAsync(organizedModules, cancellationToken).ConfigureAwait(false);

        // Wait for cancellation - the session runs until disposed
        try
        {
            await Task.Delay(Timeout.Infinite, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // Expected - progress ends when cancelled
        }
    }

    /// <inheritdoc />
    void IProgressDisplay.OnModuleStarted(ModuleState moduleState, TimeSpan estimatedDuration)
    {
        _activeSession?.OnModuleStarted(moduleState, estimatedDuration);
    }

    /// <inheritdoc />
    void IProgressDisplay.OnModuleCompleted(ModuleState moduleState, bool isSuccessful)
    {
        _activeSession?.OnModuleCompleted(moduleState, isSuccessful);
    }

    /// <inheritdoc />
    void IProgressDisplay.OnModuleSkipped(ModuleState moduleState)
    {
        _activeSession?.OnModuleSkipped(moduleState);
    }

    /// <inheritdoc />
    void IProgressDisplay.OnSubModuleCreated(IModule parentModule, SubModuleBase subModule, TimeSpan estimatedDuration)
    {
        _activeSession?.OnSubModuleCreated(parentModule, subModule, estimatedDuration);
    }

    /// <inheritdoc />
    void IProgressDisplay.OnSubModuleCompleted(SubModuleBase subModule, bool isSuccessful)
    {
        _activeSession?.OnSubModuleCompleted(subModule, isSuccessful);
    }

    #endregion

    /// <summary>
    /// Ends the progress phase. Called by ProgressSession.DisposeAsync().
    /// </summary>
    internal void EndProgressPhase()
    {
        lock (_phaseLock)
        {
            _outputCoordinator.SetProgressController(NoOpProgressController.Instance);
            _outputCoordinator.SetProgressActive(false);
            _isProgressActive = false;
            _activeSession = null;
        }
    }

    internal static AnsiConsoleSettings CreateAnsiConsoleSettings(TextWriter output, bool isKnownBuildAgent)
    {
        var settings = new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(output),
        };

        if (isKnownBuildAgent)
        {
            settings.Ansi = AnsiSupport.Yes;
            settings.ColorSystem = ColorSystemSupport.Standard;
            settings.Interactive = InteractionSupport.No;
        }

        return settings;
    }

    private bool UsesGlobalAnsiConsole => ReferenceEquals(_ansiConsole, DelegatingAnsiConsole.Instance);

    private async Task<IReadOnlyList<IModuleOutputBuffer>> FlushPendingWritesAsync(OutputFlushKind flushKind)
    {
        var populatedBeforeFlush = _moduleBuffers.Values
            .Where(buffer => buffer.HasOutput)
            .ToHashSet();

        CoordinatedTextWriter? output;
        CoordinatedTextWriter? error;
        lock (_phaseLock)
        {
            output = _coordinatedOut;
            error = _coordinatedError;
        }

        if (output is not null)
        {
            await FlushWriterAsync(output, flushKind).ConfigureAwait(false);
        }

        if (error is not null && !ReferenceEquals(error, output))
        {
            await FlushWriterAsync(error, flushKind).ConfigureAwait(false);
        }

        return _moduleBuffers.Values
            .Where(buffer => buffer.HasOutput && !populatedBeforeFlush.Contains(buffer))
            .Cast<IModuleOutputBuffer>()
            .ToArray();
    }

    private async Task FlushBufferSafelyAsync(
        IModuleOutputBuffer buffer,
        OutputFlushKind flushKind,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            if (flushKind is OutputFlushKind.Complete)
            {
                await _outputCoordinator
                    .OnModuleCompletedAsync(buffer, buffer.ModuleType, cancellationToken)
                    .ConfigureAwait(false);
            }
            else
            {
                await _outputCoordinator
                    .EnqueueAndFlushAsync(buffer, flushKind, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            ReportFlushFailure(buffer, exception);
        }
    }

    private void RequestThresholdFlush(IModuleOutputBuffer buffer)
    {
        // EnqueueAndFlushAsync registers pending work before its first await, so teardown sees this flush.
        _ = FlushThresholdAsync(buffer);
    }

    private async Task FlushThresholdAsync(IModuleOutputBuffer buffer)
    {
        try
        {
            await _outputCoordinator
                .EnqueueAndFlushAsync(buffer, OutputFlushKind.Incremental)
                .ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            ReportFlushFailure(buffer, exception);
        }
    }

    private void ReportFlushFailure(IModuleOutputBuffer buffer, Exception exception)
    {
        try
        {
            var logger = _outputLogger ?? _loggerFactory.CreateLogger<ConsoleCoordinator>();
            logger.LogWarning(
                exception,
                "Failed to flush output for {ModuleType}",
                buffer.ModuleType);
        }
        catch
        {
            // The output provider itself may be the failing sink. Preserve a fallback
            // diagnostic without preventing unrelated buffers from being flushed.
            _deferredExceptions.Enqueue(
                $"Failed to flush output for {buffer.ModuleType}: {exception}");
        }
    }

    private static Task FlushWriterAsync(CoordinatedTextWriter writer, OutputFlushKind flushKind)
    {
        return flushKind is OutputFlushKind.Complete
            ? writer.FlushAsync()
            : writer.FlushAvailableAsync();
    }

    private void ConfigureConsoleWidth()
    {
        var configuredWidth = _options.Value.Console.Width;

        if (configuredWidth.HasValue)
        {
            // User explicitly configured a width
            _ansiConsole.Profile.Width = configuredWidth.Value;
        }
        else if (_buildSystemDetector.IsKnownBuildAgent)
        {
            // Running in a known CI environment - use expanded width
            // CI environments typically don't have a TTY, causing Spectre.Console to default to 80 chars
            _ansiConsole.Profile.Width = DefaultCiConsoleWidth;
        }

        // Otherwise, leave Spectre.Console's auto-detected width (works well for local terminals)
    }
}
