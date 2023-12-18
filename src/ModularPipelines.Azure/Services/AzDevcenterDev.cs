using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter")]
public class AzDevcenterDev
{
    public AzDevcenterDev(
        AzDevcenterDevCatalog catalog,
        AzDevcenterDevDevBox devBox,
        AzDevcenterDevEnvironment environment,
        AzDevcenterDevEnvironmentDefinition environmentDefinition,
        AzDevcenterDevEnvironmentType environmentType,
        AzDevcenterDevPool pool,
        AzDevcenterDevProject project,
        AzDevcenterDevSchedule schedule
    )
    {
        Catalog = catalog;
        DevBox = devBox;
        Environment = environment;
        EnvironmentDefinition = environmentDefinition;
        EnvironmentType = environmentType;
        Pool = pool;
        Project = project;
        Schedule = schedule;
    }

    public AzDevcenterDevCatalog Catalog { get; }

    public AzDevcenterDevDevBox DevBox { get; }

    public AzDevcenterDevEnvironment Environment { get; }

    public AzDevcenterDevEnvironmentDefinition EnvironmentDefinition { get; }

    public AzDevcenterDevEnvironmentType EnvironmentType { get; }

    public AzDevcenterDevPool Pool { get; }

    public AzDevcenterDevProject Project { get; }

    public AzDevcenterDevSchedule Schedule { get; }
}