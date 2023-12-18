using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzResourceMover
{
    public AzResourceMover(
        AzResourceMoverMoveCollection moveCollection,
        AzResourceMoverMoveResource moveResource
    )
    {
        MoveCollection = moveCollection;
        MoveResource = moveResource;
    }

    public AzResourceMoverMoveCollection MoveCollection { get; }

    public AzResourceMoverMoveResource MoveResource { get; }
}