using System.Collections.ObjectModel;
using System.Reflection;
using CliWrap;
using CliWrap.Buffered;
using ModularPipelines.Attributes;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using CommandResult = ModularPipelines.Models.CommandResult;

namespace ModularPipelines.Context;

internal class Command(ISecretObfuscator secretObfuscator, ICommandLogger commandLogger) : ICommand
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

        var commandInput = secretObfuscator.Obfuscate(command.ToString(), optionsObject);

        string? inputToLog = null;
        string? outputToLog = null;
        string? errorToLog = null;
        int? resultExitCode = null;
        
        if (options.CommandLogging.HasFlag(CommandLogging.Input))
        {
            var inputLoggingManipulator = options.InputLoggingManipulator ?? (s => s);

            inputToLog = inputLoggingManipulator(commandInput);
        }

        try
        {
            if (options.InternalDryRun)
            {
                return new CommandResult(command);
            }

            var result = await Of(command, options, cancellationToken);

            var outputLoggingManipulator = options.OutputLoggingManipulator ?? (s => s);
            
            if (options.CommandLogging.HasFlag(CommandLogging.Output))
            {
                resultExitCode = result.ExitCode;
                outputToLog = outputLoggingManipulator(secretObfuscator.Obfuscate(result.StandardOutput, optionsObject));
            }

            if (options.CommandLogging.HasFlag(CommandLogging.Error))
            {
                resultExitCode = result.ExitCode;
                errorToLog = outputLoggingManipulator(secretObfuscator.Obfuscate(result.StandardError, optionsObject));
            }

            return new CommandResult(command, result);
        }
        finally
        {
            _commandLogger.Log(options, inputToLog, resultExitCode, outputToLog, errorToLog);
        }
    }

    private static object GetOptionsObject(CommandLineToolOptions options)
    {
        return options.OptionsObject ?? options;
    }

    private static async Task<BufferedCommandResult> Of(CliWrap.Command command,
        CommandLineToolOptions options, CancellationToken cancellationToken = default)
    {
        var result = await command
            .WithValidation(CommandResultValidation.None)
            .ExecuteBufferedAsync(cancellationToken);

        if (result.ExitCode != 0 && options.ThrowOnNonZeroExitCode)
        {
            var input = command.ToString();
            throw new CommandException(input, result);
        }

        return result;
    }
}
