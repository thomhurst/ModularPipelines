using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzDatabricks
{
    public AzDatabricks(
        AzDatabricksAccessConnector accessConnector,
        AzDatabricksWorkspace workspace
    )
    {
        AccessConnector = accessConnector;
        Workspace = workspace;
    }

    public AzDatabricksAccessConnector AccessConnector { get; }

    public AzDatabricksWorkspace Workspace { get; }
}