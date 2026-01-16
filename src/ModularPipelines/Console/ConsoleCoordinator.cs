using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
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
    private readonly IOptions<PipelineOptions> _options;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IBuildSystemDetector _buildSystemDetector;

    // Console state
    private TextWriter? _originalConsoleOut;
    private TextWriter? _originalConsoleError;
    private IAnsiConsole? _originalAnsiConsole;
    private CoordinatedTextWriter? _coordinatedOut;
    private CoordinatedTextWriter? _coordinatedError;

    // Phase management
    private volatile bool _isProgressActive;
    private readonly object _phaseLock = new();
    private bool _isInstalled;
    private IProgressSession? _activeSession;

    // Module buffers
    private readonly ConcurrentDictionary<Type, ModuleOutputBuffer> _moduleBuffers = new();
    private readonly ModuleOutputBuffer _unattributedBuffer;

    // Deferred exceptions
    private readonly ConcurrentQueue<string> _deferredExceptions = new();

    // Logger for output
    private ILogger? _outputLogger;

    public ConsoleCoordinator(
        IBuildSystemFormatterProvider formatterProvider,
        IResultsPrinter resultsPrinter,
        ISecretObfuscator secretObfuscator,
        IOptions<PipelineOptions> options,
        ILoggerFactory loggerFactory,
        IBuildSystemDetector buildSystemDetector)
    {
        _formatterProvider = formatterProvider;
        _resultsPrinter = resultsPrinter;
        _secretObfuscator = secretObfuscator;
        _options = options;
        _loggerFactory = loggerFactory;
        _buildSystemDetector = buildSystemDetector;
        _unattributedBuffer = new ModuleOutputBuffer("Pipeline", typeof(void));
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
                AnsiConsole.Console = AnsiConsole.Create(new AnsiConsoleSettings
                {
                    Out = new AnsiConsoleOutput(_originalConsoleOut)
                });

                // Configure console width for CI environments
                // Spectre.Console defaults to 80 characters when it can't detect terminal width,
                // which is common in CI environments where output is redirected
                ConfigureConsoleWidth();

                // Create logger for structured output during flush
                _outputLogger = _loggerFactory.CreateLogger("ModularPipelines.Output");

                // Install our intercepting writers
                _coordinatedOut = new CoordinatedTextWriter(
                    this,
                    _originalConsoleOut,
                    () => _isProgressActive,
                    _secretObfuscator);

                _coordinatedError = new CoordinatedTextWriter(
                    this,
                    _originalConsoleError,
                    () => _isProgressActive,
                    _secretObfuscator);

                System.Console.SetOut(_coordinatedOut);
                System.Console.SetError(_coordinatedError);

                _isInstalled = true;
            }
            catch
            {
                // Restore original streams on failure
                System.Console.SetOut(originalOut);
                System.Console.SetError(originalError);
                AnsiConsole.Console = originalAnsi;

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

            if (_originalAnsiConsole != null)
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
    public async Task<IProgressSession> BeginProgressAsync(
        OrganizedModules modules,
        CancellationToken cancellationToken)
    {
        if (!_options.Value.ShowProgressInConsole)
        {
            // Return a no-op session if progress is disabled
            _activeSession = new NoOpProgressSession();
            return _activeSession;
        }

        lock (_phaseLock)
        {
            if (_isProgressActive)
            {
                throw new InvalidOperationException("Progress session is already active.");
            }

            _isProgressActive = true;
        }

        var session = new ProgressSession(
            this,
            modules,
            _options,
            cancellationToken);

        _activeSession = session;

        // Start the progress display
        session.Start();

        return session;
    }

    /// <summary>
    /// Ends the progress phase. Called by ProgressSession.DisposeAsync().
    /// </summary>
    internal void EndProgressPhase()
    {
        lock (_phaseLock)
        {
            _isProgressActive = false;
            _activeSession = null;
        }
    }

    /// <inheritdoc />
    public IModuleOutputBuffer GetModuleBuffer(Type moduleType)
    {
        return _moduleBuffers.GetOrAdd(moduleType, t => new ModuleOutputBuffer(t));
    }

    /// <inheritdoc />
    public IModuleOutputBuffer GetUnattributedBuffer() => _unattributedBuffer;

    /// <inheritdoc />
    public void FlushModuleOutput()
    {
        if (_originalConsoleOut == null)
        {
            throw new InvalidOperationException("ConsoleCoordinator is not installed.");
        }

        var formatter = _formatterProvider.GetFormatter();

        // Flush unattributed output first (if any)
        if (_unattributedBuffer.HasOutput)
        {
            var unattributedLogger = _outputLogger ?? _loggerFactory.CreateLogger("ModularPipelines.Output");
            _unattributedBuffer.FlushTo(_originalConsoleOut, formatter, unattributedLogger);
        }

        // Flush module buffers in completion order
        var orderedBuffers = _moduleBuffers.Values
            .Where(b => b.HasOutput)
            .OrderBy(b => b.CompletedAtUtc ?? DateTime.MaxValue)
            .ToList();

        foreach (var buffer in orderedBuffers)
        {
            // Create logger with the correct module category for proper log replay
            var moduleLogger = _loggerFactory.CreateLogger(buffer.ModuleType);
            buffer.FlushTo(_originalConsoleOut, formatter, moduleLogger);
        }

        // Clear buffers after flush to release memory
        // This prevents accumulation in long-running pipelines
        _moduleBuffers.Clear();
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
                FlushModuleOutput();
            }
            catch (InvalidOperationException)
            {
                // Ignore if not installed - FlushModuleOutput requires installation
            }
        }

        Uninstall();
        await Task.CompletedTask;
    }

    private void ConfigureConsoleWidth()
    {
        var configuredWidth = _options.Value.ConsoleWidth;

        if (configuredWidth.HasValue)
        {
            // User explicitly configured a width
            AnsiConsole.Console.Profile.Width = configuredWidth.Value;
        }
        else if (_buildSystemDetector.IsKnownBuildAgent)
        {
            // Running in a known CI environment - use expanded width
            // CI environments typically don't have a TTY, causing Spectre.Console to default to 80 chars
            AnsiConsole.Console.Profile.Width = DefaultCiConsoleWidth;
        }

        // Otherwise, leave Spectre.Console's auto-detected width (works well for local terminals)
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
}
