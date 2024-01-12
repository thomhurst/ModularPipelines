using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudArtifacts
{
    public GcloudArtifacts(
        GcloudArtifactsApt apt,
        GcloudArtifactsDocker docker,
        GcloudArtifactsFiles files,
        GcloudArtifactsGo go,
        GcloudArtifactsLocations locations,
        GcloudArtifactsOperations operations,
        GcloudArtifactsPackages packages,
        GcloudArtifactsPrintSettings printSettings,
        GcloudArtifactsRepositories repositories,
        GcloudArtifactsSbom sbom,
        GcloudArtifactsSettings settings,
        GcloudArtifactsTags tags,
        GcloudArtifactsVersions versions,
        GcloudArtifactsVpcscConfig vpcscConfig,
        GcloudArtifactsVulnerabilities vulnerabilities,
        GcloudArtifactsYum yum
    )
    {
        Apt = apt;
        Docker = docker;
        Files = files;
        Go = go;
        Locations = locations;
        Operations = operations;
        Packages = packages;
        PrintSettings = printSettings;
        Repositories = repositories;
        Sbom = sbom;
        Settings = settings;
        Tags = tags;
        Versions = versions;
        VpcscConfig = vpcscConfig;
        Vulnerabilities = vulnerabilities;
        Yum = yum;
    }

    public GcloudArtifactsApt Apt { get; }

    public GcloudArtifactsDocker Docker { get; }

    public GcloudArtifactsFiles Files { get; }

    public GcloudArtifactsGo Go { get; }

    public GcloudArtifactsLocations Locations { get; }

    public GcloudArtifactsOperations Operations { get; }

    public GcloudArtifactsPackages Packages { get; }

    public GcloudArtifactsPrintSettings PrintSettings { get; }

    public GcloudArtifactsRepositories Repositories { get; }

    public GcloudArtifactsSbom Sbom { get; }

    public GcloudArtifactsSettings Settings { get; }

    public GcloudArtifactsTags Tags { get; }

    public GcloudArtifactsVersions Versions { get; }

    public GcloudArtifactsVpcscConfig VpcscConfig { get; }

    public GcloudArtifactsVulnerabilities Vulnerabilities { get; }

    public GcloudArtifactsYum Yum { get; }
}