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
            var arguments = new List<string>
            {
                packagePath
            };

            var commandResult = await _context.Command
                .ExecuteCommandLineTool(options.WithArguments(arguments) with
                {
                    Arguments = arguments,
                    InputLoggingManipulator = string.IsNullOrWhiteSpace(options.ApiKey) ? s => s : s => s.Replace(options.ApiKey, "**********"),
                    OutputLoggingManipulator = string.IsNullOrWhiteSpace(options.ApiKey) ? s => s : s => s.Replace(options.ApiKey, "**********")
                });

            results.Add(commandResult);
        }

        return results;
    }

    public async Task<CommandResult> AddSource(NuGetSourceOptions options)
    {
        return await _context.Command.ExecuteCommandLineTool(options);
    }
}
