
using CliWrap.Buffered;
using Pipeline.NET.Context;
using Pipeline.NET.DotNet.Modules;
using Pipeline.NET.DotNet.Options;
using Pipeline.NET.Models;
using Pipeline.NET.Modules;

namespace Pipeline.NET.NuGet;

public abstract class NuGetUploadModule : Module<List<BufferedCommandResult?>>
{
    protected NuGetUploadModule(IModuleContext context) : base(context)
    {
    }
    
    protected abstract IEnumerable<string> PackagePaths { get; set; }
    protected abstract string ApiKey { get; set; }
    protected abstract Uri FeedUri { get; set; }

    protected override async Task<ModuleResult<List<BufferedCommandResult?>>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var results = new List<ModuleResult<BufferedCommandResult>>();
        foreach (var packagePath in PackagePaths)
        {
            var module = new ExternalRunnableDotNetCommandModule(Context, new DotNetCommandModuleOptions
            {
                Command = new[] {"nuget", "push"},
                TargetPath = packagePath,
                ExtraArguments = new List<string>
                {
                    "-k", ApiKey,
                    "-n",
                    "-s", FeedUri.AbsoluteUri
                }
            });

            await module.StartProcessingModule();
            
            results.Add(await module);
        }

        return results.Select(x => x.Value).ToList();
    }
}