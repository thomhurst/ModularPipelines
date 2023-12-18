using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzAzurestackhci
{
    public AzAzurestackhci(
        AzAzurestackhciGalleryimage galleryimage,
        AzAzurestackhciImage image,
        AzAzurestackhciNetworkinterface networkinterface,
        AzAzurestackhciStoragepath storagepath,
        AzAzurestackhciVirtualharddisk virtualharddisk,
        AzAzurestackhciVirtualmachine virtualmachine,
        AzAzurestackhciVirtualnetwork virtualnetwork
    )
    {
        Galleryimage = galleryimage;
        Image = image;
        Networkinterface = networkinterface;
        Storagepath = storagepath;
        Virtualharddisk = virtualharddisk;
        Virtualmachine = virtualmachine;
        Virtualnetwork = virtualnetwork;
    }

    public AzAzurestackhciGalleryimage Galleryimage { get; }

    public AzAzurestackhciImage Image { get; }

    public AzAzurestackhciNetworkinterface Networkinterface { get; }

    public AzAzurestackhciStoragepath Storagepath { get; }

    public AzAzurestackhciVirtualharddisk Virtualharddisk { get; }

    public AzAzurestackhciVirtualmachine Virtualmachine { get; }

    public AzAzurestackhciVirtualnetwork Virtualnetwork { get; }
}