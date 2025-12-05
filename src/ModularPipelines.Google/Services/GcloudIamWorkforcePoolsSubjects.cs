using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workforce-pools")]
public class GcloudIamWorkforcePoolsSubjects
{
    public GcloudIamWorkforcePoolsSubjects(
        GcloudIamWorkforcePoolsSubjectsOperations operations,
        ICommand internalCommand
    )
    {
        Operations = operations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudIamWorkforcePoolsSubjectsOperations Operations { get; }

    public async Task<CommandResult> Delete(GcloudIamWorkforcePoolsSubjectsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Undelete(GcloudIamWorkforcePoolsSubjectsUndeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}