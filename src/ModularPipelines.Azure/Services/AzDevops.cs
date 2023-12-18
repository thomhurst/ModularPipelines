using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzDevops
{
    public AzDevops(
        AzDevopsAdmin admin,
        AzDevopsExtension extension,
        AzDevopsProject project,
        AzDevopsSecurity security,
        AzDevopsServiceEndpoint serviceEndpoint,
        AzDevopsTeam team,
        AzDevopsUser user,
        AzDevopsWiki wiki,
        ICommand internalCommand
    )
    {
        Admin = admin;
        Extension = extension;
        Project = project;
        Security = security;
        ServiceEndpoint = serviceEndpoint;
        Team = team;
        User = user;
        Wiki = wiki;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDevopsAdmin Admin { get; }

    public AzDevopsExtension Extension { get; }

    public AzDevopsProject Project { get; }

    public AzDevopsSecurity Security { get; }

    public AzDevopsServiceEndpoint ServiceEndpoint { get; }

    public AzDevopsTeam Team { get; }

    public AzDevopsUser User { get; }

    public AzDevopsWiki Wiki { get; }

    public async Task<CommandResult> Configure(AzDevopsConfigureOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevopsConfigureOptions(), token);
    }

    public async Task<CommandResult> Feedback(AzDevopsFeedbackOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevopsFeedbackOptions(), token);
    }

    public async Task<CommandResult> Invoke(AzDevopsInvokeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevopsInvokeOptions(), token);
    }

    public async Task<CommandResult> Login(AzDevopsLoginOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevopsLoginOptions(), token);
    }

    public async Task<CommandResult> Logout(AzDevopsLogoutOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevopsLogoutOptions(), token);
    }
}