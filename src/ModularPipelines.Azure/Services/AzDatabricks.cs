using System.Diagnostics.CodeAnalysis;

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