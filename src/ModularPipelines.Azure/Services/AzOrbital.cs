using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzOrbital
{
    public AzOrbital(
        AzOrbitalAvailableGroundStation availableGroundStation,
        AzOrbitalContactProfile contactProfile,
        AzOrbitalOperationResult operationResult,
        AzOrbitalSpacecraft spacecraft
    )
    {
        AvailableGroundStation = availableGroundStation;
        ContactProfile = contactProfile;
        OperationResult = operationResult;
        Spacecraft = spacecraft;
    }

    public AzOrbitalAvailableGroundStation AvailableGroundStation { get; }

    public AzOrbitalContactProfile ContactProfile { get; }

    public AzOrbitalOperationResult OperationResult { get; }

    public AzOrbitalSpacecraft Spacecraft { get; }
}