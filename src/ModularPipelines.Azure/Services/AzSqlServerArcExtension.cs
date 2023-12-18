using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

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