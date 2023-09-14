using FluentFTP;
using ModularPipelines.Ftp.Options;

namespace ModularPipelines.Ftp;

public interface IFtp
{
    Task<AsyncFtpClient> GetFtpClientAsync(FtpOptions options);
}