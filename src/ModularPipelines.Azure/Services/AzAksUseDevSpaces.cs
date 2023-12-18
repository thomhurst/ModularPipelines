using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksUseDevSpaces
{
    public AzAksUseDevSpaces(
        AzAksUseDevSpacesDevSpaces devSpaces
    )
    {
        DevSpaces = devSpaces;
    }

    public AzAksUseDevSpacesDevSpaces DevSpaces { get; }
}