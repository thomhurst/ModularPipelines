using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

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