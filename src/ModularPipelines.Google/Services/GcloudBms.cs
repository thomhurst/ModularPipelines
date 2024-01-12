using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudBms
{
    public GcloudBms(
        GcloudBmsInstances instances,
        GcloudBmsNetworks networks,
        GcloudBmsNfsShares nfsShares,
        GcloudBmsOperations operations,
        GcloudBmsOsImages osImages,
        GcloudBmsSshKeys sshKeys,
        GcloudBmsVolumes volumes
    )
    {
        Instances = instances;
        Networks = networks;
        NfsShares = nfsShares;
        Operations = operations;
        OsImages = osImages;
        SshKeys = sshKeys;
        Volumes = volumes;
    }

    public GcloudBmsInstances Instances { get; }

    public GcloudBmsNetworks Networks { get; }

    public GcloudBmsNfsShares NfsShares { get; }

    public GcloudBmsOperations Operations { get; }

    public GcloudBmsOsImages OsImages { get; }

    public GcloudBmsSshKeys SshKeys { get; }

    public GcloudBmsVolumes Volumes { get; }
}