using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzGraphServices
{
    public AzGraphServices(
        AzGraphServicesAccount account
    )
    {
        Account = account;
    }

    public AzGraphServicesAccount Account { get; }
}