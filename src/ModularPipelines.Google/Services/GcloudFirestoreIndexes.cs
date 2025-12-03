using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("firestore")]
public class GcloudFirestoreIndexes
{
    public GcloudFirestoreIndexes(
        GcloudFirestoreIndexesComposite composite,
        GcloudFirestoreIndexesFields fields
    )
    {
        Composite = composite;
        Fields = fields;
    }

    public GcloudFirestoreIndexesComposite Composite { get; }

    public GcloudFirestoreIndexesFields Fields { get; }
}