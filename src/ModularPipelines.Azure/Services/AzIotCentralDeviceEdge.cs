using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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