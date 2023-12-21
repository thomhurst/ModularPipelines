using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp")]
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