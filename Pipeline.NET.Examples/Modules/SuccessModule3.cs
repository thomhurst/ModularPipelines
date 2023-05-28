using Pipeline.NET.Context;
using Pipeline.NET.Models;
using Pipeline.NET.Modules;

namespace Pipeline.NET.Examples.Modules;

public class SuccessModule3 : Module
{
    public SuccessModule3(IModuleContext context) : base(context)
    {
    }

    protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(12), cancellationToken);

        var gitModule = await GetModule<GitVersionModule>();

        return null;
    }
}