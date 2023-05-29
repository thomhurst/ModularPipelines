using CliWrap.Buffered;
using ModularPipelines.Command.Options;

namespace ModularPipelines.Command;

public interface ICommand
{
    Task<BufferedCommandResult> Of(CommandOptions options, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> UsingCommandLineTool(CommandLineToolOptions options, CancellationToken cancellationToken = default);
}