using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("boards")]
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