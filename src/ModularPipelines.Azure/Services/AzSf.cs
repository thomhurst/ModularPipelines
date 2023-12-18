using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzSf
{
    public AzSf(
        AzSfApplication application,
        AzSfApplicationType applicationType,
        AzSfCluster cluster,
        AzSfManagedApplication managedApplication,
        AzSfManagedApplicationType managedApplicationType,
        AzSfManagedCluster managedCluster,
        AzSfManagedNodeType managedNodeType,
        AzSfManagedService managedService,
        AzSfService service
    )
    {
        Application = application;
        ApplicationType = applicationType;
        Cluster = cluster;
        ManagedApplication = managedApplication;
        ManagedApplicationType = managedApplicationType;
        ManagedCluster = managedCluster;
        ManagedNodeType = managedNodeType;
        ManagedService = managedService;
        Service = service;
    }

    public AzSfApplication Application { get; }

    public AzSfApplicationType ApplicationType { get; }

    public AzSfCluster Cluster { get; }

    public AzSfManagedApplication ManagedApplication { get; }

    public AzSfManagedApplicationType ManagedApplicationType { get; }

    public AzSfManagedCluster ManagedCluster { get; }

    public AzSfManagedNodeType ManagedNodeType { get; }

    public AzSfManagedService ManagedService { get; }

    public AzSfService Service { get; }
}