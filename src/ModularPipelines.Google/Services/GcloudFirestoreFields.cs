using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firestore")]
public class GcloudFirestoreFields
{
    public GcloudFirestoreFields(
        GcloudFirestoreFieldsTtls ttls
    )
    {
        Ttls = ttls;
    }

    public GcloudFirestoreFieldsTtls Ttls { get; }
}