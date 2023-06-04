using CliWrap.Buffered;
using ModularPipelines.Command.Options;

namespace ModularPipelines.Command;

public interface ICommand
{
    Task<BufferedCommandResult> UsingCommandLineTool(CommandLineToolOptions options, CancellationToken cancellationToken = default);
}

public interface ICommand<T> : ICommand
{
}