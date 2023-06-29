using CliWrap.Buffered;
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
    
    public async Task<List<BufferedCommandResult>> UploadPackages(NuGetUploadOptions options)
    {
        var results = new List<BufferedCommandResult>();
        foreach (var packagePath in options.PackagePaths)
        {
            var arguments = new List<string>
            { 
                "nuget", "push", packagePath, "-n"
            };

            arguments.AddNonNullOrEmptyArgumentWithSwitch("-s", options.FeedUri.AbsoluteUri);
            arguments.AddNonNullOrEmptyArgumentWithSwitch("-k", options.ApiKey);
            
            var commandResult = await _context.Command
                .UsingCommandLineTool(new CommandLineToolOptions("dotnet")
                {
                    Arguments = arguments,
                    InputManipulator = string.IsNullOrWhiteSpace(options.ApiKey) ? s => s : s => s.Replace(options.ApiKey, "**********"),
                    OutputManipulator = string.IsNullOrWhiteSpace(options.ApiKey) ? s => s : s => s.Replace(options.ApiKey, "**********")
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

        arguments.AddNonNullOrEmptyArgumentWithSwitch("--username", options.Username);
        arguments.AddNonNullOrEmptyArgumentWithSwitch("--password", options.Password);

        return _context.DotNet().CustomCommand(new DotNetCommandOptions
        {
            Command = arguments,
            EnvironmentVariables = options.EnvironmentVariables,
            WorkingDirectory = options.WorkingDirectory,
            Credentials = options.Credentials,
            LogInput = options.LogInput,
            LogOutput = options.LogOutput
        });
    }
}