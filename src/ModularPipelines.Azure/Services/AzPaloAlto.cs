using System.Diagnostics.CodeAnalysis;

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