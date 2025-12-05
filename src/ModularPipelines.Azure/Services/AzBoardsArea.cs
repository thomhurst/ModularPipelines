using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("boards")]
public class AzBoardsArea
{
    public AzBoardsArea(
        AzBoardsAreaProject project,
        AzBoardsAreaTeam team
    )
    {
        Project = project;
        Team = team;
    }

    public AzBoardsAreaProject Project { get; }

    public AzBoardsAreaTeam Team { get; }
}