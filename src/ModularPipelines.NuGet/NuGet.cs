using System.Diagnostics.CodeAnalysis;
using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.NuGet.Options;

namespace ModularPipelines.NuGet;

[ExcludeFromCodeCoverage]
internal class NuGet : INuGet
{
    private readonly IPipelineContext _context;

    public NuGet(IPipelineContext context)
    {
        _context = context;
    }

    public async Task<CommandResult[]> UploadPackages(NuGetUploadOptions options)
    {
        return await options.PackagePaths.ToAsyncProcessorBuilder()
            .SelectAsync(async packagePath =>
                await _context.Command.ExecuteCommandLineTool(options.WithArguments(packagePath)))
            .ProcessOneAtATime();
    }

    public async Task<CommandResult> AddSource(NuGetSourceOptions options)
    {
        return await _context.Command.ExecuteCommandLineTool(options);
    }
}