using System.Collections.ObjectModel;
using CliWrap;
using CliWrap.Buffered;
using ModularPipelines.Command.Options;

namespace ModularPipelines.Command;

public class Command : ICommand
{
    public async Task<BufferedCommandResult> Of(CommandOptions options, CancellationToken cancellationToken = default)
    {
        var command = Cli.Wrap(options.Command);

        if (options.WorkingDirectory != null)
        {
            command = command.WithWorkingDirectory(options.WorkingDirectory);
        }

        if (options.EnvironmentVariables != null)
        {
            command = command.WithEnvironmentVariables(options.EnvironmentVariables);
        }
        
        var result = await command
            .ExecuteBufferedAsync(cancellationToken: cancellationToken);
        
        return result;
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
        
        var result = await command
            .ExecuteBufferedAsync(cancellationToken: cancellationToken);

        return result;
    }
}