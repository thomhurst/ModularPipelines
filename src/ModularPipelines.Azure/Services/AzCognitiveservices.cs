using System.Diagnostics.CodeAnalysis;

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