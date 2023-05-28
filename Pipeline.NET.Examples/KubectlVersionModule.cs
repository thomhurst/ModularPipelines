using Pipeline.NET.Context;
using Pipeline.NET.Kubernetes;

public class KubectlVersionModule : KubectlModule
{
    public KubectlVersionModule(IModuleContext context) : base(context)
    {
    }

    protected override string Command => "version";
}