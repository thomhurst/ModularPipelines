using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventarc")]
public class GcloudEventarcAuditLogsProvider
{
    public GcloudEventarcAuditLogsProvider(
        GcloudEventarcAuditLogsProviderMethodNames methodNames,
        GcloudEventarcAuditLogsProviderServiceNames serviceNames
    )
    {
        MethodNames = methodNames;
        ServiceNames = serviceNames;
    }

    public GcloudEventarcAuditLogsProviderMethodNames MethodNames { get; }

    public GcloudEventarcAuditLogsProviderServiceNames ServiceNames { get; }
}