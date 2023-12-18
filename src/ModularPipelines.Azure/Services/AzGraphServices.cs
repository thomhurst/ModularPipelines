using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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