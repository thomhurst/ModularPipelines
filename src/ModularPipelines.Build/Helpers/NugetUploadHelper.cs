using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Helpers;

public static class NugetUploadHelper
{
    public static async Task<CommandResult[]> UploadPackagesAsync(
        IModuleContext context,
        IEnumerable<File> packagePaths,
        string source,
        string? apiKey,
        CancellationToken cancellationToken)
    {
        return await packagePaths
            .SelectAsync(async nugetFile => await context.DotNet().Nuget.Push(new DotNetNugetPushOptions
            {
                Path = nugetFile,
                Source = source,
                ApiKey = apiKey,
            }, cancellationToken), cancellationToken: cancellationToken)
            .ProcessOneAtATime();
    }
}
