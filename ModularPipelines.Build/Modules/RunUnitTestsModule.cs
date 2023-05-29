using CliWrap.Buffered;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

public class RunUnitTestsModule : Module<List<BufferedCommandResult>>
{
    public RunUnitTestsModule(IModuleContext context) : base(context)
    {
    }

    protected override async Task<ModuleResult<List<BufferedCommandResult>>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var results = new List<BufferedCommandResult>();

        foreach (var unitTestProjectFile in Context.Environment
                     .GitRootDirectory!
                     .GetFiles(file => file.Path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)
                                       && file.Path.Contains("UnitTests", StringComparison.OrdinalIgnoreCase)))
        {
            results.Add(await Context.DotNet().Test(new DotNetOptions
            {
                TargetPath = unitTestProjectFile.Path
            }, cancellationToken));
        }

        return results;
    }
}