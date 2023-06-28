using CliWrap.Buffered;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

public interface ICommand
{
    Task<BufferedCommandResult> UsingCommandLineTool(CommandLineToolOptions options, CancellationToken cancellationToken = default);
}