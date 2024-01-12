using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudNetworkManagement
{
    public GcloudNetworkManagement(
        GcloudNetworkManagementConnectivityTests connectivityTests,
        GcloudNetworkManagementOperations operations
    )
    {
        ConnectivityTests = connectivityTests;
        Operations = operations;
    }

    public GcloudNetworkManagementConnectivityTests ConnectivityTests { get; }

    public GcloudNetworkManagementOperations Operations { get; }
}