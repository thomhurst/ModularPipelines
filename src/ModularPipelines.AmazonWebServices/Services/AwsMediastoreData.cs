using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsMediastoreData
{
    public AwsMediastoreData(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> DeleteObject(AwsMediastoreDataDeleteObjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeObject(AwsMediastoreDataDescribeObjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetObject(AwsMediastoreDataGetObjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListItems(AwsMediastoreDataListItemsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMediastoreDataListItemsOptions(), token);
    }

    public async Task<CommandResult> PutObject(AwsMediastoreDataPutObjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}