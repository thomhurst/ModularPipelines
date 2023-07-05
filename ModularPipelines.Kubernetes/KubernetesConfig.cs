using ModularPipelines.Models;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Kubernetes.Options;
using ModularPipelines.Options;

namespace ModularPipelines.Kubernetes;

public class KubernetesConfig : IKubernetesConfig
{
    private readonly ICommand _command;

    public KubernetesConfig(ICommand command)
    {
        _command = command;
    }

    public async Task UseContext(KubernetesUseContextOptions kubernetesUseContextOptions)
    {
        await _command.ExecuteCommandLineTool(kubernetesUseContextOptions.ToCommandLineToolOptions("kubernetes",
            "config", "use-context", kubernetesUseContextOptions.ContextName)
        );
    }

    public async Task<CommandResult> View(KubernetesViewConfigOptions viewConfigOptions)
    {
        return await _command.ExecuteCommandLineTool(viewConfigOptions.ToCommandLineToolOptions("kubernetes",
            "config", "view")
        );
    }

    public async Task<CommandResult> GetContexts()
    {
        return await _command.ExecuteCommandLineTool(new CommandLineToolOptions("kubernetes")
        {
            Arguments = new[] { "config", "get-contexts" }
        });
    }

    public async Task<CommandResult> CurrentContext()
    {
        return await _command.ExecuteCommandLineTool(new CommandLineToolOptions("kubernetes")
        {
            Arguments = new[] { "config", "current-context" }
        });
    }
}
