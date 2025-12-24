# Command Logging System Cleanup Implementation Plan

> **For Claude:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** Simplify the command logging system by deprecating the legacy `CommandLogging` enum, implementing verbosity levels properly, removing unused code, and making options immutable.

**Architecture:** Replace dual logging systems with a single `CommandLoggingOptions`-based approach. Remove the `CommandLogging` flags enum and `CommandLogFormat` enum. Update `CommandLineOptions` to use `init` properties and include `CommandLoggingOptions` directly. Remove thread locking from `CommandLogger`.

**Tech Stack:** C#, .NET 10, TUnit for testing

---

## Task 1: Add Obsolete Attribute to CommandLogging Enum

**Files:**
- Modify: `src/ModularPipelines/Enums/CommandLogging.cs`

**Step 1: Add Obsolete attribute to the enum**

Open `src/ModularPipelines/Enums/CommandLogging.cs` and add the `[Obsolete]` attribute:

```csharp
namespace ModularPipelines.Enums;

/// <summary>
/// Enum to control the level of logging a command should do
/// Can combine multiple e.g. Input | Error.
/// </summary>
[Obsolete("Use CommandLoggingOptions instead. This enum will be removed in a future version.")]
[Flags]
public enum CommandLogging
{
    // ... existing members unchanged
}
```

**Step 2: Build to verify no errors**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: Build succeeds with CS0618 obsolete warnings

**Step 3: Commit**

```bash
git add src/ModularPipelines/Enums/CommandLogging.cs
git commit -m "chore: mark CommandLogging enum as obsolete"
```

---

## Task 2: Delete CommandLogFormat Enum and Property

**Files:**
- Delete: `src/ModularPipelines/Options/CommandLogFormat.cs`
- Modify: `src/ModularPipelines/Options/CommandLoggingOptions.cs`

**Step 1: Remove OutputFormat property from CommandLoggingOptions**

Open `src/ModularPipelines/Options/CommandLoggingOptions.cs` and remove the `OutputFormat` property and its documentation:

```csharp
namespace ModularPipelines.Options;

/// <summary>
/// Options for customizing command execution logging.
/// </summary>
public record CommandLoggingOptions
{
    /// <summary>
    /// Gets or sets the verbosity level. Default is Normal.
    /// </summary>
    public CommandLogVerbosity Verbosity { get; init; } = CommandLogVerbosity.Normal;

    // DELETE: OutputFormat property and its XML doc (lines 13-17)

    /// <summary>
    /// Gets or sets whether to include timestamps in output. Default is false.
    /// </summary>
    public bool IncludeTimestamps { get; init; }

    // ... rest of file unchanged
}
```

**Step 2: Delete the CommandLogFormat.cs file**

Run: `git rm src/ModularPipelines/Options/CommandLogFormat.cs`

**Step 3: Build to verify**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: Build succeeds

**Step 4: Commit**

```bash
git add -A
git commit -m "chore: remove unused CommandLogFormat enum and OutputFormat property"
```

---

## Task 3: Add CommandLoggingOptions Property to CommandLineOptions

**Files:**
- Modify: `src/ModularPipelines/Options/CommandLineOptions.cs`

**Step 1: Add the new property**

Open `src/ModularPipelines/Options/CommandLineOptions.cs` and add a `LoggingOptions` property:

```csharp
using CliWrap;
using ModularPipelines.Enums;

namespace ModularPipelines.Options;

/// <summary>
/// Options for setting the context of a command.
/// </summary>
public record CommandLineOptions
{
    /// <summary>
    /// Gets any EnvironmentVariables to pass to the command.
    /// </summary>
    public IDictionary<string, string?>? EnvironmentVariables { get; init; }

    /// <summary>
    /// Gets the working directory to run the command from.
    /// </summary>
    public string? WorkingDirectory { get; init; }

    /// <inheritdoc cref="CommandLineCredentials"/>>
    public Credentials? CommandLineCredentials { get; init; }

    /// <summary>
    /// Gets or sets logging options for command execution.
    /// When set, this takes precedence over the legacy CommandLogging property.
    /// </summary>
    public CommandLoggingOptions? LoggingOptions { get; init; }

    /// <summary>
    /// Gets controls command logging
    /// e.g. Logging = CommandLogging.Input | CommandLogging.Output | CommandLogging.Error.
    /// </summary>
    [Obsolete("Use LoggingOptions instead. This property will be removed in a future version.")]
    public CommandLogging? CommandLogging { get; init; }

    /// <summary>
    /// Gets if logging input, you can use this to edit how the input is logged
    /// E.g. if you want to replace a secret value with stars.
    /// </summary>
    public Func<string, string>? InputLoggingManipulator { get; init; }

    /// <summary>
    /// Gets if logging output, you can use this to edit how the output is logged
    /// E.g. if you want to replace a secret value with stars.
    /// </summary>
    public Func<string, string>? OutputLoggingManipulator { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether prefix commands with Sudo to run with elevated priveliges for Unix systems.
    /// </summary>
    public bool Sudo { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether to throw an exception on non-zero exit codes.
    /// </summary>
    public bool ThrowOnNonZeroExitCode { get; init; } = true;

    internal bool InternalDryRun { get; set; }
}
```

**Step 2: Build to verify**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: Build succeeds (may have obsolete warnings)

**Step 3: Commit**

```bash
git add src/ModularPipelines/Options/CommandLineOptions.cs
git commit -m "feat: add LoggingOptions property to CommandLineOptions, make Sudo and ThrowOnNonZeroExitCode init-only"
```

---

## Task 4: Update ICommand Interface to Remove Separate Logging Parameter

**Files:**
- Modify: `src/ModularPipelines/Context/ICommand.cs`

**Step 1: Mark the overload with CommandLoggingOptions as obsolete**

Open `src/ModularPipelines/Context/ICommand.cs`:

```csharp
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

public interface ICommand
{
    /// <summary>
    /// Execute a command line tool.
    /// </summary>
    /// <param name="options">The command options.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<CommandResult> ExecuteCommandLineTool(CommandLineToolOptions options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Execute a command line tool with custom logging options.
    /// </summary>
    /// <param name="options">The command options.</param>
    /// <param name="loggingOptions">The logging options for this command execution.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Obsolete("Set LoggingOptions on CommandLineToolOptions instead. This overload will be removed in a future version.")]
    Task<CommandResult> ExecuteCommandLineTool(CommandLineToolOptions options, CommandLoggingOptions? loggingOptions, CancellationToken cancellationToken = default);
}
```

**Step 2: Build to verify**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: Build succeeds

**Step 3: Commit**

```bash
git add src/ModularPipelines/Context/ICommand.cs
git commit -m "chore: mark separate loggingOptions parameter overload as obsolete"
```

---

## Task 5: Implement Verbosity Levels in CommandLogger

**Files:**
- Modify: `src/ModularPipelines/Logging/CommandLogger.cs`

**Step 1: Rewrite CommandLogger to use verbosity levels**

Open `src/ModularPipelines/Logging/CommandLogger.cs` and replace the entire content:

```csharp
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Helpers;
using ModularPipelines.Options;

namespace ModularPipelines.Logging;

internal class CommandLogger : ICommandLogger
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly ISecretObfuscator _secretObfuscator;

    public CommandLogger(IModuleLoggerProvider moduleLoggerProvider,
        IOptions<PipelineOptions> pipelineOptions,
        ISecretObfuscator secretObfuscator)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
        _pipelineOptions = pipelineOptions;
        _secretObfuscator = secretObfuscator;
    }

    private ILogger Logger => _moduleLoggerProvider.GetLogger();

    public void Log(
        CommandLineToolOptions options,
        CommandLoggingOptions? loggingOptions,
        string? inputToLog,
        int? exitCode,
        TimeSpan? runTime,
        string standardOutput,
        string standardError,
        string commandWorkingDirPath)
    {
        // Determine effective logging options
        var effectiveOptions = GetEffectiveLoggingOptions(options, loggingOptions);

        // Silent = no logging at all
        if (effectiveOptions.Verbosity == CommandLogVerbosity.Silent)
        {
            return;
        }

        if (options.InternalDryRun)
        {
            LogDryRunCommand(effectiveOptions, commandWorkingDirPath, inputToLog);
            return;
        }

        LogCommandInput(effectiveOptions, options, commandWorkingDirPath, inputToLog);
        LogExitCode(effectiveOptions, exitCode);
        LogDuration(effectiveOptions, runTime);
        LogOutput(effectiveOptions, options, standardOutput);
        LogError(effectiveOptions, options, exitCode, standardError);
        LogWorkingDirectory(effectiveOptions, commandWorkingDirPath);
    }

    private CommandLoggingOptions GetEffectiveLoggingOptions(CommandLineToolOptions options, CommandLoggingOptions? parameterOptions)
    {
        // Priority: parameter > property > default based on legacy enum
        if (parameterOptions is not null)
        {
            return parameterOptions;
        }

        if (options.LoggingOptions is not null)
        {
            return options.LoggingOptions;
        }

#pragma warning disable CS0618 // Type or member is obsolete
        // Fall back to legacy CommandLogging enum conversion
        var legacyLogging = options.CommandLogging ?? _pipelineOptions.Value.DefaultCommandLogging;
        return ConvertFromLegacy(legacyLogging);
#pragma warning restore CS0618
    }

#pragma warning disable CS0618 // Type or member is obsolete
    private static CommandLoggingOptions ConvertFromLegacy(CommandLogging legacy)
    {
        if (legacy == CommandLogging.None)
        {
            return CommandLoggingOptions.Silent;
        }

        return new CommandLoggingOptions
        {
            Verbosity = CommandLogVerbosity.Normal,
            ShowCommandArguments = legacy.HasFlag(CommandLogging.Input),
            ShowStandardOutput = legacy.HasFlag(CommandLogging.Output),
            ShowStandardError = legacy.HasFlag(CommandLogging.Error),
            ShowExitCode = legacy.HasFlag(CommandLogging.ExitCode),
            ShowExecutionTime = legacy.HasFlag(CommandLogging.Duration),
        };
    }
#pragma warning restore CS0618

    private void LogDryRunCommand(CommandLoggingOptions options, string workingDirectory, string? input)
    {
        if (!ShouldShowInput(options))
        {
            return;
        }

        var timestamp = GetTimestampPrefix(options);
        Logger.LogInformation("{Timestamp}Command (Dry-Run): {WorkingDirectory}> {Input}",
            timestamp,
            workingDirectory,
            input);
        Logger.LogInformation("{Timestamp}⚠ Dry-Run: No actual execution", timestamp);
    }

    private void LogCommandInput(CommandLoggingOptions options, CommandLineToolOptions commandOptions, string workingDirectory, string? input)
    {
        // Minimal and above shows command input
        if (options.Verbosity < CommandLogVerbosity.Minimal)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(options);

        if (ShouldShowInput(options))
        {
            Logger.LogInformation("{Timestamp}Command: {WorkingDirectory}> {Input}",
                timestamp,
                workingDirectory,
                _secretObfuscator.Obfuscate(input, commandOptions));
        }
        else
        {
            Logger.LogInformation("{Timestamp}Command: {WorkingDirectory}> ********",
                timestamp,
                workingDirectory);
        }
    }

    private void LogExitCode(CommandLoggingOptions options, int? exitCode)
    {
        // Detailed and above shows exit code, or if explicitly requested
        if (options.Verbosity < CommandLogVerbosity.Detailed && !options.ShowExitCode)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(options);
        var icon = exitCode == 0 ? "✓" : "✗";
        Logger.LogInformation("{Timestamp}{Icon} Exit Code: {ExitCode}", timestamp, icon, exitCode);
    }

    private void LogDuration(CommandLoggingOptions options, TimeSpan? runTime)
    {
        // Detailed and above shows duration, or if explicitly requested
        if (options.Verbosity < CommandLogVerbosity.Detailed && !options.ShowExecutionTime)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(options);
        Logger.LogInformation("{Timestamp}Duration: {Duration}", timestamp, runTime?.ToDisplayString());
    }

    private void LogOutput(CommandLoggingOptions options, CommandLineToolOptions commandOptions, string standardOutput)
    {
        // Normal and above shows output, or if explicitly requested
        if ((options.Verbosity < CommandLogVerbosity.Normal && !options.ShowStandardOutput)
            || string.IsNullOrWhiteSpace(standardOutput))
        {
            return;
        }

        if (!options.ShowStandardOutput)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(options);
        Logger.LogInformation("{Timestamp}Output:\n{Output}", timestamp, _secretObfuscator.Obfuscate(standardOutput, commandOptions));
    }

    private void LogError(CommandLoggingOptions options, CommandLineToolOptions commandOptions, int? exitCode, string standardError)
    {
        // Normal and above shows errors (when exit code != 0), or if explicitly requested
        if ((options.Verbosity < CommandLogVerbosity.Normal && !options.ShowStandardError)
            || string.IsNullOrWhiteSpace(standardError))
        {
            return;
        }

        if (!options.ShowStandardError || exitCode == 0)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(options);
        Logger.LogInformation("{Timestamp}✗ Error:\n{Error}", timestamp, _secretObfuscator.Obfuscate(standardError, commandOptions));
    }

    private void LogWorkingDirectory(CommandLoggingOptions options, string workingDirectory)
    {
        // Diagnostic shows working directory, or if explicitly requested
        if (options.Verbosity < CommandLogVerbosity.Diagnostic && !options.ShowWorkingDirectory)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(options);
        Logger.LogInformation("{Timestamp}Working Directory: {WorkingDirectory}", timestamp, workingDirectory);
    }

    private static string GetTimestampPrefix(CommandLoggingOptions options)
    {
        // Diagnostic automatically includes timestamps, or if explicitly requested
        if (options.Verbosity < CommandLogVerbosity.Diagnostic && !options.IncludeTimestamps)
        {
            return string.Empty;
        }

        return $"[{DateTime.UtcNow:HH:mm:ss.fff}] ";
    }

    private static bool ShouldShowInput(CommandLoggingOptions options)
    {
        // ShowCommandArguments controls whether to show full command or obfuscated
        return options.ShowCommandArguments;
    }
}
```

**Step 2: Build to verify**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: Build succeeds

**Step 3: Commit**

```bash
git add src/ModularPipelines/Logging/CommandLogger.cs
git commit -m "feat: implement verbosity levels in CommandLogger, remove thread lock"
```

---

## Task 6: Update Command.cs to Use LoggingOptions from Options

**Files:**
- Modify: `src/ModularPipelines/Context/Command.cs`

**Step 1: Update ExecuteCommandLineTool to prefer LoggingOptions property**

Open `src/ModularPipelines/Context/Command.cs` and update the method:

```csharp
public async Task<CommandResult> ExecuteCommandLineTool(CommandLineToolOptions options, CommandLoggingOptions? loggingOptions, CancellationToken cancellationToken = default)
{
    // Prefer LoggingOptions from options property over parameter
    var effectiveLoggingOptions = options.LoggingOptions ?? loggingOptions;

    var optionsObject = GetOptionsObject(options);

    // ... rest of method unchanged, but pass effectiveLoggingOptions instead of loggingOptions to _commandLogger.Log calls
```

Find all calls to `_commandLogger.Log` and ensure they pass `effectiveLoggingOptions` instead of `loggingOptions`.

**Step 2: Build to verify**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: Build succeeds

**Step 3: Commit**

```bash
git add src/ModularPipelines/Context/Command.cs
git commit -m "feat: Command.cs uses LoggingOptions property from options"
```

---

## Task 7: Update PipelineOptions to Use CommandLoggingOptions

**Files:**
- Modify: `src/ModularPipelines/Options/PipelineOptions.cs`

**Step 1: Add new property and mark old as obsolete**

Open `src/ModularPipelines/Options/PipelineOptions.cs`:

```csharp
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Enums;
using Spectre.Console;

namespace ModularPipelines.Options;

/// <summary>
/// Configuration options for pipeline execution behavior.
/// </summary>
[ExcludeFromCodeCoverage]
public record PipelineOptions
{
    // ... existing properties unchanged until DefaultCommandLogging ...

    /// <summary>
    /// Gets or sets the default logging options for all commands.
    /// When set, this takes precedence over DefaultCommandLogging.
    /// </summary>
    public CommandLoggingOptions? DefaultLoggingOptions { get; set; }

    /// <summary>
    /// Gets or sets the default command logging level for all commands.
    /// </summary>
    [Obsolete("Use DefaultLoggingOptions instead. This property will be removed in a future version.")]
    public CommandLogging DefaultCommandLogging { get; set; } = CommandLogging.Default;

    // ... rest unchanged
}
```

**Step 2: Build to verify**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: Build succeeds

**Step 3: Commit**

```bash
git add src/ModularPipelines/Options/PipelineOptions.cs
git commit -m "feat: add DefaultLoggingOptions to PipelineOptions, mark DefaultCommandLogging as obsolete"
```

---

## Task 8: Update CommandLogger to Use New PipelineOptions Property

**Files:**
- Modify: `src/ModularPipelines/Logging/CommandLogger.cs`

**Step 1: Update GetEffectiveLoggingOptions to check DefaultLoggingOptions first**

In `CommandLogger.cs`, update the `GetEffectiveLoggingOptions` method:

```csharp
private CommandLoggingOptions GetEffectiveLoggingOptions(CommandLineToolOptions options, CommandLoggingOptions? parameterOptions)
{
    // Priority: parameter > property > pipeline default > legacy enum
    if (parameterOptions is not null)
    {
        return parameterOptions;
    }

    if (options.LoggingOptions is not null)
    {
        return options.LoggingOptions;
    }

    if (_pipelineOptions.Value.DefaultLoggingOptions is not null)
    {
        return _pipelineOptions.Value.DefaultLoggingOptions;
    }

#pragma warning disable CS0618 // Type or member is obsolete
    // Fall back to legacy CommandLogging enum conversion
    var legacyLogging = options.CommandLogging ?? _pipelineOptions.Value.DefaultCommandLogging;
    return ConvertFromLegacy(legacyLogging);
#pragma warning restore CS0618
}
```

**Step 2: Build to verify**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: Build succeeds

**Step 3: Commit**

```bash
git add src/ModularPipelines/Logging/CommandLogger.cs
git commit -m "feat: CommandLogger checks DefaultLoggingOptions before legacy enum"
```

---

## Task 9: Write Tests for Verbosity Levels

**Files:**
- Modify: `test/ModularPipelines.UnitTests/CommandLoggerTests.cs`

**Step 1: Add tests for each verbosity level**

Open `test/ModularPipelines.UnitTests/CommandLoggerTests.cs` and add new test methods:

```csharp
[Test]
public async Task Silent_Verbosity_Logs_Nothing()
{
    var file = await RunPowershellCommandWithLoggingOptions(
        "echo Hello",
        new CommandLoggingOptions { Verbosity = CommandLogVerbosity.Silent });

    var logFile = await File.ReadAllTextAsync(file);
    await Assert.That(logFile).DoesNotContain("[ModularPipelines.Logging.CommandLogger]");
}

[Test]
public async Task Minimal_Verbosity_Logs_Only_Input()
{
    var file = await RunPowershellCommandWithLoggingOptions(
        "echo Hello",
        new CommandLoggingOptions { Verbosity = CommandLogVerbosity.Minimal });

    var logFile = await File.ReadAllTextAsync(file);
    await Assert.That(logFile).Contains("Command:");
    await Assert.That(logFile).DoesNotContain("Output:");
    await Assert.That(logFile).DoesNotContain("Exit Code:");
    await Assert.That(logFile).DoesNotContain("Duration:");
}

[Test]
public async Task Normal_Verbosity_Logs_Input_And_Output()
{
    var file = await RunPowershellCommandWithLoggingOptions(
        "echo Hello",
        new CommandLoggingOptions { Verbosity = CommandLogVerbosity.Normal });

    var logFile = await File.ReadAllTextAsync(file);
    await Assert.That(logFile).Contains("Command:");
    await Assert.That(logFile).Contains("Output:");
    await Assert.That(logFile).DoesNotContain("Exit Code:");
    await Assert.That(logFile).DoesNotContain("Duration:");
}

[Test]
public async Task Detailed_Verbosity_Logs_Input_Output_ExitCode_Duration()
{
    var file = await RunPowershellCommandWithLoggingOptions(
        "echo Hello",
        new CommandLoggingOptions { Verbosity = CommandLogVerbosity.Detailed });

    var logFile = await File.ReadAllTextAsync(file);
    await Assert.That(logFile).Contains("Command:");
    await Assert.That(logFile).Contains("Output:");
    await Assert.That(logFile).Contains("Exit Code:");
    await Assert.That(logFile).Contains("Duration:");
}

[Test]
public async Task Diagnostic_Verbosity_Logs_Everything_Including_Timestamps()
{
    var file = await RunPowershellCommandWithLoggingOptions(
        "echo Hello",
        new CommandLoggingOptions { Verbosity = CommandLogVerbosity.Diagnostic });

    var logFile = await File.ReadAllTextAsync(file);
    await Assert.That(logFile).Contains("Command:");
    await Assert.That(logFile).Contains("Output:");
    await Assert.That(logFile).Contains("Exit Code:");
    await Assert.That(logFile).Contains("Duration:");
    await Assert.That(logFile).Contains("Working Directory:");
    // Check for timestamp pattern [HH:mm:ss.fff]
    await Assert.That(logFile).Contains(Regex.Match(logFile, @"\[\d{2}:\d{2}:\d{2}\.\d{3}\]").Success).IsTrue();
}

private async Task<string> RunPowershellCommandWithLoggingOptions(string command, CommandLoggingOptions loggingOptions)
{
    var file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt");

    var result = await GetService<ICommand>((_, collection) =>
    {
        collection.Configure<SpectreLoggerOptions>(options => options.MinimumLogLevel = LogLevel.Information);
        collection.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
        collection.AddLogging(builder => { builder.AddFile(file); });
    });

    await result.T.ExecuteCommandLineTool(new PowershellScriptOptions(command)
    {
        LoggingOptions = loggingOptions,
        ThrowOnNonZeroExitCode = false,
    });

    await result.Host.DisposeAsync();

    return file;
}
```

**Step 2: Add required using statement**

Add at top of file: `using System.Text.RegularExpressions;`

**Step 3: Run the tests**

Run: `dotnet test test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj --filter "FullyQualifiedName~CommandLoggerTests" --framework net10.0`
Expected: All tests pass

**Step 4: Commit**

```bash
git add test/ModularPipelines.UnitTests/CommandLoggerTests.cs
git commit -m "test: add tests for verbosity levels in CommandLogger"
```

---

## Task 10: Update Usages of CommandLogging.None to Use LoggingOptions

**Files:**
- Modify: `src/ModularPipelines.Git/GitCommandRunner.cs`
- Modify: `src/ModularPipelines.Git/StaticGitInformation.cs`
- Modify: `src/ModularPipelines.GitHub/GitHubRepositoryInfo.cs`

**Step 1: Update GitCommandRunner.cs**

```csharp
var commandLineToolOptions = commandEnvironmentOptions.ToCommandLineToolOptions("git", commands.OfType<string>().ToArray()) with
{
    LoggingOptions = CommandLoggingOptions.Silent,
};
```

**Step 2: Update StaticGitInformation.cs GetGitVersion method**

```csharp
var result = await _command.ExecuteCommandLineTool(new CommandLineToolOptions("git")
{
    Arguments = ["version"],
    LoggingOptions = CommandLoggingOptions.Silent,
});
```

**Step 3: Update StaticGitInformation.cs GetOutput method**

```csharp
var result = await _command.ExecuteCommandLineTool(gitOptions with
{
    LoggingOptions = _logger.IsEnabled(LogLevel.Debug) ? CommandLoggingOptions.Diagnostic : CommandLoggingOptions.Silent,
});
```

**Step 4: Update GitHubRepositoryInfo.cs**

```csharp
var options = new GitRemoteOptions
{
    Arguments = ["get-url", "origin"],
    ThrowOnNonZeroExitCode = false,
    LoggingOptions = scope.ServiceProvider
        .GetRequiredService<IOptions<LoggerFilterOptions>>()
        .Value
        .MinLevel == LogLevel.Debug
        ? CommandLoggingOptions.Diagnostic
        : CommandLoggingOptions.Silent,
};
```

**Step 5: Build to verify**

Run: `dotnet build ModularPipelines.sln -c Release`
Expected: Build succeeds

**Step 6: Commit**

```bash
git add src/ModularPipelines.Git/GitCommandRunner.cs src/ModularPipelines.Git/StaticGitInformation.cs src/ModularPipelines.GitHub/GitHubRepositoryInfo.cs
git commit -m "refactor: update Git and GitHub modules to use LoggingOptions instead of CommandLogging enum"
```

---

## Task 11: Update CommandLoggerTests to Suppress Obsolete Warnings

**Files:**
- Modify: `test/ModularPipelines.UnitTests/CommandLoggerTests.cs`

**Step 1: Add pragma to suppress obsolete warnings for legacy tests**

Around the existing tests that use `CommandLogging` enum, add pragmas:

```csharp
#pragma warning disable CS0618 // Type or member is obsolete
[Test]
[MatrixDataSource]
public async Task Logs_As_Expected_With_Options(
    // ... existing test unchanged
)
{
    // ... existing implementation
}

private async Task<string> RunPowershellCommand(string command, bool logInput, bool logOutput, bool logError,
    bool logExitCode, bool logDuration)
{
    // ... existing implementation with CommandLogging enum
}
#pragma warning restore CS0618
```

**Step 2: Build and run tests**

Run: `dotnet test test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj --filter "FullyQualifiedName~CommandLoggerTests" --framework net10.0`
Expected: All tests pass, no obsolete warnings in test output

**Step 3: Commit**

```bash
git add test/ModularPipelines.UnitTests/CommandLoggerTests.cs
git commit -m "test: suppress obsolete warnings in legacy CommandLogger tests"
```

---

## Task 12: Run Full Test Suite and Fix Any Issues

**Files:**
- May need to modify various test files if issues arise

**Step 1: Build entire solution**

Run: `dotnet build ModularPipelines.sln -c Release`
Expected: Build succeeds (may have obsolete warnings, which is expected)

**Step 2: Run all unit tests**

Run: `dotnet test ModularPipelines.sln -c Release --framework net10.0`
Expected: All tests pass

**Step 3: If tests fail, investigate and fix**

Review any failing tests and make necessary adjustments.

**Step 4: Commit any fixes**

```bash
git add -A
git commit -m "fix: address any test failures from logging refactor"
```

---

## Task 13: Update Documentation in CommandLoggingOptions

**Files:**
- Modify: `src/ModularPipelines/Options/CommandLoggingOptions.cs`

**Step 1: Add XML documentation explaining verbosity levels**

```csharp
namespace ModularPipelines.Options;

/// <summary>
/// Options for customizing command execution logging.
/// </summary>
/// <remarks>
/// <para>Verbosity levels control what is logged automatically:</para>
/// <list type="bullet">
/// <item><description><see cref="CommandLogVerbosity.Silent"/> - No logging</description></item>
/// <item><description><see cref="CommandLogVerbosity.Minimal"/> - Command input only</description></item>
/// <item><description><see cref="CommandLogVerbosity.Normal"/> - Input, output, and errors</description></item>
/// <item><description><see cref="CommandLogVerbosity.Detailed"/> - Above plus exit code and duration</description></item>
/// <item><description><see cref="CommandLogVerbosity.Diagnostic"/> - Everything including working directory and timestamps</description></item>
/// </list>
/// <para>Individual Show* properties can override the verbosity defaults.</para>
/// </remarks>
public record CommandLoggingOptions
{
    // ... rest unchanged
}
```

**Step 2: Commit**

```bash
git add src/ModularPipelines/Options/CommandLoggingOptions.cs
git commit -m "docs: add XML documentation explaining verbosity levels"
```

---

## Summary

After completing all tasks, the command logging system will:

1. Have `CommandLogging` enum marked as `[Obsolete]`
2. Have `CommandLogFormat` enum deleted entirely
3. Use `CommandLoggingOptions` as the primary logging configuration
4. Support verbosity levels (Silent → Diagnostic) with proper semantics
5. Use `init` properties for immutability on `CommandLineOptions`
6. No longer use thread locking in `CommandLogger`
7. Have `LoggingOptions` property on `CommandLineOptions` and `DefaultLoggingOptions` on `PipelineOptions`
8. Have all internal usages migrated to the new `LoggingOptions` pattern
