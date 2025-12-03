using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation")]
public class AwsCloudformationWait
{
    public AwsCloudformationWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> ChangeSetCreateComplete(AwsCloudformationWaitChangeSetCreateCompleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StackCreateComplete(AwsCloudformationWaitStackCreateCompleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationWaitStackCreateCompleteOptions(), token);
    }

    public async Task<CommandResult> StackDeleteComplete(AwsCloudformationWaitStackDeleteCompleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationWaitStackDeleteCompleteOptions(), token);
    }

    public async Task<CommandResult> StackExists(AwsCloudformationWaitStackExistsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationWaitStackExistsOptions(), token);
    }

    public async Task<CommandResult> StackImportComplete(AwsCloudformationWaitStackImportCompleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationWaitStackImportCompleteOptions(), token);
    }

    public async Task<CommandResult> StackRollbackComplete(AwsCloudformationWaitStackRollbackCompleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationWaitStackRollbackCompleteOptions(), token);
    }

    public async Task<CommandResult> StackUpdateComplete(AwsCloudformationWaitStackUpdateCompleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationWaitStackUpdateCompleteOptions(), token);
    }

    public async Task<CommandResult> TypeRegistrationComplete(AwsCloudformationWaitTypeRegistrationCompleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}