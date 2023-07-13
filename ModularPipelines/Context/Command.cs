using System.Collections.ObjectModel;
using System.Reflection;
using CliWrap;
using CliWrap.Buffered;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
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
                ?.PrecedingArguments ?? Array.Empty<string>();

        var parsedArgs = (string.Equals(options.Arguments?.ElementAtOrDefault(0), options.Tool)
            ? options.Arguments?.Skip(1).ToList() : options.Arguments?.ToList()) ?? new List<string>();

        parsedArgs = precedingArgs.Concat(parsedArgs).ToList();

        CommandOptionsObjectArgumentParser.AddArgumentsFromOptionsObject(parsedArgs, optionsObject);

        if (options.RunSettings != null)
        {
            parsedArgs.Add("--");
            parsedArgs.AddRange(options.RunSettings);
        }

        var command = Cli.Wrap(options.Tool).WithArguments(parsedArgs);

        if (options.WorkingDirectory != null)
        {
            command = command.WithWorkingDirectory(options.WorkingDirectory);
        }

        if (options.EnvironmentVariables != null)
        {
            command = command.WithEnvironmentVariables(new ReadOnlyDictionary<string, string?>(options.EnvironmentVariables));
        }

        var commandInput = _secretObfuscator.Obfuscate(command.ToString(), optionsObject);

        if (options.LogInput)
        {
            var inputLoggingManipulator = options.InputLoggingManipulator ?? (s => s);

            Logger.LogInformation("---Executing Command---\r\n\t{Input}", inputLoggingManipulator(commandInput));
        }

        var result = await Of(command, cancellationToken);

        if (options.LogOutput)
        {
            var outputLoggingManipulator = options.OutputLoggingManipulator ?? (s => s);

            Logger.LogInformation("---Command Result---\r\n\t{Output}",
                string.IsNullOrEmpty(result.StandardError)
                    ? outputLoggingManipulator(_secretObfuscator.Obfuscate(result.StandardOutput, optionsObject))
                    : outputLoggingManipulator(_secretObfuscator.Obfuscate(result.StandardError, optionsObject)));
        }

        return new CommandResult(commandInput, result);
    }

    private static object GetOptionsObject(CommandLineToolOptions options)
    {
        return options.OptionsObject ?? options;
    }

    private static async Task<BufferedCommandResult> Of(CliWrap.Command command, CancellationToken cancellationToken = default)
    {
        var result = await command
            .WithValidation(CommandResultValidation.None)
            .ExecuteBufferedAsync(cancellationToken);

        if (result.ExitCode != 0)
        {
            var input = command.ToString();
            throw new CommandException(input, result);
        }

        return result;
    }
}
