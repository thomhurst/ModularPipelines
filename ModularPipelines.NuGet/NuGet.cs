using ModularPipelines.Models;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Extensions;
using ModularPipelines.NuGet.Options;
using ModularPipelines.Options;

namespace ModularPipelines.NuGet;

public class NuGet : INuGet
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
                "nuget", "push", packagePath, "-n"
            };

            var commandResult = await _context.Command
                .ExecuteCommandLineTool(options.ToCommandLineToolOptions("dotnet", arguments) with
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
        return await _context.Command.ExecuteCommandLineTool(options.ToCommandLineToolOptions("dotnet", 
            "nuget", "add", "source", options.FeedUri.AbsoluteUri));
    }
}