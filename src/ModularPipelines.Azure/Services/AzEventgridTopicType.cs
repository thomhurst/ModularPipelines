using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid")]
public class AzEventgridTopicType
{
    public AzEventgridTopicType(
        AzEventgridTopicTypeList list,
        AzEventgridTopicTypeListEventTypes listEventTypes,
        AzEventgridTopicTypeShow show,
        ICommand internalCommand
    )
    {
        ListCommands = list;
        ListEventTypesCommands = listEventTypes;
        ShowCommands = show;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventgridTopicTypeList ListCommands { get; }

    public AzEventgridTopicTypeListEventTypes ListEventTypesCommands { get; }

    public AzEventgridTopicTypeShow ShowCommands { get; }

    public async Task<CommandResult> List(AzEventgridTopicTypeListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridTopicTypeListOptions(), token);
    }

    public async Task<CommandResult> ListEventTypes(AzEventgridTopicTypeListEventTypesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzEventgridTopicTypeShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}