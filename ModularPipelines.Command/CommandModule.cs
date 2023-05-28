using CliWrap;
using CliWrap.Buffered;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Command;

public abstract class CommandModule : Module<BufferedCommandResult>
{
    protected CommandModule(IModuleContext context) : base(context)
    {
    }

    protected abstract string Command { get; }
    
    public virtual IReadOnlyDictionary<string, string?>? EnvironmentVariables { get; }
    protected virtual string WorkingDirectory => Context.Environment.WorkingDirectory.FullName;

    protected override async Task<ModuleResult<BufferedCommandResult>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var command = Cli.Wrap(Command)
            .WithWorkingDirectory(WorkingDirectory);

        if (EnvironmentVariables != null)
        {
            command = command.WithEnvironmentVariables(EnvironmentVariables);
        }
        
        var result = await command
            .ExecuteBufferedAsync(cancellationToken: cancellationToken);
        
        return result;
    }
}