using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudActiveDirectory
{
    public GcloudActiveDirectory(
        GcloudActiveDirectoryDomains domains,
        GcloudActiveDirectoryOperations operations,
        GcloudActiveDirectoryPeerings peerings
    )
    {
        Domains = domains;
        Operations = operations;
        Peerings = peerings;
    }

    public GcloudActiveDirectoryDomains Domains { get; }

    public GcloudActiveDirectoryOperations Operations { get; }

    public GcloudActiveDirectoryPeerings Peerings { get; }
}