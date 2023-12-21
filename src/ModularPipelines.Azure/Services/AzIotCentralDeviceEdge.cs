using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "device")]
public class AzIotCentralDeviceEdge
{
    public AzIotCentralDeviceEdge(
        AzIotCentralDeviceEdgeChildren children,
        AzIotCentralDeviceEdgeManifest manifest,
        AzIotCentralDeviceEdgeModule module
    )
    {
        Children = children;
        Manifest = manifest;
        Module = module;
    }

    public AzIotCentralDeviceEdgeChildren Children { get; }

    public AzIotCentralDeviceEdgeManifest Manifest { get; }

    public AzIotCentralDeviceEdgeModule Module { get; }
}