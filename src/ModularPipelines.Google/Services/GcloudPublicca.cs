using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudPublicca
{
    public GcloudPublicca(
        GcloudPubliccaExternalAccountKeys externalAccountKeys
    )
    {
        ExternalAccountKeys = externalAccountKeys;
    }

    public GcloudPubliccaExternalAccountKeys ExternalAccountKeys { get; }
}