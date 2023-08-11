using FluentFTP;
using ModularPipelines.Ftp.Options;

namespace ModularPipelines.Ftp;

internal class Ftp : IAsyncDisposable, IFtp
{
	private readonly List<AsyncFtpClient> _clients = new();
	
	public async Task<AsyncFtpClient> GetFtpClient(FtpOptions downloadOptions)
	{
		var client = new AsyncFtpClient(downloadOptions.Host, downloadOptions.Credentials);
        
		await client.AutoConnect();
		
		_clients.Add(client);

		return client;
	}

	public async ValueTask DisposeAsync()
	{
		foreach (var asyncFtpClient in _clients)
		{
			await asyncFtpClient.Disconnect();
			asyncFtpClient.Dispose();
		}
	}
}