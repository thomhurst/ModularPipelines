using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ModularPipelines.Ftp.UnitTests.Helpers;

internal sealed class LocalFtpServer : IAsyncDisposable
{
    public const string RemotePath = "/fixture.txt";
    public const string Contents = "local FTP fixture\r\n";

    private static readonly byte[] ContentBytes = Encoding.UTF8.GetBytes(Contents);
    private readonly CancellationTokenSource _cancellationTokenSource = new(TimeSpan.FromSeconds(15));
    private readonly ConcurrentQueue<string> _commands = new();
    private readonly TcpListener _listener;
    private readonly Task _serverTask;
    private TcpListener? _dataListener;

    private LocalFtpServer()
    {
        _listener = new TcpListener(IPAddress.Loopback, 0);
        _listener.Start();
        _serverTask = RunAsync(_cancellationTokenSource.Token);
    }

    public int Port => ((IPEndPoint) _listener.LocalEndpoint).Port;

    public IReadOnlyCollection<string> Commands => _commands.ToArray();

    public static LocalFtpServer Start() => new();

    public async ValueTask DisposeAsync()
    {
        await _cancellationTokenSource.CancelAsync();
        _dataListener?.Stop();
        _listener.Stop();
        await _serverTask;
        _cancellationTokenSource.Dispose();
    }

    private async Task RunAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using var client = await _listener.AcceptTcpClientAsync(cancellationToken);
                await ServeClientAsync(client, cancellationToken);
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (SocketException) when (cancellationToken.IsCancellationRequested)
        {
        }
    }

    private async Task ServeClientAsync(TcpClient client, CancellationToken cancellationToken)
    {
        await using var stream = client.GetStream();
        using var reader = new StreamReader(stream, Encoding.ASCII);
        await using var writer = new StreamWriter(stream, Encoding.ASCII)
        {
            AutoFlush = true,
            NewLine = "\r\n",
        };

        await writer.WriteLineAsync("220 localhost FTP ready");

        while (await reader.ReadLineAsync(cancellationToken) is { } command)
        {
            _commands.Enqueue(command);
            var separator = command.IndexOf(' ');
            var verb = (separator < 0 ? command : command[..separator]).ToUpperInvariant();
            var argument = separator < 0 ? string.Empty : command[(separator + 1)..];

            switch (verb)
            {
                case "USER":
                    await writer.WriteLineAsync("331 Password required");
                    break;
                case "PASS":
                    await writer.WriteLineAsync("230 Logged on");
                    break;
                case "FEAT":
                    await writer.WriteLineAsync("211-Extensions supported:");
                    await writer.WriteLineAsync(" EPSV");
                    await writer.WriteLineAsync(" MDTM");
                    await writer.WriteLineAsync(" SIZE");
                    await writer.WriteLineAsync(" UTF8");
                    await writer.WriteLineAsync("211 End");
                    break;
                case "SYST":
                    await writer.WriteLineAsync("215 UNIX Type: L8");
                    break;
                case "PWD":
                    await writer.WriteLineAsync("257 \"/\" is current directory");
                    break;
                case "TYPE":
                case "OPTS":
                case "NOOP":
                    await writer.WriteLineAsync("200 Command okay");
                    break;
                case "SIZE":
                    await writer.WriteLineAsync(argument == RemotePath
                        ? $"213 {ContentBytes.Length}"
                        : "550 File unavailable");
                    break;
                case "MDTM":
                    await writer.WriteLineAsync(argument == RemotePath
                        ? "213 20260101000000"
                        : "550 File unavailable");
                    break;
                case "REST":
                    await writer.WriteLineAsync("350 Restart position accepted");
                    break;
                case "PASV":
                    await StartPassiveListenerAsync(writer, extended: false);
                    break;
                case "EPSV":
                    await StartPassiveListenerAsync(writer, extended: true);
                    break;
                case "RETR":
                    await SendFileAsync(writer, argument, cancellationToken);
                    break;
                case "AUTH":
                    await writer.WriteLineAsync("502 TLS not supported");
                    break;
                case "QUIT":
                    await writer.WriteLineAsync("221 Bye");
                    return;
                default:
                    await writer.WriteLineAsync("200 Command okay");
                    break;
            }
        }
    }

    private async Task StartPassiveListenerAsync(StreamWriter writer, bool extended)
    {
        _dataListener?.Stop();
        _dataListener = new TcpListener(IPAddress.Loopback, 0);
        _dataListener.Start();
        var port = ((IPEndPoint) _dataListener.LocalEndpoint).Port;

        await writer.WriteLineAsync(extended
            ? $"229 Entering Extended Passive Mode (|||{port}|)"
            : $"227 Entering Passive Mode (127,0,0,1,{port / 256},{port % 256})");
    }

    private async Task SendFileAsync(
        StreamWriter writer,
        string remotePath,
        CancellationToken cancellationToken)
    {
        if (remotePath != RemotePath)
        {
            await writer.WriteLineAsync("550 File unavailable");
            return;
        }

        if (_dataListener is null)
        {
            await writer.WriteLineAsync("425 Use PASV or EPSV first");
            return;
        }

        await writer.WriteLineAsync("150 Opening binary data connection");
        using var dataClient = await _dataListener.AcceptTcpClientAsync(cancellationToken);
        await using (var dataStream = dataClient.GetStream())
        {
            await dataStream.WriteAsync(ContentBytes, cancellationToken);
        }

        _dataListener.Stop();
        _dataListener = null;
        await writer.WriteLineAsync("226 Transfer complete");
    }
}
