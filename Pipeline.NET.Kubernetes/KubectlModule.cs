using Pipeline.NET.Command;
using Pipeline.NET.Context;

namespace Pipeline.NET.Kubernetes;

public abstract class KubectlModule : CommandLineToolModule
{
    protected KubectlModule(IModuleContext context) : base(context)
    {
    }

    protected override string Tool => "kubectl";
}