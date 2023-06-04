using CliWrap.Buffered;

namespace ModularPipelines.Node;

public interface INode<T> : INode
{
}

public interface INode
{
    Task<BufferedCommandResult> Version(CancellationToken cancellationToken = default);
    public INpm Npm { get; }
    public INvm Nvm { get; }
}