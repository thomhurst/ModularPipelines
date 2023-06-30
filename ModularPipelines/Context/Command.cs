using System.Collections.ObjectModel;
using CliWrap;
using CliWrap.Buffered;
using Microsoft.Extensions.Logging;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

internal class Command : ICommand
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private ILogger Logger => _moduleLoggerProvider.GetLogger();

    public Command(IModuleLoggerProvider moduleLoggerProvider)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
    }
    
    public async Task<BufferedCommandResult> UsingCommandLineTool(CommandLineToolOptions options, CancellationToken cancellationToken = default)
    {
        var parsedArgs = string.Equals(options.Arguments?.ElementAtOrDefault(0), options.Tool) 
            ? options.Arguments?.Skip(1) : options.Arguments;
        
        var command = Cli.Wrap(options.Tool)
            .WithArguments(parsedArgs ?? Array.Empty<string>());
      
        if (options.WorkingDirectory != null)
        {
            command = command.WithWorkingDirectory(options.WorkingDirectory);
        }
        
        if (options.EnvironmentVariables != null)
        {
            command = command.WithEnvironmentVariables(new ReadOnlyDictionary<string, string?>(options.EnvironmentVariables));
        }

        if (options.LogInput)
        {
            var inputManipulator = options.InputLoggingManipulator ?? (s => s);

            Logger.LogInformation("---Executing Command---\r\n{Input}", inputManipulator(command.ToString()));
        }

        var result = await Of(command, cancellationToken);

        if (options.LogOutput)
        {
            var outputManipulator = options.OutputLoggingManipulator ?? (s => s);
            
            Logger.LogInformation("---Command Result---\r\n{Output}",
                string.IsNullOrEmpty(result.StandardError)
                    ? outputManipulator(result.StandardOutput)
                    : outputManipulator(result.StandardError));
        }

        return result;
    }

    public async Task<BufferedCommandResult> Of(CliWrap.Command command, CancellationToken cancellationToken = default)
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