using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ModularPipelines.Helpers.Internal;

internal readonly record struct ProcessRunResult(int ExitCode, DateTimeOffset StartTime, DateTimeOffset ExitTime)
{
    public TimeSpan RunTime => ExitTime - StartTime;
}

/// <summary>
/// Starts a command, streams its output to the targets, and waits for both the process and its output to end.
/// </summary>
internal static class ProcessRunner
{
    private const int BufferSize = 4096;

    /// <summary>
    /// Runs the command to completion.
    /// </summary>
    /// <param name="command">The command to run.</param>
    /// <param name="standardOutput">Receives standard output.</param>
    /// <param name="standardError">Receives standard error.</param>
    /// <param name="onStarted">Called once the process has started, before its output is read.</param>
    /// <param name="interruptCancellationToken">
    /// Interrupts the process when canceled: SIGINT on Unix. The caller owns forceful termination.
    /// </param>
    /// <exception cref="OperationCanceledException">The interrupt token was canceled; thrown once the process exits.</exception>
    public static async Task<ProcessRunResult> RunAsync(
        ProcessCommand command,
        CommandOutputTarget standardOutput,
        CommandOutputTarget standardError,
        Action<Process> onStarted,
        CancellationToken interruptCancellationToken)
    {
        using var process = new Process { StartInfo = command.CreateStartInfo() };
        process.Start();
        var startTime = DateTimeOffset.Now;
        onStarted(process);

        // Commands get no input, so tools that read standard input see its end instead of waiting.
        CloseStandardInput(process);

        var outputTask = ReadOutputAsync(process, process.StandardOutput.BaseStream, standardOutput);
        var errorTask = ReadOutputAsync(process, process.StandardError.BaseStream, standardError);

        var interruptRegistration = interruptCancellationToken.Register(
            static state => Interrupt((Process) state!),
            process);
        await using (interruptRegistration.ConfigureAwait(false))
        {
            try
            {
                await Task.WhenAll(outputTask, errorTask).ConfigureAwait(false);
            }
            finally
            {
                await process.WaitForExitAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }

        var exitTime = DateTimeOffset.Now;
        if (interruptCancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException(
                $"Command execution canceled. Underlying process ({Path.GetFileName(command.TargetFilePath)}#{process.Id}) was interrupted.",
                interruptCancellationToken);
        }

        return new ProcessRunResult(process.ExitCode, startTime, exitTime);
    }

    private static void CloseStandardInput(Process process)
    {
        try
        {
            process.StandardInput.Close();
        }
        catch (IOException)
        {
            // The process already exited and closed its end of the pipe.
        }
    }

    private static async Task ReadOutputAsync(Process process, Stream stream, CommandOutputTarget target)
    {
        try
        {
            using var reader = new StreamReader(
                stream,
                Encoding.Default,
                detectEncodingFromByteOrderMarks: true,
                bufferSize: BufferSize,
                leaveOpen: true);
            var characters = new char[BufferSize];
            int charactersRead;
            while ((charactersRead = await reader.ReadAsync(characters.AsMemory()).ConfigureAwait(false)) > 0)
            {
                target.Append(characters.AsSpan(0, charactersRead));
            }

            target.Complete();
        }
        catch
        {
            // Nothing drains the pipe any more, so a still-running process could block on a full buffer.
            TryKill(process);
            throw;
        }
    }

    private static void Interrupt(Process process)
    {
        // Windows has no SIGINT, and commands run in their own process group, so console Ctrl+C does not
        // reach them. The caller's forceful termination ends them after the graceful shutdown timeout.
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        try
        {
            process.Signal(PosixSignal.SIGINT);
        }
        catch (Exception exception) when (exception is InvalidOperationException or Win32Exception or PlatformNotSupportedException)
        {
            // The process already exited.
        }
    }

    private static void TryKill(Process process)
    {
        try
        {
            ProcessTree.Kill(process);
        }
        catch (Exception exception) when (exception is InvalidOperationException or Win32Exception or NotSupportedException or AggregateException)
        {
            // The process already exited or could not be terminated.
        }
    }
}
