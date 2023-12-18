using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse")]
public class AzSynapseSpark
{
    public AzSynapseSpark(
        AzSynapseSparkJob job,
        AzSynapseSparkPool pool,
        AzSynapseSparkSession session,
        AzSynapseSparkStatement statement
    )
    {
        Job = job;
        Pool = pool;
        Session = session;
        Statement = statement;
    }

    public AzSynapseSparkJob Job { get; }

    public AzSynapseSparkPool Pool { get; }

    public AzSynapseSparkSession Session { get; }

    public AzSynapseSparkStatement Statement { get; }
}