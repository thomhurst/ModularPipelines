using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("devcenter")]
public class AzDevcenterAdmin
{
    public AzDevcenterAdmin(
        AzDevcenterAdminAttachedNetwork attachedNetwork,
        AzDevcenterAdminCatalog catalog,
        AzDevcenterAdminCheckNameAvailability checkNameAvailability,
        AzDevcenterAdminDevboxDefinition devboxDefinition,
        AzDevcenterAdminDevcenter devcenter,
        AzDevcenterAdminEnvironmentDefinition environmentDefinition,
        AzDevcenterAdminEnvironmentType environmentType,
        AzDevcenterAdminGallery gallery,
        AzDevcenterAdminImage image,
        AzDevcenterAdminImageVersion imageVersion,
        AzDevcenterAdminNetworkConnection networkConnection,
        AzDevcenterAdminPool pool,
        AzDevcenterAdminProject project,
        AzDevcenterAdminProjectAllowedEnvironmentType projectAllowedEnvironmentType,
        AzDevcenterAdminProjectEnvironmentType projectEnvironmentType,
        AzDevcenterAdminSchedule schedule,
        AzDevcenterAdminSku sku,
        AzDevcenterAdminUsage usage
    )
    {
        AttachedNetwork = attachedNetwork;
        Catalog = catalog;
        CheckNameAvailability = checkNameAvailability;
        DevboxDefinition = devboxDefinition;
        Devcenter = devcenter;
        EnvironmentDefinition = environmentDefinition;
        EnvironmentType = environmentType;
        Gallery = gallery;
        Image = image;
        ImageVersion = imageVersion;
        NetworkConnection = networkConnection;
        Pool = pool;
        Project = project;
        ProjectAllowedEnvironmentType = projectAllowedEnvironmentType;
        ProjectEnvironmentType = projectEnvironmentType;
        Schedule = schedule;
        Sku = sku;
        Usage = usage;
    }

    public AzDevcenterAdminAttachedNetwork AttachedNetwork { get; }

    public AzDevcenterAdminCatalog Catalog { get; }

    public AzDevcenterAdminCheckNameAvailability CheckNameAvailability { get; }

    public AzDevcenterAdminDevboxDefinition DevboxDefinition { get; }

    public AzDevcenterAdminDevcenter Devcenter { get; }

    public AzDevcenterAdminEnvironmentDefinition EnvironmentDefinition { get; }

    public AzDevcenterAdminEnvironmentType EnvironmentType { get; }

    public AzDevcenterAdminGallery Gallery { get; }

    public AzDevcenterAdminImage Image { get; }

    public AzDevcenterAdminImageVersion ImageVersion { get; }

    public AzDevcenterAdminNetworkConnection NetworkConnection { get; }

    public AzDevcenterAdminPool Pool { get; }

    public AzDevcenterAdminProject Project { get; }

    public AzDevcenterAdminProjectAllowedEnvironmentType ProjectAllowedEnvironmentType { get; }

    public AzDevcenterAdminProjectEnvironmentType ProjectEnvironmentType { get; }

    public AzDevcenterAdminSchedule Schedule { get; }

    public AzDevcenterAdminSku Sku { get; }

    public AzDevcenterAdminUsage Usage { get; }
}