using Microsoft.Extensions.Logging;
using ModularPipelines.Helpers;
using ModularPipelines.Options;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

internal class Downloader : IDownloader
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly HttpClient _defaultHttpClient;

    public Downloader( IModuleLoggerProvider moduleLoggerProvider, HttpClient defaultHttpClient )
    {
        _moduleLoggerProvider = moduleLoggerProvider;
        _defaultHttpClient = defaultHttpClient;
    }

    public async Task<File> DownloadFileAsync( DownloadOptions options, CancellationToken cancellationToken = default )
    {
        var request = new HttpRequestMessage( HttpMethod.Get, options.DownloadUri );

        options.RequestConfigurator?.Invoke( request );

        var response = await (options.HttpClient ?? _defaultHttpClient).GetAsync( options.DownloadUri, cancellationToken );

        await using var stream = await response.Content.ReadAsStreamAsync( cancellationToken );

        var filePathToSave = GetSaveLocation( options );

        if (!options.Overwrite && System.IO.File.Exists( filePathToSave ))
        {
            throw new IOException( $"{filePathToSave} already exists and overwrite is false" );
        }

        await using var newFile = System.IO.File.Create( filePathToSave );

        await stream.CopyToAsync( newFile, cancellationToken );

        _moduleLoggerProvider.GetLogger().LogInformation( "File {Uri} downloaded to {SaveLocation}", options.DownloadUri, filePathToSave );

        return filePathToSave!;
    }

    private string GetSaveLocation( DownloadOptions options )
    {
        if (string.IsNullOrWhiteSpace( options.SavePath ))
        {
            return Path.GetTempFileName() + Guid.NewGuid() + GetExtension( options.DownloadUri.AbsoluteUri );
        }

        if (Path.HasExtension( options.SavePath ))
        {
            Directory.CreateDirectory( new FileInfo( options.SavePath ).Directory!.FullName );
            return options.SavePath;
        }

        Directory.CreateDirectory( options.SavePath );
        return Path.Combine( options.SavePath, Guid.NewGuid() + GetExtension( options.DownloadUri.AbsoluteUri ) );
    }

    private string GetExtension( string downloadUri )
    {
        if (Path.HasExtension( downloadUri ))
        {
            return Path.GetExtension( downloadUri );
        }

        return string.Empty;
    }
}
