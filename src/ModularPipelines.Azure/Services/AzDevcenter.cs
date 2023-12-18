using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzDevcenter
{
    public AzDevcenter(
        AzDevcenterAdmin admin,
        AzDevcenterDev dev
    )
    {
        Admin = admin;
        Dev = dev;
    }

    public AzDevcenterAdmin Admin { get; }

    public AzDevcenterDev Dev { get; }
}