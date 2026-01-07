using ModularPipelines.Context;
using ModularPipelines.DotNet.Builders;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Builders;

public class DotNetBuildBuilderTests : TestBase
{
    private static Mock<ICommand> CreateMockCommand()
    {
        var mock = new Mock<ICommand>();
        mock
            .Setup(c => c.ExecuteCommandLineTool(
                It.IsAny<CommandLineToolOptions>(),
                It.IsAny<CommandExecutionOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CommandResult(
                "dotnet build",
                "/working/dir",
                "",
                "",
                new Dictionary<string, string?>(),
                DateTimeOffset.Now,
                DateTimeOffset.Now,
                TimeSpan.Zero,
                0));
        return mock;
    }

    #region Tool-Specific Options Tests

    [Test]
    public async Task ForProject_SetsProjectPath()
    {
        var mockCommand = CreateMockCommand();
        var builder = new DotNetBuildBuilder(mockCommand.Object);

        builder.ForProject("MyProject.csproj");

        var (toolOptions, _) = builder.ToOptions();
        await Assert.That(toolOptions.ProjectSolution).IsEqualTo("MyProject.csproj");
    }

    [Test]
    public async Task WithConfiguration_SetsConfiguration()
    {
        var mockCommand = CreateMockCommand();
        var builder = new DotNetBuildBuilder(mockCommand.Object);

        builder.WithConfiguration("Release");

        var (toolOptions, _) = builder.ToOptions();
        await Assert.That(toolOptions.Configuration).IsEqualTo("Release");
    }

    [Test]
    public async Task WithFramework_SetsFramework()
    {
        var mockCommand = CreateMockCommand();
        var builder = new DotNetBuildBuilder(mockCommand.Object);

        builder.WithFramework("net8.0");

        var (toolOptions, _) = builder.ToOptions();
        await Assert.That(toolOptions.Framework).IsEqualTo("net8.0");
    }

    [Test]
    public async Task WithRuntime_SetsRuntime()
    {
        var mockCommand = CreateMockCommand();
        var builder = new DotNetBuildBuilder(mockCommand.Object);

        builder.WithRuntime("win-x64");

        var (toolOptions, _) = builder.ToOptions();
        await Assert.That(toolOptions.Runtime).IsEqualTo("win-x64");
    }

    [Test]
    public async Task WithOutput_SetsOutput()
    {
        var mockCommand = CreateMockCommand();
        var builder = new DotNetBuildBuilder(mockCommand.Object);

        builder.WithOutput("/output/path");

        var (toolOptions, _) = builder.ToOptions();
        await Assert.That(toolOptions.Output).IsEqualTo("/output/path");
    }

    [Test]
    public async Task WithNoRestore_EnablesNoRestore()
    {
        var mockCommand = CreateMockCommand();
        var builder = new DotNetBuildBuilder(mockCommand.Object);

        builder.WithNoRestore();

        var (toolOptions, _) = builder.ToOptions();
        await Assert.That(toolOptions.NoRestore).IsTrue();
    }

    [Test]
    public async Task WithNoIncremental_EnablesNoIncremental()
    {
        var mockCommand = CreateMockCommand();
        var builder = new DotNetBuildBuilder(mockCommand.Object);

        builder.WithNoIncremental();

        var (toolOptions, _) = builder.ToOptions();
        await Assert.That(toolOptions.NoIncremental).IsTrue();
    }

    [Test]
    public async Task WithNoLogo_EnablesNoLogo()
    {
        var mockCommand = CreateMockCommand();
        var builder = new DotNetBuildBuilder(mockCommand.Object);

        builder.WithNoLogo();

        var (toolOptions, _) = builder.ToOptions();
        await Assert.That(toolOptions.Nologo).IsTrue();
    }

    [Test]
    public async Task WithProperty_AddsProperty()
    {
        var mockCommand = CreateMockCommand();
        var builder = new DotNetBuildBuilder(mockCommand.Object);

        builder.WithProperty("Version", "1.0.0");

        var (toolOptions, _) = builder.ToOptions();
        await Assert.That(toolOptions.Properties).IsNotNull();
        await Assert.That(toolOptions.Properties!.Count()).IsEqualTo(1);
        await Assert.That(toolOptions.Properties!.First().Key).IsEqualTo("Version");
        await Assert.That(toolOptions.Properties!.First().Value).IsEqualTo("1.0.0");
    }

    [Test]
    public async Task WithProperty_AddsMultipleProperties()
    {
        var mockCommand = CreateMockCommand();
        var builder = new DotNetBuildBuilder(mockCommand.Object);

        builder
            .WithProperty("Version", "1.0.0")
            .WithProperty("AssemblyVersion", "1.0.0.0");

        var (toolOptions, _) = builder.ToOptions();
        await Assert.That(toolOptions.Properties).IsNotNull();
        await Assert.That(toolOptions.Properties!.Count()).IsEqualTo(2);
    }

    #endregion

    #region Execution Options Tests

    [Test]
    public async Task WithWorkingDirectory_SetsWorkingDirectory()
    {
        var mockCommand = CreateMockCommand();
        var builder = new DotNetBuildBuilder(mockCommand.Object);

        builder.WithWorkingDirectory("/project/dir");

        var (_, execOptions) = builder.ToOptions();
        await Assert.That(execOptions.WorkingDirectory).IsEqualTo("/project/dir");
    }

    [Test]
    public async Task WithTimeout_SetsTimeout()
    {
        var mockCommand = CreateMockCommand();
        var builder = new DotNetBuildBuilder(mockCommand.Object);
        var timeout = TimeSpan.FromMinutes(30);

        builder.WithTimeout(timeout);

        var (_, execOptions) = builder.ToOptions();
        await Assert.That(execOptions.ExecutionTimeout).IsEqualTo(timeout);
    }

    [Test]
    public async Task WithEnvironmentVariable_AddsVariable()
    {
        var mockCommand = CreateMockCommand();
        var builder = new DotNetBuildBuilder(mockCommand.Object);

        builder.WithEnvironmentVariable("DOTNET_CLI_TELEMETRY_OPTOUT", "1");

        var (_, execOptions) = builder.ToOptions();
        await Assert.That(execOptions.EnvironmentVariables).IsNotNull();
        await Assert.That(execOptions.EnvironmentVariables!["DOTNET_CLI_TELEMETRY_OPTOUT"]).IsEqualTo("1");
    }

    [Test]
    public async Task WithThrowOnError_SetsThrowOnError()
    {
        var mockCommand = CreateMockCommand();
        var builder = new DotNetBuildBuilder(mockCommand.Object);

        builder.WithThrowOnError(false);

        var (_, execOptions) = builder.ToOptions();
        await Assert.That(execOptions.ThrowOnNonZeroExitCode).IsFalse();
    }

    #endregion

    #region Chaining Tests

    [Test]
    public async Task FluentChaining_SetsAllOptions()
    {
        var mockCommand = CreateMockCommand();
        var builder = new DotNetBuildBuilder(mockCommand.Object);

        builder
            .ForProject("MyProject.sln")
            .WithConfiguration("Release")
            .WithFramework("net8.0")
            .WithNoRestore()
            .WithNoLogo()
            .WithProperty("Version", "2.0.0")
            .WithWorkingDirectory("/project")
            .WithTimeout(TimeSpan.FromMinutes(15));

        var (toolOptions, execOptions) = builder.ToOptions();

        await Assert.That(toolOptions.ProjectSolution).IsEqualTo("MyProject.sln");
        await Assert.That(toolOptions.Configuration).IsEqualTo("Release");
        await Assert.That(toolOptions.Framework).IsEqualTo("net8.0");
        await Assert.That(toolOptions.NoRestore).IsTrue();
        await Assert.That(toolOptions.Nologo).IsTrue();
        await Assert.That(toolOptions.Properties!.First().Key).IsEqualTo("Version");
        await Assert.That(execOptions.WorkingDirectory).IsEqualTo("/project");
        await Assert.That(execOptions.ExecutionTimeout).IsEqualTo(TimeSpan.FromMinutes(15));
    }

    [Test]
    public async Task FluentChaining_ReturnsSameBuilderInstance()
    {
        var mockCommand = CreateMockCommand();
        var builder = new DotNetBuildBuilder(mockCommand.Object);

        var result1 = builder.ForProject("test.csproj");
        var result2 = result1.WithConfiguration("Release");
        var result3 = result2.WithFramework("net8.0");

        await Assert.That(ReferenceEquals(builder, result1)).IsTrue();
        await Assert.That(ReferenceEquals(result1, result2)).IsTrue();
        await Assert.That(ReferenceEquals(result2, result3)).IsTrue();
    }

    #endregion

    #region ExecuteAsync Tests

    [Test]
    public async Task ExecuteAsync_CallsCommandExecuteWithOptions()
    {
        var mockCommand = CreateMockCommand();
        var builder = new DotNetBuildBuilder(mockCommand.Object);
        builder
            .ForProject("MyProject.csproj")
            .WithConfiguration("Release");

        var result = await builder.ExecuteAsync();

        await Assert.That(result).IsNotNull();
        mockCommand.Verify(c => c.ExecuteCommandLineTool(
            It.Is<DotNetBuildOptions>(o =>
                o.ProjectSolution == "MyProject.csproj" &&
                o.Configuration == "Release"),
            It.IsAny<CommandExecutionOptions>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task ExecuteAsync_PassesExecutionOptions()
    {
        var mockCommand = CreateMockCommand();
        CommandExecutionOptions? capturedExecOptions = null;
        mockCommand
            .Setup(c => c.ExecuteCommandLineTool(
                It.IsAny<CommandLineToolOptions>(),
                It.IsAny<CommandExecutionOptions>(),
                It.IsAny<CancellationToken>()))
            .Callback<CommandLineToolOptions, CommandExecutionOptions?, CancellationToken>((_, exec, _) => capturedExecOptions = exec)
            .ReturnsAsync(new CommandResult(
                "dotnet build",
                "/test/dir",
                "",
                "",
                new Dictionary<string, string?>(),
                DateTimeOffset.Now,
                DateTimeOffset.Now,
                TimeSpan.Zero,
                0));

        var builder = new DotNetBuildBuilder(mockCommand.Object);
        builder
            .WithWorkingDirectory("/test/dir")
            .WithTimeout(TimeSpan.FromMinutes(10));

        await builder.ExecuteAsync();

        await Assert.That(capturedExecOptions).IsNotNull();
        await Assert.That(capturedExecOptions!.WorkingDirectory).IsEqualTo("/test/dir");
        await Assert.That(capturedExecOptions.ExecutionTimeout).IsEqualTo(TimeSpan.FromMinutes(10));
    }

    #endregion

    #region Initial Options Tests

    [Test]
    public async Task InitialOptions_UsesProvidedOptions()
    {
        var mockCommand = CreateMockCommand();
        var initialOptions = new DotNetBuildOptions { Configuration = "Debug", Framework = "net7.0" };
        var builder = new DotNetBuildBuilder(mockCommand.Object, initialOptions);

        var (toolOptions, _) = builder.ToOptions();

        await Assert.That(toolOptions.Configuration).IsEqualTo("Debug");
        await Assert.That(toolOptions.Framework).IsEqualTo("net7.0");
    }

    [Test]
    public async Task InitialOptions_CanBeOverridden()
    {
        var mockCommand = CreateMockCommand();
        var initialOptions = new DotNetBuildOptions { Configuration = "Debug" };
        var builder = new DotNetBuildBuilder(mockCommand.Object, initialOptions);

        builder.WithConfiguration("Release");

        var (toolOptions, _) = builder.ToOptions();
        await Assert.That(toolOptions.Configuration).IsEqualTo("Release");
    }

    #endregion
}
