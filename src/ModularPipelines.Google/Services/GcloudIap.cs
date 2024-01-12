using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudIap
{
    public GcloudIap(
        GcloudIapOauthBrands oauthBrands,
        GcloudIapOauthClients oauthClients,
        GcloudIapSettings settings,
        GcloudIapTcp tcp,
        GcloudIapWeb web
    )
    {
        OauthBrands = oauthBrands;
        OauthClients = oauthClients;
        Settings = settings;
        Tcp = tcp;
        Web = web;
    }

    public GcloudIapOauthBrands OauthBrands { get; }

    public GcloudIapOauthClients OauthClients { get; }

    public GcloudIapSettings Settings { get; }

    public GcloudIapTcp Tcp { get; }

    public GcloudIapWeb Web { get; }
}