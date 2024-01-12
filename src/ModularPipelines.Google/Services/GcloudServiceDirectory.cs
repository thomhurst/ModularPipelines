using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudServiceDirectory
{
    public GcloudServiceDirectory(
        GcloudServiceDirectoryEndpoints endpoints,
        GcloudServiceDirectoryLocations locations,
        GcloudServiceDirectoryNamespaces namespaces,
        GcloudServiceDirectoryServices services
    )
    {
        Endpoints = endpoints;
        Locations = locations;
        Namespaces = namespaces;
        Services = services;
    }

    public GcloudServiceDirectoryEndpoints Endpoints { get; }

    public GcloudServiceDirectoryLocations Locations { get; }

    public GcloudServiceDirectoryNamespaces Namespaces { get; }

    public GcloudServiceDirectoryServices Services { get; }
}