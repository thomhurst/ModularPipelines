using System.Diagnostics;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.ProcessTestHost;

namespace ModularPipelines.UnitTests.Helpers;

internal sealed class ProcessTreeFixture : IAsyncDisposable
{
    private readonly string _directory = Directory.CreateTempSubdirectory("modular-pipelines-process-tree-").FullName;
    private readonly CancellationTokenSource _cancellation = new();
    private readonly Dictionary<int, Process> _processes = [];
    private readonly TaskCompletionSource _forcefulCancellationReady = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private bool _disposed;

    public Task<CommandResult> Execution { get; private set; } = null!;

    public void Start(ICommandContext command, string role, TimeSpan gracefulTimeout, bool gateForcefulCancellation = false)
    {
        var runtimeConfiguration = Path.ChangeExtension(typeof(CommandTests).Assembly.Location, ".runtimeconfig.json");
        Execution = command.ExecuteCommandLineToolAsync(new CommandLineToolOptions("dotnet")
        {
            Arguments = ["exec", "--runtimeconfig", runtimeConfiguration, typeof(Host).Assembly.Location, role, _directory, runtimeConfiguration],
        }, new CommandExecutionOptions
        {
            GracefulShutdownTimeout = gracefulTimeout,
            InternalForcefulCancellationReady = gateForcefulCancellation ? _forcefulCancellationReady.Task : null,
        }, _cancellation.Token);
    }

    public void Cancel() => _cancellation.Cancel();

    public void AllowForcefulCancellation() => _forcefulCancellationReady.TrySetResult();

    public Task TriggerAsync(string name) => File.WriteAllTextAsync(Path.Combine(_directory, name + ".trigger"), string.Empty);

    public async Task<Process> WaitForProcessAsync(string name, TimeSpan timeout)
    {
        await WaitForFileAsync(name + ".pid", timeout);
        var processId = int.Parse(await File.ReadAllTextAsync(Path.Combine(_directory, name + ".pid")));
        if (!_processes.TryGetValue(processId, out var process))
        {
            process = Process.GetProcessById(processId);
            _processes.Add(processId, process);
        }

        return process;
    }

    public Task WaitForReadyAsync(string name, TimeSpan timeout) => WaitForFileAsync(name + ".ready", timeout);

    private async Task WaitForFileAsync(string name, TimeSpan timeout)
    {
        using var deadline = new CancellationTokenSource(timeout);
        while (!File.Exists(Path.Combine(_directory, name)))
        {
            if (Execution.IsCompleted)
            {
                try
                {
                    var result = await Execution;
                    throw new InvalidOperationException($"Process fixture exited with code {result.ExitCode} before publishing {name}. {DescribeState()}");
                }
                catch (CommandException exception)
                {
                    throw new InvalidOperationException($"Process fixture failed before publishing {name}. {DescribeState()}", exception);
                }
            }

            try
            {
                await Task.Delay(20, deadline.Token);
            }
            catch (OperationCanceledException exception) when (deadline.IsCancellationRequested)
            {
                throw new TimeoutException($"Process fixture did not publish {name} within {timeout}. Execution: {Execution.Status}. {DescribeState()}", exception);
            }
        }
    }

    private string DescribeState() => string.Join(Environment.NewLine,
        Directory.EnumerateFiles(_directory).Where(path => !path.EndsWith(".tmp", StringComparison.Ordinal))
            .Select(path => $"{Path.GetFileName(path)}: {File.ReadAllText(path)} (published {File.GetLastWriteTimeUtc(path):O})"));

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        CapturePublishedProcesses();
        _cancellation.Cancel();
        _forcefulCancellationReady.TrySetResult();
        try
        {
            await ObserveExecutionAsync();
        }
        catch (TimeoutException)
        {
            // Retained process handles provide a fallback if command cancellation stalls.
        }
        finally
        {
            try
            {
                await StopPublishedProcessesAsync();
            }
            finally
            {
                _cancellation.Dispose();
            }

            await ObserveExecutionAsync();
            Directory.Delete(_directory, recursive: true);
        }
    }

    private async Task ObserveExecutionAsync()
    {
        if (Execution is null)
        {
            return;
        }

        try
        {
            await Execution.WaitAsync(TimeSpan.FromSeconds(5));
        }
        catch (Exception exception) when (exception is OperationCanceledException or CommandException)
        {
            // Readiness reports command failures; cleanup still observes execution completion.
        }
    }

    private async Task StopPublishedProcessesAsync()
    {
        CapturePublishedProcesses();
        List<Exception> cleanupErrors = [];
        foreach (var process in _processes.Values)
        {
            try
            {
                if (!process.HasExited)
                {
                    process.Kill(entireProcessTree: true);
                    await process.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(5));
                }
            }
            catch (InvalidOperationException) when (process.HasExited)
            {
                // The process exited between the liveness check and Kill.
            }
            catch (Exception exception)
            {
                cleanupErrors.Add(exception);
            }
            finally
            {
                process.Dispose();
            }
        }

        if (cleanupErrors.Count > 0)
        {
            throw new AggregateException($"Process fixture cleanup failed. Diagnostics remain in {_directory}.", cleanupErrors);
        }
    }

    private void CapturePublishedProcesses()
    {
        foreach (var path in Directory.EnumerateFiles(_directory, "*.pid"))
        {
            var processId = int.Parse(File.ReadAllText(path));
            if (_processes.ContainsKey(processId))
            {
                continue;
            }

            try
            {
                _processes.Add(processId, Process.GetProcessById(processId));
            }
            catch (ArgumentException)
            {
                // The process already exited before cleanup acquired its handle.
            }
        }
    }
}
