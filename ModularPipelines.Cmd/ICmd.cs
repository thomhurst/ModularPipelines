using CliWrap.Buffered;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ModularPipelines.Cmd.Models;

namespace ModularPipelines.Cmd;

public interface ICmd<T> : ICmd
{
    
}

public interface ICmd
{
    Task<BufferedCommandResult> Script(CmdScriptOptions options, CancellationToken cancellationToken = default);
}