using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzAutomanage
{
    public AzAutomanage(
        AzAutomanageBestPractice bestPractice,
        AzAutomanageConfigurationProfile configurationProfile,
        AzAutomanageConfigurationProfileAssignment configurationProfileAssignment,
        AzAutomanageServicePrincipal servicePrincipal
    )
    {
        BestPractice = bestPractice;
        ConfigurationProfile = configurationProfile;
        ConfigurationProfileAssignment = configurationProfileAssignment;
        ServicePrincipal = servicePrincipal;
    }

    public AzAutomanageBestPractice BestPractice { get; }

    public AzAutomanageConfigurationProfile ConfigurationProfile { get; }

    public AzAutomanageConfigurationProfileAssignment ConfigurationProfileAssignment { get; }

    public AzAutomanageServicePrincipal ServicePrincipal { get; }
}