using System.Collections.ObjectModel;
using CliWrap;
using CliWrap.Buffered;
using Microsoft.Extensions.Logging;
using ModularPipelines.Exceptions;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

internal class Command<T> : ICommand<T>
{
    private readonly ModuleLogger<T> _logger;

    public Command(ModuleLogger<T> logger)
    {
        _logger = logger;
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
            _logger.LogInformation("Executing Command: {Input}", command.ToString());
        }

        var result = await Of(command, cancellationToken);

        if (options.LogOutput)
        {
            _logger.LogInformation("Command Result: {Output}",
                string.IsNullOrEmpty(result.StandardError)
                    ? result.StandardOutput
                    : result.StandardError);
        }

        return result;
    }

    public async Task<BufferedCommandResult> Of(Command command, CancellationToken cancellationToken = default)
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