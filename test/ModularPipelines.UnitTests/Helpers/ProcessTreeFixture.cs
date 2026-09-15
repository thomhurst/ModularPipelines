using System.Diagnostics;
using System.ComponentModel;
using System.Text.Json;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.ProcessTestHost;

namespace ModularPipelines.UnitTests.Helpers;

internal sealed class ProcessTreeFixture : IAsyncDisposable
{
    public string DirectoryPath { get; } = Directory.CreateTempSubdirectory("modular-pipelines-process-tree-").FullName;
    private readonly CancellationTokenSource _cancellation = new();
    private readonly Dictionary<ProcessIdentity, Process> _processes = [];
    private readonly TaskCompletionSource _forcefulCancellationReady = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private bool _disposed;

    public Task<CommandResult> Execution { get; private set; } = null!;

    public void Start(ICommandContext command, string role, TimeSpan gracefulTimeout, bool gateForcefulCancellation = false)
    {
        var runtimeConfiguration = Path.ChangeExtension(typeof(CommandTests).Assembly.Location, ".runtimeconfig.json");
        Execution = command.ExecuteCommandLineToolAsync(new CommandLineToolOptions("dotnet")
        {
            Arguments = ["exec", "--runtimeconfig", runtimeConfiguration, typeof(Host).Assembly.Location, role, DirectoryPath, runtimeConfiguration],
        }, new CommandExecutionOptions
        {
            GracefulShutdownTimeout = gracefulTimeout,
            InternalForcefulCancellationReady = gateForcefulCancellation ? _forcefulCancellationReady.Task : null,
        }, _cancellation.Token);
    }

    public void Cancel() => _cancellation.Cancel();

    public void AllowForcefulCancellation() => _forcefulCancellationReady.TrySetResult();

    public Task TriggerAsync(string name) => File.WriteAllTextAsync(Path.Combine(DirectoryPath, name + ".trigger"), string.Empty);

    public async Task<Process> WaitForProcessAsync(string name, TimeSpan timeout)
    {
        await WaitForFileAsync(name + ".pid", timeout);
        var identity = JsonSerializer.Deserialize<ProcessIdentity>(await File.ReadAllTextAsync(Path.Combine(DirectoryPath, name + ".pid")));
        if (!_processes.TryGetValue(identity, out var process))
        {
            process = TryOpenProcess(identity) ?? throw new InvalidOperationException($"Fixture process {name} exited before its identity could be retained. {DescribeState()}");
            _processes.Add(identity, process);
        }

        if (process.HasExited)
        {
            throw new InvalidOperationException($"Fixture process {name} exited before cancellation. {DescribeState()}");
        }

        return process;
    }

    public async Task<Process> WaitForReadyProcessAsync(string name, TimeSpan timeout)
    {
        var stopwatch = Stopwatch.StartNew();
        await WaitForReadyAsync(name, timeout);
        var remaining = timeout - stopwatch.Elapsed;
        return await WaitForProcessAsync(name, remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero);
    }

    public Task WaitForReadyAsync(string name, TimeSpan timeout) => WaitForFileAsync(name + ".ready", timeout);

    private async Task WaitForFileAsync(string name, TimeSpan timeout)
    {
        using var deadline = new CancellationTokenSource(timeout);
        while (!File.Exists(Path.Combine(DirectoryPath, name)))
        {
            if (Directory.EnumerateFiles(DirectoryPath, "*.error").Any())
            {
                throw new InvalidOperationException($"Process fixture failed before publishing {name}. {DescribeState()}");
            }

            if (Execution.IsCompleted)
            {
                CommandResult result;
                try
                {
                    result = await Execution;
                }
                catch (Exception exception)
                {
                    throw new InvalidOperationException($"Process fixture failed before publishing {name}. {DescribeState()}", exception);
                }

                throw new InvalidOperationException($"Process fixture exited with code {result.ExitCode} before publishing {name}. {DescribeState()}");
            }

            try
            {
                await Task.Delay(20, deadline.Token);
            }
            catch (OperationCanceledException exception) when (deadline.IsCancellationRequested)
            {
                if (File.Exists(Path.Combine(DirectoryPath, name)))
                {
                    return;
                }

                throw new TimeoutException($"Process fixture did not publish {name} within {timeout}. Execution: {Execution.Status}. {DescribeState()}", exception);
            }
        }
    }

    private string DescribeState() => string.Join(Environment.NewLine,
        Directory.EnumerateFiles(DirectoryPath).Where(path => !path.EndsWith(".tmp", StringComparison.Ordinal))
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
            Directory.Delete(DirectoryPath, recursive: true);
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
        catch (Exception exception) when (exception is OperationCanceledException or CommandException or Win32Exception)
        {
            // Readiness reports command failures; cleanup still observes execution completion.
        }
    }

    private async Task StopPublishedProcessesAsync()
    {
        CapturePublishedProcesses();
        List<Exception> cleanupErrors = [];
        foreach (var (identity, process) in _processes)
        {
            try
            {
                using var currentProcess = TryOpenProcess(identity);
                if (currentProcess is { HasExited: false })
                {
                    currentProcess.Kill(entireProcessTree: true);
                    await currentProcess.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(5));
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
            throw new AggregateException($"Process fixture cleanup failed. Diagnostics remain in {DirectoryPath}.", cleanupErrors);
        }
    }

    private void CapturePublishedProcesses()
    {
        foreach (var path in Directory.EnumerateFiles(DirectoryPath, "*.pid"))
        {
            var identity = JsonSerializer.Deserialize<ProcessIdentity>(File.ReadAllText(path));
            if (_processes.ContainsKey(identity))
            {
                continue;
            }

            var process = TryOpenProcess(identity);
            if (process is not null)
            {
                _processes.Add(identity, process);
            }
        }
    }

    private static Process? TryOpenProcess(ProcessIdentity identity)
    {
        Process? process = null;
        try
        {
            process = Process.GetProcessById(identity.Id);
            _ = process.SafeHandle;
            if (ProcessIdentity.Capture(process) == identity)
            {
                return process;
            }
        }
        catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or Win32Exception)
        {
            // An exited or inaccessible process cannot be verified as belonging to this fixture.
        }

        process?.Dispose();
        return null;
    }
}
