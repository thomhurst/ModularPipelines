using ModularPipelines.Models;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.NuGet.Options;

namespace ModularPipelines.NuGet;

internal class NuGet : INuGet
{
    private readonly IModuleContext _context;

    public NuGet(IModuleContext context)
    {
        _context = context;
    }

    public async Task<List<CommandResult>> UploadPackages(NuGetUploadOptions options)
    {
        var results = new List<CommandResult>();
        foreach (var packagePath in options.PackagePaths)
        {
            var commandResult = await _context.Command
                .ExecuteCommandLineTool(options.WithArguments(packagePath));

            results.Add(commandResult);
        }

        return results;
    }

    public async Task<CommandResult> AddSource(NuGetSourceOptions options)
    {
        return await _context.Command.ExecuteCommandLineTool(options);
    }
}
