using ModularPipelines.Command;
using ModularPipelines.Context;

namespace ModularPipelines.Kubernetes;

public abstract class KubectlModule : CommandLineToolModule
{
    protected KubectlModule(IModuleContext context) : base(context)
    {
    }

    protected override string Tool => "kubectl";
}