﻿using ModularPipelines.Context;
using ModularPipelines.DotNet;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

public class DotnetTestModule : Module<DotNetTestResult>
{
    protected override async Task<ModuleResult<DotNetTestResult>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        return await context.DotNet().Test(new DotNetTestOptions
        {
            WorkingDirectory = context.Environment.GitRootDirectory!.GetFolder("ModularPipelines.UnitTests").Path
        }, cancellationToken);
    }
}
