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
        public override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.DotNet().List.Package(new DotNetListPackageOptions
            {
                ProjectSolution = context.Git().RootDirectory.FindFile(x => x.Extension == ".sln").AssertExists(),
            }, token: cancellationToken);
        }
    }

    private class DotNetFormatModule : Module<CommandResult>
    {
        public override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
        var moduleResult = await await RunModule<DotNetVersionModule>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    [Skip("Temporarily disabled")]
    public async Task Format_Has_Not_Errored()
    {
        var moduleResult = await await RunModule<DotNetFormatModule>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }
}