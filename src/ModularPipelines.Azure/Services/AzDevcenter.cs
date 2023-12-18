using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

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