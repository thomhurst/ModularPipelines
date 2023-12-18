using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzCognitiveservices
{
    public AzCognitiveservices(
        AzCognitiveservicesAccount account,
        AzCognitiveservicesCommitmentTier commitmentTier,
        AzCognitiveservicesModel model,
        AzCognitiveservicesUsage usage
    )
    {
        Account = account;
        CommitmentTier = commitmentTier;
        Model = model;
        Usage = usage;
    }

    public AzCognitiveservicesAccount Account { get; }

    public AzCognitiveservicesCommitmentTier CommitmentTier { get; }

    public AzCognitiveservicesModel Model { get; }

    public AzCognitiveservicesUsage Usage { get; }
}