using System.Diagnostics.CodeAnalysis;

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