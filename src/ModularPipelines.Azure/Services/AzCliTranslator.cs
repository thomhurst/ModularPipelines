using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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