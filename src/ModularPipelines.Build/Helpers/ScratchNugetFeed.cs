using System.IO.Compression;
using System.Net;
using System.Xml.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Build.Helpers;

// A disposable NuGet v3 feed, bound exclusively to loopback. It saves the
// actual multipart package payload and returns 409 on duplicates, like a remote feed.
internal sealed class ScratchNugetFeed : IAsyncDisposable
{
    private readonly WebApplication _app;
    private readonly string _directory;

    private ScratchNugetFeed(WebApplication app, string directory)
    {
        _app = app;
        _directory = directory;
        app.MapGet("/v3/index.json", () => Results.Json(new
        {
            version = "3.0.0",
            resources = new[]
            {
                new Dictionary<string, string> { ["@id"] = app.Urls.Single() + "/packages", ["@type"] = "PackagePublish/2.0.0" },
                new Dictionary<string, string> { ["@id"] = app.Urls.Single() + "/symbols", ["@type"] = "SymbolPackagePublish/4.9.0" },
            },
        }));
        app.MapPut("/packages", context => ReceiveAsync(context, ".nupkg"));
        app.MapPut("/symbols", context => ReceiveAsync(context, ".snupkg"));
    }

    public string PackageSource => _app.Urls.Single() + "/v3/index.json";

    public static async Task<ScratchNugetFeed> StartAsync(string directory, CancellationToken cancellationToken)
    {
        var builder = WebApplication.CreateSlimBuilder(new WebApplicationOptions { Args = [] });
        builder.Logging.ClearProviders();
        builder.Services.Configure<FormOptions>(options => options.MultipartBodyLengthLimit = long.MaxValue);
        builder.WebHost.ConfigureKestrel(server =>
        {
            server.Listen(IPAddress.Loopback, 0);
            server.Limits.MaxRequestBodySize = null;
        });
        var app = builder.Build();
        var feed = new ScratchNugetFeed(app, directory);
        try
        {
            await app.StartAsync(cancellationToken).ConfigureAwait(false);
            return feed;
        }
        catch
        {
            await app.DisposeAsync().ConfigureAwait(false);
            throw;
        }
    }

    public ValueTask DisposeAsync() => _app.DisposeAsync();

    private async Task ReceiveAsync(HttpContext context, string extension)
    {
        var token = context.RequestAborted;
        var form = await context.Request.ReadFormAsync(token).ConfigureAwait(false);
        if (form.Files.Count != 1)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        var temporaryPath = Path.Combine(_directory, $".upload-{Guid.NewGuid():N}.tmp");
        try
        {
            string id;
            string version;
            await using (var payload = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None))
            {
                await form.Files[0].CopyToAsync(payload, token).ConfigureAwait(false);
                payload.Position = 0;
                (id, version) = await ReadPackageIdentityAsync(payload, token).ConfigureAwait(false);
            }

            // Treat request metadata as untrusted even though this endpoint is local.
            if (HasInvalidPackageIdentity(id, version))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            var path = Path.Combine(_directory, $"{id}.{version}{extension}");
            context.Response.StatusCode = CommitPackage(temporaryPath, path);
        }
        finally
        {
            File.Delete(temporaryPath);
        }
    }

    private static async Task<(string Id, string Version)> ReadPackageIdentityAsync(Stream payload, CancellationToken token)
    {
        using var archive = new ZipArchive(payload, ZipArchiveMode.Read, leaveOpen: true);
        var nuspec = archive.Entries.Single(entry => entry.FullName.EndsWith(".nuspec", StringComparison.OrdinalIgnoreCase));
        await using var stream = nuspec.Open();
        var document = await XDocument.LoadAsync(stream, LoadOptions.None, token).ConfigureAwait(false);
        var metadata = document.Root!.Elements().Single(element => element.Name.LocalName == "metadata");
        var id = metadata.Elements().Single(element => element.Name.LocalName == "id").Value;
        // NuGet omits SemVer build metadata from archive filenames.
        var version = metadata.Elements().Single(element => element.Name.LocalName == "version").Value.Split('+')[0];
        return (id, version);
    }

    private static int CommitPackage(string temporaryPath, string path)
    {
        try
        {
            // A same-directory move publishes the complete archive without overwriting a duplicate.
            File.Move(temporaryPath, path);
            return StatusCodes.Status201Created;
        }
        catch (IOException) when (File.Exists(path))
        {
            return StatusCodes.Status409Conflict;
        }
    }

    private static bool HasInvalidPackageIdentity(string id, string version) =>
        string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(version)
        || id.Any(character => !char.IsAsciiLetterOrDigit(character) && character is not '.' and not '-' and not '_')
        || version.Any(character => !char.IsAsciiLetterOrDigit(character) && character is not '.' and not '-' and not '+');
}
