using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Kubernetes.Options;

namespace ModularPipelines.Kubernetes;

internal class Kubernetes : IKubernetes
{
    private readonly ICommand _command;

    public Kubernetes( IKubernetesConfig config,
        ICommand command )
    {
        _command = command;
        Config = config;
    }

    public IKubernetesConfig Config { get; }

    public Task Apply( KubernetesApplyOptions applyOptions, CancellationToken cancellationToken = default )
    {
        return _command.ExecuteCommandLineTool( applyOptions.ToCommandLineToolOptions( "kubectl", "apply" ), cancellationToken );
    }
}
