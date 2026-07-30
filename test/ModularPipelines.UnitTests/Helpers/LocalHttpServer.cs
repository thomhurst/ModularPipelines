using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ModularPipelines.UnitTests.Helpers;

internal sealed class LocalHttpServer : IAsyncDisposable
{
    private static readonly byte[] DefaultResponseBody = "local HTTP response"u8.ToArray();

    private readonly CancellationTokenSource _cancellationTokenSource = new(TimeSpan.FromSeconds(10));
    private readonly TcpListener _listener;
    private readonly byte[] _responseBody;
    private readonly Task _serverTask;

    private LocalHttpServer(byte[] responseBody)
    {
        _responseBody = responseBody;
        _listener = new TcpListener(IPAddress.Loopback, 0);
        _listener.Start();
        Uri = new Uri($"http://127.0.0.1:{((IPEndPoint) _listener.LocalEndpoint).Port}/fixture.bin");
        _serverTask = RunAsync(_cancellationTokenSource.Token);
    }

    public Uri Uri { get; }

    public static LocalHttpServer Start(byte[]? responseBody = null) =>
        new(responseBody ?? DefaultResponseBody);

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
            using var reader = new StreamReader(
                stream,
                Encoding.ASCII,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);

            while (!string.IsNullOrEmpty(await reader.ReadLineAsync(cancellationToken)))
            {
            }

            var headers = Encoding.ASCII.GetBytes(
                "HTTP/1.1 200 OK\r\n" +
                "Server: LocalHttpServer\r\n" +
                "Content-Type: application/octet-stream\r\n" +
                $"Content-Length: {_responseBody.Length}\r\n" +
                "Connection: close\r\n\r\n");

            await stream.WriteAsync(headers, cancellationToken);
            await stream.WriteAsync(_responseBody, cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (SocketException) when (cancellationToken.IsCancellationRequested)
        {
        }
    }
}
