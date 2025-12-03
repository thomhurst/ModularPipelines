using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("aks")]
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