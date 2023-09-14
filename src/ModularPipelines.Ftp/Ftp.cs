using FluentFTP;
using ModularPipelines.Ftp.Options;
using ModularPipelines.Helpers;

namespace ModularPipelines.Ftp;

internal class Ftp : IAsyncDisposable, IFtp
{
	private readonly List<AsyncFtpClient> _clients = new();
	
	public async Task<AsyncFtpClient> GetFtpClientAsync(FtpOptions options)
	{
		var client = new AsyncFtpClient(options.Host, options.Credentials);

		options.ClientConfigurator?.Invoke(client);
		
		await client.AutoConnect();
		
		_clients.Add(client);

		return client;
	}

	public async ValueTask DisposeAsync()
	{
		foreach (var asyncFtpClient in _clients)
		{
			await Disposer.DisposeObjectAsync(asyncFtpClient);
		}
	}
}