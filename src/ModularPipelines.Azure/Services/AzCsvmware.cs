using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzCsvmware
{
    public AzCsvmware(
        AzCsvmwarePrivateCloud privateCloud,
        AzCsvmwareResourcePool resourcePool,
        AzCsvmwareVirtualNetwork virtualNetwork,
        AzCsvmwareVm vm,
        AzCsvmwareVmTemplate vmTemplate
    )
    {
        PrivateCloud = privateCloud;
        ResourcePool = resourcePool;
        VirtualNetwork = virtualNetwork;
        Vm = vm;
        VmTemplate = vmTemplate;
    }

    public AzCsvmwarePrivateCloud PrivateCloud { get; }

    public AzCsvmwareResourcePool ResourcePool { get; }

    public AzCsvmwareVirtualNetwork VirtualNetwork { get; }

    public AzCsvmwareVm Vm { get; }

    public AzCsvmwareVmTemplate VmTemplate { get; }
}