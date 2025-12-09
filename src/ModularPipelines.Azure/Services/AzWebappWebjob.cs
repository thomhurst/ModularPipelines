using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("webapp")]
public class AzWebappWebjob
{
    public AzWebappWebjob(
        AzWebappWebjobContinuous continuous,
        AzWebappWebjobTriggered triggered
    )
    {
        Continuous = continuous;
        Triggered = triggered;
    }

    public AzWebappWebjobContinuous Continuous { get; }

    public AzWebappWebjobTriggered Triggered { get; }
}