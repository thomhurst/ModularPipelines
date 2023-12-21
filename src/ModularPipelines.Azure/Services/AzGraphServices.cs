using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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