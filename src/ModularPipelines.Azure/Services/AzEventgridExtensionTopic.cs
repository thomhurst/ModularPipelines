using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid")]
public class AzEventgridExtensionTopic
{
    public AzEventgridExtensionTopic(
        AzEventgridExtensionTopicShow show,
        ICommand internalCommand
    )
    {
        ShowCommands = show;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventgridExtensionTopicShow ShowCommands { get; }

    public async Task<CommandResult> Show(AzEventgridExtensionTopicShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}