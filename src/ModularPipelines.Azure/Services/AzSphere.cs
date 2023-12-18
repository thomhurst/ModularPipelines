using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzSphere
{
    public AzSphere(
        AzSphereCaCertificate caCertificate,
        AzSphereCatalog catalog,
        AzSphereDeployment deployment,
        AzSphereDevice device,
        AzSphereDeviceGroup deviceGroup,
        AzSphereImage image,
        AzSphereImagePackage imagePackage,
        AzSphereProduct product,
        ICommand internalCommand
    )
    {
        CaCertificate = caCertificate;
        Catalog = catalog;
        Deployment = deployment;
        Device = device;
        DeviceGroup = deviceGroup;
        Image = image;
        ImagePackage = imagePackage;
        Product = product;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSphereCaCertificate CaCertificate { get; }

    public AzSphereCatalog Catalog { get; }

    public AzSphereDeployment Deployment { get; }

    public AzSphereDevice Device { get; }

    public AzSphereDeviceGroup DeviceGroup { get; }

    public AzSphereImage Image { get; }

    public AzSphereImagePackage ImagePackage { get; }

    public AzSphereProduct Product { get; }

    public async Task<CommandResult> GetSupportData(AzSphereGetSupportDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

