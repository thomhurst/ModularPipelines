using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class DotNetTests : TestBase
{
    private class DotNetVersionModule : Module<CommandResult>
    {
        protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await context.DotNet().List.Package(new DotNetListPackageOptions
            {
                ProjectSolution = context.Git().RootDirectory.FindFile(x => x.Extension == ".sln").AssertExists(),
            }, token: cancellationToken);
        }
    }
    
    private class DotNetFormatModule : Module<CommandResult>
    {
        protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await context.DotNet().Format(new DotNetFormatOptions
            {
                ProjectSolution = context.Git().RootDirectory.FindFile(x => x.Name.Contains("TestsForTests")).AssertExists(),
            }, token: cancellationToken);
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var module = await RunModule<DotNetVersionModule>();

        var moduleResult = await module;
        
        await using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).Is.EqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).Is.Null();
            await Assert.That(moduleResult.Value).Is.Not.Null();
        }
    }
    
    [Test]
    public async Task Format_Has_Not_Errored()
    {
        var module = await RunModule<DotNetFormatModule>();

        var moduleResult = await module;
        
        await using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).Is.EqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).Is.Null();
            await Assert.That(moduleResult.Value).Is.Not.Null();
        }
    }

    [Test]
    public async Task Standard_Output_Starts_With_Git_Version()
    {
        var module = await RunModule<DotNetVersionModule>();

        var moduleResult = await module;

        await using (Assert.Multiple())
        {
            await Assert.That(moduleResult.Value!.StandardError).Is.Null().Or.Is.Empty();
            await Assert.That(moduleResult.Value.StandardOutput).Does.Match("\\d+");
        }
    }
}