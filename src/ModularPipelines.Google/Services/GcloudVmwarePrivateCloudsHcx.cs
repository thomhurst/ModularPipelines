using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds")]
public class GcloudVmwarePrivateCloudsHcx
{
    public GcloudVmwarePrivateCloudsHcx(
        GcloudVmwarePrivateCloudsHcxActivationkeys activationkeys
    )
    {
        Activationkeys = activationkeys;
    }

    public GcloudVmwarePrivateCloudsHcxActivationkeys Activationkeys { get; }
}