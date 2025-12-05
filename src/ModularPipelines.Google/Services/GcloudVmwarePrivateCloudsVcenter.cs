using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds")]
public class GcloudVmwarePrivateCloudsVcenter
{
    public GcloudVmwarePrivateCloudsVcenter(
        GcloudVmwarePrivateCloudsVcenterCredentials credentials
    )
    {
        Credentials = credentials;
    }

    public GcloudVmwarePrivateCloudsVcenterCredentials Credentials { get; }
}