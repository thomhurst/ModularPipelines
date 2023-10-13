using System;
using System.Collections.ObjectModel;
using System.Reflection;
using CliWrap;
using CliWrap.Buffered;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using CommandResult = ModularPipelines.Models.CommandResult;

namespace ModularPipelines.Context;

internal class Command(ISecretObfuscator secretObfuscator, ICommandLogger commandLogger, IOptions<PipelineOptions> pipelineOptions) : ICommand
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
            _commandLogger.Log(options, command.ToString(), 
                new BufferedCommandResult(0, 
                    DateTimeOffset.UtcNow, 
                    DateTimeOffset.UtcNow,
                    "Dummy Output Response",
                    "Dummy Error Response"));
            
            return new CommandResult(command);
        }

        var result = await Of(command, options, cancellationToken);

        return new CommandResult(command, result);
    }

    private static object GetOptionsObject(CommandLineToolOptions options)
    {
        return options.OptionsObject ?? options;
    }

    private async Task<BufferedCommandResult> Of(CliWrap.Command command,
        CommandLineToolOptions options, CancellationToken cancellationToken = default)
    {
        var result = await command
            .WithValidation(CommandResultValidation.None)
            .ExecuteBufferedAsync(cancellationToken);
        
        _commandLogger.Log(options: options, 
            inputToLog: command.ToString(), 
            result);

        if (result.ExitCode != 0 && options.ThrowOnNonZeroExitCode)
        {
            var input = command.ToString();
            throw new CommandException(input, result);
        }

        return result;
    }
}
