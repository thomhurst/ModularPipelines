using ModularPipelines.Context;
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

    [Test]
    public async Task Test_Renders_VSTest_Options()
    {
        var result = await GetResult(new DotNetTestOptions
        {
            Settings = "test.runsettings",
            Filter = "Category=Unit",
            Logger = "trx",
            Collect = "Coverage",
            Blame = true,
        });

        await Assert.That(result.CommandInput).IsEqualTo(
            "dotnet test --settings test.runsettings --filter Category=Unit --logger trx --collect Coverage --blame");
    }

    [Test]
    public async Task Build_Renders_Options_With_Multiple_Aliases()
    {
        var result = await GetResult(new DotNetBuildOptions
        {
            UseCurrentRuntime = true,
            Verbosity = "normal",
            SelfContained = true,
        });

        await Assert.That(result.CommandInput).IsEqualTo(
            "dotnet build --use-current-runtime -verbosity normal --self-contained");
    }

    [Test]
    public async Task Test_Preserves_Explicit_MTP_Arguments()
    {
        var result = await GetResult(new DotNetTestOptions
        {
            Arguments = ["--project", "Tests.csproj", "--", "--filter", "Category=Unit", "--", "--report-trx"],
            ArgumentsContainOptionTerminator = true,
        });

        await Assert.That(result.CommandInput).IsEqualTo(
            "dotnet test --project Tests.csproj -- --filter Category=Unit -- --report-trx");
    }

    [Test]
    public async Task Pack_Renders_Version_Value()
    {
        var result = await GetResult(new DotNetPackOptions { Version = "1.2.3" });
        await Assert.That(result.CommandInput).IsEqualTo("dotnet pack --version 1.2.3");
    }

    private async Task<CommandResult> GetResult(CommandLineToolOptions options)
    {
        var command = await GetService<ICommandContext>();
        return await command.ExecuteCommandLineToolAsync(
            options,
            new CommandExecutionOptions { InternalDryRun = true });
    }
}
