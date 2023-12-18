using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzScvmm
{
    public AzScvmm(
        AzScvmmAvset avset,
        AzScvmmCloud cloud,
        AzScvmmVirtualNetwork virtualNetwork,
        AzScvmmVm vm,
        AzScvmmVmTemplate vmTemplate,
        AzScvmmVmmserver vmmserver
    )
    {
        Avset = avset;
        Cloud = cloud;
        VirtualNetwork = virtualNetwork;
        Vm = vm;
        VmTemplate = vmTemplate;
        Vmmserver = vmmserver;
    }

    public AzScvmmAvset Avset { get; }

    public AzScvmmCloud Cloud { get; }

    public AzScvmmVirtualNetwork VirtualNetwork { get; }

    public AzScvmmVm Vm { get; }

    public AzScvmmVmTemplate VmTemplate { get; }

    public AzScvmmVmmserver Vmmserver { get; }
}