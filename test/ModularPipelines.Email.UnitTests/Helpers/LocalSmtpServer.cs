using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ModularPipelines.Email.UnitTests.Helpers;

internal sealed class LocalSmtpServer : IAsyncDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource = new(TimeSpan.FromSeconds(10));
    private readonly TcpListener _listener;
    private readonly TaskCompletionSource<string> _message =
        new(TaskCreationOptions.RunContinuationsAsynchronously);
    private readonly Task _serverTask;

    private LocalSmtpServer()
    {
        _listener = new TcpListener(IPAddress.Loopback, 0);
        _listener.Start();
        _serverTask = RunAsync(_cancellationTokenSource.Token);
    }

    public int Port => ((IPEndPoint) _listener.LocalEndpoint).Port;

    public Task<string> Message => _message.Task;

    public static LocalSmtpServer Start() => new();

    public async ValueTask DisposeAsync()
    {
        await _cancellationTokenSource.CancelAsync();
        _listener.Stop();
        await _serverTask;
        _cancellationTokenSource.Dispose();
    }

    private async Task RunAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var client = await _listener.AcceptTcpClientAsync(cancellationToken);
            await using var stream = client.GetStream();
            using var reader = new StreamReader(stream, Encoding.ASCII);
            await using var writer = new StreamWriter(stream, Encoding.ASCII)
            {
                AutoFlush = true,
                NewLine = "\r\n",
            };

            await writer.WriteLineAsync("220 localhost ESMTP ready");

            while (await reader.ReadLineAsync(cancellationToken) is { } command)
            {
                if (command.StartsWith("EHLO ", StringComparison.OrdinalIgnoreCase)
                    || command.StartsWith("HELO ", StringComparison.OrdinalIgnoreCase))
                {
                    await writer.WriteLineAsync("250-localhost");
                    await writer.WriteLineAsync("250 SIZE 10485760");
                }
                else if (command.Equals("DATA", StringComparison.OrdinalIgnoreCase))
                {
                    await writer.WriteLineAsync("354 End data with <CR><LF>.<CR><LF>");
                    _message.TrySetResult(await ReadMessageAsync(reader, cancellationToken));
                    await writer.WriteLineAsync("250 2.0.0 queued");
                }
                else if (command.Equals("QUIT", StringComparison.OrdinalIgnoreCase))
                {
                    await writer.WriteLineAsync("221 2.0.0 Bye");
                    return;
                }
                else
                {
                    await writer.WriteLineAsync("250 2.0.0 OK");
                }
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (SocketException) when (cancellationToken.IsCancellationRequested)
        {
        }
        finally
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _message.TrySetCanceled(cancellationToken);
            }
            else
            {
                _message.TrySetException(
                    new InvalidOperationException("SMTP connection closed before message data was received."));
            }
        }
    }

    private static async Task<string> ReadMessageAsync(
        StreamReader reader,
        CancellationToken cancellationToken)
    {
        var lines = new List<string>();

        while (await reader.ReadLineAsync(cancellationToken) is { } line && line != ".")
        {
            lines.Add(line.StartsWith("..", StringComparison.Ordinal) ? line[1..] : line);
        }

        return string.Join("\r\n", lines);
    }
}
