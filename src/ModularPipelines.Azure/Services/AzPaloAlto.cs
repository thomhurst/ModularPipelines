using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzPaloAlto
{
    public AzPaloAlto(
        AzPaloAltoCloudngfw cloudngfw
    )
    {
        Cloudngfw = cloudngfw;
    }

    public AzPaloAltoCloudngfw Cloudngfw { get; }
}