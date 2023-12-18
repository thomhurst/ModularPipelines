using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

