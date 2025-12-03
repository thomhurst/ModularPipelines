using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("firebase")]
public class GcloudFirebaseTest
{
    public GcloudFirebaseTest(
        GcloudFirebaseTestAndroid android,
        GcloudFirebaseTestIos ios,
        GcloudFirebaseTestNetworkProfiles networkProfiles
    )
    {
        Android = android;
        Ios = ios;
        NetworkProfiles = networkProfiles;
    }

    public GcloudFirebaseTestAndroid Android { get; }

    public GcloudFirebaseTestIos Ios { get; }

    public GcloudFirebaseTestNetworkProfiles NetworkProfiles { get; }
}