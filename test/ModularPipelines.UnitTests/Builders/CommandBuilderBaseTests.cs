using ModularPipelines.Attributes;
using ModularPipelines.Builders;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Builders;

public class CommandBuilderBaseTests : TestBase
{
    #region Test Options and Builder

    [CliTool("testtool")]
    [CliCommand("testtool", "command")]
    private record TestToolOptions : CommandLineToolOptions
    {
        [CliOption("--config")]
        public string? Configuration { get; init; }

        [CliOption("--framework")]
        public string? Framework { get; init; }

        [CliFlag("--no-restore")]
        public bool? NoRestore { get; init; }
    }

    private class TestToolBuilder : CommandBuilderBase<TestToolBuilder, TestToolOptions>
    {
        public TestToolBuilder(ICommand command) : base(command) { }

        public TestToolBuilder(ICommand command, TestToolOptions initialOptions) : base(command, initialOptions) { }

        public TestToolBuilder WithConfiguration(string configuration)
        {
            ToolOptions = ToolOptions with { Configuration = configuration };
            return this;
        }

        public TestToolBuilder WithFramework(string framework)
        {
            ToolOptions = ToolOptions with { Framework = framework };
            return this;
        }

        public TestToolBuilder WithNoRestore(bool noRestore = true)
        {
            ToolOptions = ToolOptions with { NoRestore = noRestore };
            return this;
        }
    }

    #endregion

    #region Execution Options Tests

    [Test]
    public async Task WithWorkingDirectory_SetsWorkingDirectory()
    {
        var mockCommand = new Mock<ICommand>();
        var builder = new TestToolBuilder(mockCommand.Object);

        builder.WithWorkingDirectory("/test/directory");

        var (_, execOptions) = builder.ToOptions();
        await Assert.That(execOptions.WorkingDirectory).IsEqualTo("/test/directory");
    }

    [Test]
    public async Task WithTimeout_SetsTimeout()
    {
        var mockCommand = new Mock<ICommand>();
        var builder = new TestToolBuilder(mockCommand.Object);
        var timeout = TimeSpan.FromMinutes(5);

        builder.WithTimeout(timeout);

        var (_, execOptions) = builder.ToOptions();
        await Assert.That(execOptions.ExecutionTimeout).IsEqualTo(timeout);
    }

    [Test]
    public async Task WithEnvironmentVariable_AddsVariable()
    {
        var mockCommand = new Mock<ICommand>();
        var builder = new TestToolBuilder(mockCommand.Object);

        builder.WithEnvironmentVariable("MY_VAR", "my_value");

        var (_, execOptions) = builder.ToOptions();
        await Assert.That(execOptions.EnvironmentVariables).IsNotNull();
        await Assert.That(execOptions.EnvironmentVariables!["MY_VAR"]).IsEqualTo("my_value");
    }

    [Test]
    public async Task WithEnvironmentVariable_AddsMultipleVariables()
    {
        var mockCommand = new Mock<ICommand>();
        var builder = new TestToolBuilder(mockCommand.Object);

        builder
            .WithEnvironmentVariable("VAR1", "value1")
            .WithEnvironmentVariable("VAR2", "value2");

        var (_, execOptions) = builder.ToOptions();
        await Assert.That(execOptions.EnvironmentVariables).IsNotNull();
        await Assert.That(execOptions.EnvironmentVariables!["VAR1"]).IsEqualTo("value1");
        await Assert.That(execOptions.EnvironmentVariables!["VAR2"]).IsEqualTo("value2");
    }

    [Test]
    public async Task WithEnvironmentVariables_AddsDictionary()
    {
        var mockCommand = new Mock<ICommand>();
        var builder = new TestToolBuilder(mockCommand.Object);
        var variables = new Dictionary<string, string?>
        {
            ["VAR1"] = "value1",
            ["VAR2"] = "value2"
        };

        builder.WithEnvironmentVariables(variables);

        var (_, execOptions) = builder.ToOptions();
        await Assert.That(execOptions.EnvironmentVariables).IsNotNull();
        await Assert.That(execOptions.EnvironmentVariables!["VAR1"]).IsEqualTo("value1");
        await Assert.That(execOptions.EnvironmentVariables!["VAR2"]).IsEqualTo("value2");
    }

    [Test]
    public async Task WithSudo_EnablesSudo()
    {
        var mockCommand = new Mock<ICommand>();
        var builder = new TestToolBuilder(mockCommand.Object);

        builder.WithSudo();

        var (_, execOptions) = builder.ToOptions();
        await Assert.That(execOptions.Sudo).IsTrue();
    }

    [Test]
    public async Task WithSudo_DisablesSudo_WhenFalse()
    {
        var mockCommand = new Mock<ICommand>();
        var builder = new TestToolBuilder(mockCommand.Object);

        builder.WithSudo(false);

        var (_, execOptions) = builder.ToOptions();
        await Assert.That(execOptions.Sudo).IsFalse();
    }

    [Test]
    public async Task WithThrowOnError_EnablesThrowOnError()
    {
        var mockCommand = new Mock<ICommand>();
        var builder = new TestToolBuilder(mockCommand.Object);

        // Default is true, so we explicitly set it
        builder.WithThrowOnError();

        var (_, execOptions) = builder.ToOptions();
        await Assert.That(execOptions.ThrowOnNonZeroExitCode).IsTrue();
    }

    [Test]
    public async Task WithThrowOnError_DisablesThrowOnError_WhenFalse()
    {
        var mockCommand = new Mock<ICommand>();
        var builder = new TestToolBuilder(mockCommand.Object);

        builder.WithThrowOnError(false);

        var (_, execOptions) = builder.ToOptions();
        await Assert.That(execOptions.ThrowOnNonZeroExitCode).IsFalse();
    }

    [Test]
    public async Task WithGracefulShutdownTimeout_SetsTimeout()
    {
        var mockCommand = new Mock<ICommand>();
        var builder = new TestToolBuilder(mockCommand.Object);
        var timeout = TimeSpan.FromSeconds(60);

        builder.WithGracefulShutdownTimeout(timeout);

        var (_, execOptions) = builder.ToOptions();
        await Assert.That(execOptions.GracefulShutdownTimeout).IsEqualTo(timeout);
    }

    [Test]
    public async Task WithLogging_SetsLoggingOptions()
    {
        var mockCommand = new Mock<ICommand>();
        var builder = new TestToolBuilder(mockCommand.Object);
        var loggingOptions = new CommandLoggingOptions
        {
            Verbosity = CommandLogVerbosity.Diagnostic,
            ShowExitCode = true
        };

        builder.WithLogging(loggingOptions);

        var (_, execOptions) = builder.ToOptions();
        await Assert.That(execOptions.LogSettings).IsNotNull();
        await Assert.That(execOptions.LogSettings!.Verbosity).IsEqualTo(CommandLogVerbosity.Diagnostic);
        await Assert.That(execOptions.LogSettings.ShowExitCode).IsTrue();
    }

    [Test]
    public async Task WithLogging_ConfiguresUsingAction()
    {
        var mockCommand = new Mock<ICommand>();
        var builder = new TestToolBuilder(mockCommand.Object);

        builder.WithLogging(options =>
        {
            options = options with
            {
                Verbosity = CommandLogVerbosity.Silent,
                ShowWorkingDirectory = true
            };
        });

        var (_, execOptions) = builder.ToOptions();
        await Assert.That(execOptions.LogSettings).IsNotNull();
    }

    #endregion

    #region Tool Options Tests

    [Test]
    public async Task ToolSpecificOption_SetsToolOptions()
    {
        var mockCommand = new Mock<ICommand>();
        var builder = new TestToolBuilder(mockCommand.Object);

        builder.WithConfiguration("Release");

        var (toolOptions, _) = builder.ToOptions();
        await Assert.That(toolOptions.Configuration).IsEqualTo("Release");
    }

    [Test]
    public async Task InitialOptions_UsesProvidedOptions()
    {
        var mockCommand = new Mock<ICommand>();
        var initialOptions = new TestToolOptions { Configuration = "Debug" };
        var builder = new TestToolBuilder(mockCommand.Object, initialOptions);

        var (toolOptions, _) = builder.ToOptions();
        await Assert.That(toolOptions.Configuration).IsEqualTo("Debug");
    }

    [Test]
    public async Task InitialOptions_CanBeModified()
    {
        var mockCommand = new Mock<ICommand>();
        var initialOptions = new TestToolOptions { Configuration = "Debug" };
        var builder = new TestToolBuilder(mockCommand.Object, initialOptions);

        builder.WithFramework("net8.0");

        var (toolOptions, _) = builder.ToOptions();
        await Assert.That(toolOptions.Configuration).IsEqualTo("Debug");
        await Assert.That(toolOptions.Framework).IsEqualTo("net8.0");
    }

    #endregion

    #region Chaining Tests

    [Test]
    public async Task FluentChaining_SetsAllOptions()
    {
        var mockCommand = new Mock<ICommand>();
        var builder = new TestToolBuilder(mockCommand.Object);

        builder
            .WithConfiguration("Release")
            .WithFramework("net8.0")
            .WithNoRestore()
            .WithTimeout(TimeSpan.FromMinutes(10))
            .WithWorkingDirectory("/project")
            .WithEnvironmentVariable("CI", "true");

        var (toolOptions, execOptions) = builder.ToOptions();

        await Assert.That(toolOptions.Configuration).IsEqualTo("Release");
        await Assert.That(toolOptions.Framework).IsEqualTo("net8.0");
        await Assert.That(toolOptions.NoRestore).IsTrue();
        await Assert.That(execOptions.ExecutionTimeout).IsEqualTo(TimeSpan.FromMinutes(10));
        await Assert.That(execOptions.WorkingDirectory).IsEqualTo("/project");
        await Assert.That(execOptions.EnvironmentVariables!["CI"]).IsEqualTo("true");
    }

    [Test]
    public async Task FluentChaining_ReturnsSameBuilderInstance()
    {
        var mockCommand = new Mock<ICommand>();
        var builder = new TestToolBuilder(mockCommand.Object);

        var result1 = builder.WithConfiguration("Release");
        var result2 = result1.WithFramework("net8.0");
        var result3 = result2.WithTimeout(TimeSpan.FromMinutes(5));

        await Assert.That(ReferenceEquals(builder, result1)).IsTrue();
        await Assert.That(ReferenceEquals(result1, result2)).IsTrue();
        await Assert.That(ReferenceEquals(result2, result3)).IsTrue();
    }

    #endregion

    #region ExecuteAsync Tests

    [Test]
    public async Task ExecuteAsync_CallsCommandExecuteWithOptions()
    {
        var mockCommand = new Mock<ICommand>();
        var expectedResult = new CommandResult(
            "testtool command",
            "/working/dir",
            "output",
            "",
            new Dictionary<string, string?>(),
            DateTimeOffset.Now,
            DateTimeOffset.Now,
            TimeSpan.Zero,
            0);

        mockCommand
            .Setup(c => c.ExecuteCommandLineTool(
                It.IsAny<CommandLineToolOptions>(),
                It.IsAny<CommandExecutionOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var builder = new TestToolBuilder(mockCommand.Object);
        builder.WithConfiguration("Release");

        var result = await builder.ExecuteAsync();

        await Assert.That(result).IsNotNull();
        mockCommand.Verify(c => c.ExecuteCommandLineTool(
            It.Is<TestToolOptions>(o => o.Configuration == "Release"),
            It.IsAny<CommandExecutionOptions>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task ExecuteAsync_PassesCancellationToken()
    {
        var mockCommand = new Mock<ICommand>();
        var expectedResult = new CommandResult(
            "testtool command",
            "/working/dir",
            "output",
            "",
            new Dictionary<string, string?>(),
            DateTimeOffset.Now,
            DateTimeOffset.Now,
            TimeSpan.Zero,
            0);

        CancellationToken capturedToken = default;
        mockCommand
            .Setup(c => c.ExecuteCommandLineTool(
                It.IsAny<CommandLineToolOptions>(),
                It.IsAny<CommandExecutionOptions>(),
                It.IsAny<CancellationToken>()))
            .Callback<CommandLineToolOptions, CommandExecutionOptions?, CancellationToken>((_, _, ct) => capturedToken = ct)
            .ReturnsAsync(expectedResult);

        var builder = new TestToolBuilder(mockCommand.Object);
        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        await builder.ExecuteAsync(token);

        await Assert.That(capturedToken).IsEqualTo(token);
    }

    [Test]
    public async Task ExecuteAsync_PassesExecutionOptions()
    {
        var mockCommand = new Mock<ICommand>();
        var expectedResult = new CommandResult(
            "testtool command",
            "/test/dir",
            "output",
            "",
            new Dictionary<string, string?>(),
            DateTimeOffset.Now,
            DateTimeOffset.Now,
            TimeSpan.Zero,
            0);

        CommandExecutionOptions? capturedExecOptions = null;
        mockCommand
            .Setup(c => c.ExecuteCommandLineTool(
                It.IsAny<CommandLineToolOptions>(),
                It.IsAny<CommandExecutionOptions>(),
                It.IsAny<CancellationToken>()))
            .Callback<CommandLineToolOptions, CommandExecutionOptions?, CancellationToken>((_, exec, _) => capturedExecOptions = exec)
            .ReturnsAsync(expectedResult);

        var builder = new TestToolBuilder(mockCommand.Object);
        builder
            .WithWorkingDirectory("/test/dir")
            .WithTimeout(TimeSpan.FromMinutes(5));

        await builder.ExecuteAsync();

        await Assert.That(capturedExecOptions).IsNotNull();
        await Assert.That(capturedExecOptions!.WorkingDirectory).IsEqualTo("/test/dir");
        await Assert.That(capturedExecOptions.ExecutionTimeout).IsEqualTo(TimeSpan.FromMinutes(5));
    }

    #endregion

    #region ToOptions Tests

    [Test]
    public async Task ToOptions_ReturnsBothOptionsTuple()
    {
        var mockCommand = new Mock<ICommand>();
        var builder = new TestToolBuilder(mockCommand.Object);
        builder
            .WithConfiguration("Release")
            .WithWorkingDirectory("/project");

        var (toolOptions, execOptions) = builder.ToOptions();

        await Assert.That(toolOptions).IsNotNull();
        await Assert.That(execOptions).IsNotNull();
        await Assert.That(toolOptions.Configuration).IsEqualTo("Release");
        await Assert.That(execOptions.WorkingDirectory).IsEqualTo("/project");
    }

    [Test]
    public async Task ToOptions_CanBeCalledMultipleTimes()
    {
        var mockCommand = new Mock<ICommand>();
        var builder = new TestToolBuilder(mockCommand.Object);
        builder.WithConfiguration("Release");

        var (options1, _) = builder.ToOptions();
        builder.WithFramework("net8.0");
        var (options2, _) = builder.ToOptions();

        await Assert.That(options1.Configuration).IsEqualTo("Release");
        await Assert.That(options1.Framework).IsNull();
        await Assert.That(options2.Configuration).IsEqualTo("Release");
        await Assert.That(options2.Framework).IsEqualTo("net8.0");
    }

    #endregion

    #region Non-Generic Interface Tests

    [Test]
    public async Task NonGenericInterface_CanBeUsedForChaining()
    {
        var mockCommand = new Mock<ICommand>();
        var builder = new TestToolBuilder(mockCommand.Object);

        // Use via non-generic interface
        ICommandBuilder nonGenericBuilder = builder;
        nonGenericBuilder
            .WithWorkingDirectory("/test")
            .WithTimeout(TimeSpan.FromMinutes(1));

        // The underlying builder should still have the options set
        var (_, execOptions) = builder.ToOptions();
        await Assert.That(execOptions.WorkingDirectory).IsEqualTo("/test");
        await Assert.That(execOptions.ExecutionTimeout).IsEqualTo(TimeSpan.FromMinutes(1));
    }

    #endregion
}
