using System.Collections.ObjectModel;
using System.Reflection;
using CliWrap;
using CliWrap.Buffered;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using CommandResult = ModularPipelines.Models.CommandResult;

namespace ModularPipelines.Context;

internal class Command : ICommand
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly ISecretObfuscator _secretObfuscator;
    private ILogger Logger => _moduleLoggerProvider.GetLogger();

    public Command(IModuleLoggerProvider moduleLoggerProvider,
        ISecretObfuscator secretObfuscator)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
        _secretObfuscator = secretObfuscator;
    }

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

        var commandInput = _secretObfuscator.Obfuscate(command.ToString(), optionsObject);

        string? inputToLog = null;
        string? outputToLog = null;
        int? resultExitCode = null;
        
        if (options.LogInput)
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

            if (options.LogOutput)
            {
                var outputLoggingManipulator = options.OutputLoggingManipulator ?? (s => s);

                resultExitCode = result.ExitCode;
                outputToLog = string.IsNullOrEmpty(result.StandardError)
                    ? outputLoggingManipulator(_secretObfuscator.Obfuscate(result.StandardOutput, optionsObject))
                    : outputLoggingManipulator(_secretObfuscator.Obfuscate(result.StandardError, optionsObject));
            }

            return new CommandResult(command, result);
        }
        finally
        {
            if (options is { LogInput: true, InternalDryRun: true })
            {
                Logger.LogInformation("---Executing Command---\r\n\t{Input}", inputToLog);
                Logger.LogInformation("---Dry-Run Command - No Output---");
            }
            
            if (options is { LogInput: true, LogOutput: true } && (resultExitCode != null || outputToLog != null))
            {
                Logger.LogInformation("---Executing Command---\r\n\t{Input}\r\n\r\n---Command Result---\r\n\tExit Code: {ExitCode}\r\n\t{Output}", inputToLog, resultExitCode, outputToLog);
            }
            
            if (options is { LogInput: true, LogOutput: false } || (options is { LogInput: true, LogOutput: true } && resultExitCode == null && outputToLog == null))
            {
                Logger.LogInformation("---Executing Command---\r\n\t{Input}", inputToLog);
            }
            
            if (options is { LogInput: false, LogOutput: true })
            {
                Logger.LogInformation("---Command Result---\r\n\tExit Code: {ExitCode}\r\n\t{Output}",
                    resultExitCode, outputToLog
                );
            }
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
