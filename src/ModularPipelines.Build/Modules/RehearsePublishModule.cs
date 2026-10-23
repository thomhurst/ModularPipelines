using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Helpers;
using ModularPipelines.Build.Settings;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[RequiresCapability("ci-master")]
[DependsOn<PackagePathsParserModule>]
[RunIf<ModularPipelines.OnLinux>]
public class RehearsePublishModule(IOptions<PublishSettings> settings) : Module<CommandResult[]>
{
    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithSkipWhen(_ => string.IsNullOrWhiteSpace(settings.Value.RehearsalFeedPath), "No scratch feed was configured");

    protected override async Task<CommandResult[]> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var feed = settings.Value.RehearsalFeedPath!;
        // This path never uses publishing credentials or accepts a remote feed.
        if (!Path.IsPathFullyQualified(feed) || (Uri.TryCreate(feed, UriKind.Absolute, out var uri) && !uri.IsFile))
        {
            throw new InvalidOperationException("The publishing rehearsal requires an absolute local feed path.");
        }

        Directory.CreateDirectory(feed);
        var manifestPath = Path.Combine(feed, "manifest.json");
        File.Delete(manifestPath);
        var packages = (await context.GetModule<PackagePathsParserModule>()).Value;
        if (packages.Count == 0)
        {
            throw new InvalidOperationException("The publishing rehearsal received no packages.");
        }

        await using var server = await ScratchNugetFeed.StartAsync(feed, cancellationToken).ConfigureAwait(false);
        void ConfigurePush(ModularPipelines.DotNet.Options.DotNetNuGetPushOptions options)
        {
            options.AllowInsecureConnections = true; // Only the loopback endpoint created above.
            options.NoServiceEndpoint = true;
        }

        var first = await NugetUploadHelper.UploadPackagesAsync(context, packages, server.PackageSource, null, cancellationToken, ConfigurePush).ConfigureAwait(false);
        // Re-run the same push to exercise the release workflow's retry behavior.
        var retry = await NugetUploadHelper.UploadPackagesAsync(context, packages, server.PackageSource, null, cancellationToken, ConfigurePush).ConfigureAwait(false);

        var verified = new List<object>();
        foreach (var package in packages)
        {
            var expected = new[] { package.Path, Path.ChangeExtension(package.Path, ".snupkg") }.Where(File.Exists);
            foreach (var path in expected)
            {
                var uploaded = Path.Combine(feed, Path.GetFileName(path));
                if (!File.Exists(uploaded))
                {
                    throw new InvalidOperationException($"The scratch feed is missing '{Path.GetFileName(path)}'.");
                }

                await using var originalStream = File.OpenRead(path);
                await using var uploadedStream = File.OpenRead(uploaded);
                var originalHash = await SHA256.HashDataAsync(originalStream, cancellationToken).ConfigureAwait(false);
                var uploadedHash = await SHA256.HashDataAsync(uploadedStream, cancellationToken).ConfigureAwait(false);
                if (!originalHash.SequenceEqual(uploadedHash))
                {
                    throw new InvalidOperationException($"The scratch feed changed '{Path.GetFileName(path)}'.");
                }

                verified.Add(new { File = Path.GetFileName(path), Sha256 = Convert.ToHexString(uploadedHash) });
            }
        }

        await File.WriteAllTextAsync(manifestPath, JsonSerializer.Serialize(new
        {
            PackageCount = packages.Count,
            RetrySucceeded = true,
            Files = verified,
        }, new JsonSerializerOptions { WriteIndented = true }), cancellationToken).ConfigureAwait(false);

        return [.. first, .. retry];
    }
}
