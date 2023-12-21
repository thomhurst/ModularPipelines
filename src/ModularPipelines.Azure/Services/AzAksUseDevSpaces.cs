using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

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