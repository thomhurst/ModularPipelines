using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds")]
public class GcloudVmwarePrivateCloudsNsx
{
    public GcloudVmwarePrivateCloudsNsx(
        GcloudVmwarePrivateCloudsNsxCredentials credentials
    )
    {
        Credentials = credentials;
    }

    public GcloudVmwarePrivateCloudsNsxCredentials Credentials { get; }
}