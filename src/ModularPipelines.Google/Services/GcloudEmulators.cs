using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudEmulators
{
    public GcloudEmulators(
        GcloudEmulatorsFirestore firestore,
        GcloudEmulatorsSpanner spanner
    )
    {
        Firestore = firestore;
        Spanner = spanner;
    }

    public GcloudEmulatorsFirestore Firestore { get; }

    public GcloudEmulatorsSpanner Spanner { get; }
}