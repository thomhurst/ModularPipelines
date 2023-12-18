using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzCliTranslator
{
    public AzCliTranslator(
        AzCliTranslatorArm arm
    )
    {
        Arm = arm;
    }

    public AzCliTranslatorArm Arm { get; }
}