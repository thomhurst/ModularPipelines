using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudScc
{
    public GcloudScc(
        GcloudSccAssets assets,
        GcloudSccBqexports bqexports,
        GcloudSccCustomModules customModules,
        GcloudSccFindings findings,
        GcloudSccMuteconfigs muteconfigs,
        GcloudSccNotifications notifications,
        GcloudSccOperations operations,
        GcloudSccSources sources
    )
    {
        Assets = assets;
        Bqexports = bqexports;
        CustomModules = customModules;
        Findings = findings;
        Muteconfigs = muteconfigs;
        Notifications = notifications;
        Operations = operations;
        Sources = sources;
    }

    public GcloudSccAssets Assets { get; }

    public GcloudSccBqexports Bqexports { get; }

    public GcloudSccCustomModules CustomModules { get; }

    public GcloudSccFindings Findings { get; }

    public GcloudSccMuteconfigs Muteconfigs { get; }

    public GcloudSccNotifications Notifications { get; }

    public GcloudSccOperations Operations { get; }

    public GcloudSccSources Sources { get; }
}