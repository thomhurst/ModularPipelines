using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "server-arc")]
public class AzSqlServerArcExtension
{
    public AzSqlServerArcExtension(
        AzSqlServerArcExtensionFeatureFlag featureFlag
    )
    {
        FeatureFlag = featureFlag;
    }

    public AzSqlServerArcExtensionFeatureFlag FeatureFlag { get; }
}