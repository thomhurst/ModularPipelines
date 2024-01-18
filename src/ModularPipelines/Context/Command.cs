using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using CliWrap;
using CliWrap.Buffered;
using CliWrap.Exceptions;
using ModularPipelines.Attributes;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using CommandResult = ModularPipelines.Models.CommandResult;

namespace ModularPipelines.Context;

internal class Command(ICommandLogger commandLogger) : ICommand
{
    private readonly ICommandLogger _commandLogger = commandLogger;

    public async Task<CommandResult> ExecuteCommandLineTool(CommandLineToolOptions options, CancellationToken cancellationToken = default)
    {
        var optionsObject = GetOptionsObject(options);

        var precedingArgs =
            optionsObject.GetType().GetCustomAttribute<CommandPrecedingArgumentsAttribute>()
                ?.PrecedingArguments.ToList() ?? new List<string>();

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

        var command = Cli.Wrap(tool).WithArguments(parsedArgs);

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
                inputToLog: command.ToString(),
                exitCode: 0,
                runTime: TimeSpan.Zero,
                standardOutput: "Dummy Output Response",
                standardError: "Dummy Error Response"
            );

            return new CommandResult(command);
        }

        return await Of(command, options, cancellationToken);
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
        
        try
        {
            var result = await command
                .WithStandardOutputPipe(PipeTarget.ToStringBuilder(standardOutputStringBuilder))
                .WithStandardErrorPipe(PipeTarget.ToStringBuilder(standardErrorStringBuilder))
                .WithValidation(CommandResultValidation.None)
                .ExecuteAsync(cancellationToken);
            
            standardOutput = standardOutputStringBuilder.ToString();
            standardError = standardErrorStringBuilder.ToString();

            _commandLogger.Log(options: options,
                inputToLog: command.ToString(),
                result.ExitCode,
                result.RunTime,
                standardOutput,
                standardError
            );

            if (result.ExitCode != 0 && options.ThrowOnNonZeroExitCode)
            {
                var input = command.ToString();
                throw new CommandException(input, result.ExitCode, result.RunTime, standardOutput, standardError);
            }

            return new CommandResult(command, result, standardOutput, standardError);
        }
        catch (CommandExecutionException e)
        {
            _commandLogger.Log(options: options,
                inputToLog: command.ToString(),
                e.ExitCode,
                stopwatch.Elapsed,
                standardOutput,
                standardError
            );
            throw;
        }
        catch (Exception e) when (e is not CommandExecutionException)
        {
            _commandLogger.Log(options: options,
                inputToLog: command.ToString(),
                -1,
                stopwatch.Elapsed,
                standardOutput,
                standardError
            );
            
            throw;
        }
    }
}