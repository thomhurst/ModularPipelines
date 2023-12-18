using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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