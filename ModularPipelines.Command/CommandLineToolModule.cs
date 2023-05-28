using CliWrap;
using CliWrap.Buffered;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Command;

public abstract class CommandLineToolModule : Module<BufferedCommandResult>
{
    protected CommandLineToolModule(IModuleContext context) : base(context)
    {
    }

    protected abstract string Tool { get; }
    protected abstract IEnumerable<string> Arguments { get; }
    
    public virtual IReadOnlyDictionary<string, string?>? EnvironmentVariables { get; }
    protected virtual string WorkingDirectory => Context.Environment.WorkingDirectory.FullName;

    protected override async Task<ModuleResult<BufferedCommandResult>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var command = Cli.Wrap(Tool)
            .WithArguments(ParseArguments())
            .WithWorkingDirectory(WorkingDirectory);
      
        if (EnvironmentVariables != null)
        {
            command = command.WithEnvironmentVariables(EnvironmentVariables);
        }
        
        var result = await command
            .ExecuteBufferedAsync(cancellationToken: cancellationToken);

        return result;
    }

    private IEnumerable<string> ParseArguments()
    {
        return string.Equals(Arguments.ElementAtOrDefault(0), Tool) 
            ? Arguments.Skip(1) : Arguments;
    }
}