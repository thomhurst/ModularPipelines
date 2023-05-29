using CliWrap.Buffered;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Command.Extensions;
using ModularPipelines.Command.Options;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.NuGet.Options;

namespace ModularPipelines.NuGet;

public class NuGet : INuGet
{
    public IModuleContext Context { get; }

    [ActivatorUtilitiesConstructor]
    internal NuGet(IModuleContext context)
    {
        Context = context;
    }
    
    public async Task<List<BufferedCommandResult>> UploadPackage(NuGetUploadOptions options)
    {
        var results = new List<BufferedCommandResult>();
        foreach (var packagePath in options.PackagePaths)
        {
            var commandResult = await Context.Command()
                .UsingCommandLineTool(new CommandLineToolOptions("dotnet")
                {
                    Arguments = new []
                    { 
                        "nuget", "push", 
                        packagePath, 
                        "-s", options.FeedUri.AbsoluteUri,
                        "-k", options.ApiKey ?? string.Empty,
                        "-n"
                    }
                });
            
            results.Add(commandResult);
        }

        return results;
    }

    public Task<BufferedCommandResult> AddSource(NuGetSourceOptions options)
    {
        var arguments = new List<string>
        {
            "nuget", "add", "source", options.FeedUri.AbsoluteUri, 
            "-n", options.Name
        };

        if (options.Username != null)
        {
            arguments.Add("--username");
            arguments.Add(options.Username);
        }
        
        if (options.Password != null)
        {
            arguments.Add("--password");
            arguments.Add(options.Password);
        }
        
        return Context.DotNet().CustomCommand(new DotNetCommandOptions
        {
            Command = arguments
        });
    }
}