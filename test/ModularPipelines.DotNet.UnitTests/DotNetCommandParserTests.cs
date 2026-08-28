using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.DotNet.UnitTests;

public class DotNetCommandParserTests : TestBase
{
    [Test]
    public async Task NuGet_Delete_With_Two_Positional_Arguments()
    {
        var result = await GetResult(new DotNetNuGetDeleteOptions
        {
            PackageName = "MyPackageName",
            Version = "1.0.0"
        });
        await Assert.That(result.CommandInput).IsEqualTo("dotnet nuget delete MyPackageName 1.0.0");
    }

    [Test]
    public async Task NuGet_Delete_With_Source_Option()
    {
        var result = await GetResult(new DotNetNuGetDeleteOptions
        {
            PackageName = "MyPackageName",
            Version = "1.0.0",
            Source = "https://api.nuget.org/v3/index.json"
        });
        await Assert.That(result.CommandInput).IsEqualTo(
            "dotnet nuget delete MyPackageName 1.0.0 --source https://api.nuget.org/v3/index.json");
    }

    [Test]
    public async Task NuGet_Delete_With_ApiKey_Option()
    {
        var result = await GetResult(new DotNetNuGetDeleteOptions
        {
            PackageName = "MyPackageName",
            Version = "1.0.0",
            ApiKey = "my-secret-key"
        });
        await Assert.That(result.CommandInput).IsEqualTo(
            "dotnet nuget delete MyPackageName 1.0.0 --api-key **********");
    }

    [Test]
    public async Task Tool_Run_Prepends_Option_Terminator()
    {
        var result = await GetResult(new DotNetToolRunOptions("csharpier")
        {
            AllowRollForward = true,
            ToolArguments = ["check", "--help"]
        });

        await Assert.That(result.CommandInput).IsEqualTo(
            "dotnet tool run csharpier --allow-roll-forward -- check --help");
    }

    private async Task<CommandResult> GetResult(CommandLineToolOptions options)
    {
        var command = await GetService<ICommandContext>();
        return await command.ExecuteCommandLineToolAsync(
            options,
            new CommandExecutionOptions { InternalDryRun = true });
    }
}
