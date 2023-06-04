using CliWrap.Buffered;
using ModularPipelines.Cmd.Models;

namespace ModularPipelines.Cmd;

public interface ICmd<T> : ICmd
{
    
}

public interface ICmd
{
    Task<BufferedCommandResult> Script(CmdScriptOptions options, CancellationToken cancellationToken = default);
}