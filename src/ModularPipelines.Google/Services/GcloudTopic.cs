using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudTopic
{
    public GcloudTopic(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Accessibility(GcloudTopicAccessibilityOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudTopicAccessibilityOptions(), token);
    }

    public async Task<CommandResult> ArgFiles(GcloudTopicArgFilesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudTopicArgFilesOptions(), token);
    }

    public async Task<CommandResult> CliTrees(GcloudTopicCliTreesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudTopicCliTreesOptions(), token);
    }

    public async Task<CommandResult> ClientCertificate(GcloudTopicClientCertificateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudTopicClientCertificateOptions(), token);
    }

    public async Task<CommandResult> CommandConventions(GcloudTopicCommandConventionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudTopicCommandConventionsOptions(), token);
    }

    public async Task<CommandResult> Configurations(GcloudTopicConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudTopicConfigurationsOptions(), token);
    }

    public async Task<CommandResult> Datetimes(GcloudTopicDatetimesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudTopicDatetimesOptions(), token);
    }

    public async Task<CommandResult> EndpointOverride(GcloudTopicEndpointOverrideOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudTopicEndpointOverrideOptions(), token);
    }

    public async Task<CommandResult> Escaping(GcloudTopicEscapingOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudTopicEscapingOptions(), token);
    }

    public async Task<CommandResult> Filters(GcloudTopicFiltersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudTopicFiltersOptions(), token);
    }

    public async Task<CommandResult> FlagsFile(GcloudTopicFlagsFileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudTopicFlagsFileOptions(), token);
    }

    public async Task<CommandResult> Formats(GcloudTopicFormatsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudTopicFormatsOptions(), token);
    }

    public async Task<CommandResult> Gcloudignore(GcloudTopicGcloudignoreOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudTopicGcloudignoreOptions(), token);
    }

    public async Task<CommandResult> OfflineHelp(GcloudTopicOfflineHelpOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudTopicOfflineHelpOptions(), token);
    }

    public async Task<CommandResult> Projections(GcloudTopicProjectionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudTopicProjectionsOptions(), token);
    }

    public async Task<CommandResult> ResourceKeys(GcloudTopicResourceKeysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudTopicResourceKeysOptions(), token);
    }

    public async Task<CommandResult> Startup(GcloudTopicStartupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudTopicStartupOptions(), token);
    }

    public async Task<CommandResult> Uninstall(GcloudTopicUninstallOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudTopicUninstallOptions(), token);
    }
}