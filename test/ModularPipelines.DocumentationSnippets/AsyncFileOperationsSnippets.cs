using System.IO.Compression;
using ModularPipelines.Context;

namespace ModularPipelines.DocumentationSnippets;

public static class AsyncFileOperationsSnippets
{
    public static async Task CreateHashAndExtract(IModuleContext context, CancellationToken cancellationToken)
    {
        var source = context.Files.GetFolder("publish");
        var archive = await context.Files.Zip.CreateFromDirectoryAsync(
            source,
            "artifacts/package.zip",
            CompressionLevel.Optimal,
            cancellationToken);

        var hash = await context.Security.Hash.Sha256FileAsync(
            archive.Path,
            HashEncoding.Hex,
            cancellationToken);

        var extracted = await context.Files.Zip.ExtractToDirectoryAsync(
            archive.Path,
            "extracted",
            overwriteFiles: false,
            cancellationToken: cancellationToken);
    }
}
