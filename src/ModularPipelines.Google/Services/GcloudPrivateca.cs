using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudPrivateca
{
    public GcloudPrivateca(
        GcloudPrivatecaCertificates certificates,
        GcloudPrivatecaLocations locations,
        GcloudPrivatecaPools pools,
        GcloudPrivatecaRoots roots,
        GcloudPrivatecaSubordinates subordinates,
        GcloudPrivatecaTemplates templates
    )
    {
        Certificates = certificates;
        Locations = locations;
        Pools = pools;
        Roots = roots;
        Subordinates = subordinates;
        Templates = templates;
    }

    public GcloudPrivatecaCertificates Certificates { get; }

    public GcloudPrivatecaLocations Locations { get; }

    public GcloudPrivatecaPools Pools { get; }

    public GcloudPrivatecaRoots Roots { get; }

    public GcloudPrivatecaSubordinates Subordinates { get; }

    public GcloudPrivatecaTemplates Templates { get; }
}