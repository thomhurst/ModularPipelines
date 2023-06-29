using ModularPipelines.Context;
using ModularPipelines.DotNet;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

public class RunUnitTestsModule : Module<List<DotNetTestResult>>
{
    protected override async Task<ModuleResult<List<DotNetTestResult>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var results = new List<DotNetTestResult>();

        foreach (var unitTestProjectFile in context.Environment
                     .GitRootDirectory!
                     .GetFiles(file => file.Path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)
                                       && file.Path.Contains("UnitTests", StringComparison.OrdinalIgnoreCase)))
        {
            results.Add(await context.DotNet().Test(new DotNetOptions
            {
                TargetPath = unitTestProjectFile.Path,
                LogOutput = false
            }, cancellationToken));
        }

        if (!results.Any() || !results.All(x => x.Successful))
        {
            var tests = results
                .SelectMany(x => x.UnitTestResults)
                .Where(x => x.Outcome != "Passed")
                .Select(x => $"{x.TestName} - {x.Output} - {x.Output}")
                .ToList();
            
            throw new Exception($"Test failures: {string.Join(Environment.NewLine, tests)}");
        }

        return results;
    }
}