using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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