using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare")]
public class GcloudHealthcareFhirStores
{
    public GcloudHealthcareFhirStores(
        GcloudHealthcareFhirStoresExport export,
        GcloudHealthcareFhirStoresImport import,
        ICommand internalCommand
    )
    {
        Export = export;
        Import = import;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudHealthcareFhirStoresExport Export { get; }

    public GcloudHealthcareFhirStoresImport Import { get; }

    public async Task<CommandResult> AddIamPolicyBinding(GcloudHealthcareFhirStoresAddIamPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(GcloudHealthcareFhirStoresCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Deidentify(GcloudHealthcareFhirStoresDeidentifyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudHealthcareFhirStoresDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudHealthcareFhirStoresDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIamPolicy(GcloudHealthcareFhirStoresGetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudHealthcareFhirStoresListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Metrics(GcloudHealthcareFhirStoresMetricsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveIamPolicyBinding(GcloudHealthcareFhirStoresRemoveIamPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetIamPolicy(GcloudHealthcareFhirStoresSetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudHealthcareFhirStoresUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}