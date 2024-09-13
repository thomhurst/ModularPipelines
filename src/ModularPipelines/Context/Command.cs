using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using CliWrap;
using CliWrap.Exceptions;
using ModularPipelines.Attributes;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using CommandResult = ModularPipelines.Models.CommandResult;

namespace ModularPipelines.Context;

public sealed class Command(ICommandLogger commandLogger) : ICommand
{
    private readonly ICommandLogger _commandLogger = commandLogger;

    public async Task<CommandResult> ExecuteCommandLineTool(CommandLineToolOptions options, CancellationToken cancellationToken = default)
    {
        var optionsObject = GetOptionsObject(options);

        var precedingArgs = GetPrecedingArguments(optionsObject);

        CommandOptionsObjectArgumentParser.AddArgumentsFromOptionsObject(precedingArgs, optionsObject);

        var parsedArgs = (string.Equals(options.Arguments?.ElementAtOrDefault(0), options.Tool)
            ? options.Arguments?.Skip(1).ToList() : options.Arguments?.ToList()) ?? new List<string>();

        parsedArgs = precedingArgs.Concat(parsedArgs).ToList();

        if (options.RunSettings != null)
        {
            parsedArgs.Add("--");
            parsedArgs.AddRange(options.RunSettings);
        }

        string tool;
        if (options.Sudo)
        {
            tool = "sudo";
            parsedArgs.Insert(0, options.Tool);
        }
        else
        {
            tool = options.Tool;
        }

        var command = Cli.Wrap(tool).WithArguments(SantiseArguments(parsedArgs));

        if (options.WorkingDirectory != null)
        {
            command = command.WithWorkingDirectory(options.WorkingDirectory);
        }

        if (options.EnvironmentVariables != null)
        {
            command = command.WithEnvironmentVariables(new ReadOnlyDictionary<string, string?>(options.EnvironmentVariables));
        }

        if (options.InternalDryRun)
        {
            _commandLogger.Log(options,
                inputToLog: options.InputLoggingManipulator == null ? command.ToString() : options.InputLoggingManipulator(command.ToString()),
                exitCode: 0,
                runTime: TimeSpan.Zero,
                standardOutput: "Dummy Output Response",
                standardError: "Dummy Error Response",
                command.WorkingDirPath
            );

            return new CommandResult(command);
        }

        return await Of(command, options, cancellationToken);
    }

    private List<string> SantiseArguments(List<string> parsedArgs)
    {
        parsedArgs.RemoveAll(x => x.StartsWith("<"));
        
        return parsedArgs;
    }

    private static List<string> GetPrecedingArguments(object optionsObject)
    {
        if (optionsObject is CommandLineToolOptions { CommandParts: not null } commandLineToolOptions)
        {
            return commandLineToolOptions.CommandParts.ToList();
        }

        return optionsObject.GetType().GetCustomAttribute<CommandPrecedingArgumentsAttribute>()
            ?.PrecedingArguments.ToList() ?? new List<string>();
    }

    private static object GetOptionsObject(CommandLineToolOptions options)
    {
        return options.OptionsObject ?? options;
    }

    private async Task<CommandResult> Of(CliWrap.Command command,
        CommandLineToolOptions options, CancellationToken cancellationToken = default)
    {
        StringBuilder standardOutputStringBuilder = new();
        StringBuilder standardErrorStringBuilder = new();
        var stopwatch = Stopwatch.StartNew();

        var standardOutput = string.Empty;
        var standardError = string.Empty;

        var inputToLog = options.InputLoggingManipulator == null ? command.ToString() : options.InputLoggingManipulator(command.ToString());

        using var forcefulCancellationToken = new CancellationTokenSource();
        
        cancellationToken.Register(() =>
        {
            try
            {
                if (forcefulCancellationToken.Token.CanBeCanceled)
                {
                    forcefulCancellationToken.CancelAfter(TimeSpan.FromSeconds(30));
                }
            }
            catch (ObjectDisposedException)
            {
                // Ignored
            }
        });
        
        try
        {
            var result = await command
                .WithStandardOutputPipe(PipeTarget.ToStringBuilder(standardOutputStringBuilder))
                .WithStandardErrorPipe(PipeTarget.ToStringBuilder(standardErrorStringBuilder))
                .WithValidation(CommandResultValidation.None)
                .ExecuteAsync(forcefulCancellationToken.Token, cancellationToken);

            standardOutput = options.OutputLoggingManipulator == null ? standardOutputStringBuilder.ToString() : options.OutputLoggingManipulator(standardOutputStringBuilder.ToString());
            standardError = options.OutputLoggingManipulator == null ? standardErrorStringBuilder.ToString() : options.OutputLoggingManipulator(standardErrorStringBuilder.ToString());

            _commandLogger.Log(options: options,
                inputToLog: inputToLog,
                exitCode: result.ExitCode,
                runTime: result.RunTime,
                standardOutput: standardOutput,
                standardError: standardError,
                commandWorkingDirPath: command.WorkingDirPath
            );

            if (result.ExitCode != 0 && options.ThrowOnNonZeroExitCode)
            {
                var input = inputToLog;
                throw new CommandException(input, result.ExitCode, result.RunTime, standardOutput, standardError);
            }

            return new CommandResult(command, result, standardOutput, standardError);
        }
        catch (CommandExecutionException e)
        {
            standardOutput = options.OutputLoggingManipulator == null ? standardOutputStringBuilder.ToString() : options.OutputLoggingManipulator(standardOutputStringBuilder.ToString());
            standardError = options.OutputLoggingManipulator == null ? standardErrorStringBuilder.ToString() : options.OutputLoggingManipulator(standardErrorStringBuilder.ToString());

            _commandLogger.Log(options: options,
                inputToLog: inputToLog,
                exitCode: e.ExitCode,
                runTime: stopwatch.Elapsed,
                standardOutput: standardOutput,
                standardError: standardError,
                commandWorkingDirPath: command.WorkingDirPath
            );
            
            throw new CommandException(inputToLog, e.ExitCode, stopwatch.Elapsed, standardOutput, standardError, e);
        }
        catch (Exception e) when (e is not CommandExecutionException or CommandException)
        {
            standardOutput = options.OutputLoggingManipulator == null ? standardOutputStringBuilder.ToString() : options.OutputLoggingManipulator(standardOutputStringBuilder.ToString());
            standardError = options.OutputLoggingManipulator == null ? standardErrorStringBuilder.ToString() : options.OutputLoggingManipulator(standardErrorStringBuilder.ToString());

            _commandLogger.Log(options: options,
                inputToLog: inputToLog,
                exitCode: -1,
                runTime: stopwatch.Elapsed,
                standardOutput: standardOutput,
                standardError: standardError,
                commandWorkingDirPath: command.WorkingDirPath
            );

            throw new CommandException(inputToLog, -1, stopwatch.Elapsed, standardOutput, standardError, e);
        }
    }
}