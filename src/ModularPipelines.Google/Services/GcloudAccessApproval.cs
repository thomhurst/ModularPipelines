using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudAccessApproval
{
    public GcloudAccessApproval(
        GcloudAccessApprovalRequests requests,
        GcloudAccessApprovalServiceAccount serviceAccount,
        GcloudAccessApprovalSettings settings
    )
    {
        Requests = requests;
        ServiceAccount = serviceAccount;
        Settings = settings;
    }

    public GcloudAccessApprovalRequests Requests { get; }

    public GcloudAccessApprovalServiceAccount ServiceAccount { get; }

    public GcloudAccessApprovalSettings Settings { get; }
}