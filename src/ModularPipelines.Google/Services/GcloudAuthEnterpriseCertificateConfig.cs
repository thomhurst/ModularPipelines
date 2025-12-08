using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("auth")]
public class GcloudAuthEnterpriseCertificateConfig
{
    public GcloudAuthEnterpriseCertificateConfig(
        GcloudAuthEnterpriseCertificateConfigCreate create
    )
    {
        Create = create;
    }

    public GcloudAuthEnterpriseCertificateConfigCreate Create { get; }
}