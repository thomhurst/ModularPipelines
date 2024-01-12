using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudFirebase
{
    public GcloudFirebase(
        GcloudFirebaseTest test
    )
    {
        Test = test;
    }

    public GcloudFirebaseTest Test { get; }
}