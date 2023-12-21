using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "computetarget")]
public class AzMlComputetargetAmlcompute
{
    public AzMlComputetargetAmlcompute(
        AzMlComputetargetAmlcomputeIdentity identity
    )
    {
        Identity = identity;
    }

    public AzMlComputetargetAmlcomputeIdentity Identity { get; }
}