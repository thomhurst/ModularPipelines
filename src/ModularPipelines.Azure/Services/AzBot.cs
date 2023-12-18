using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzBot
{
    public AzBot(
        AzBotAuthsetting authsetting,
        AzBotDirectline directline,
        AzBotEmail email,
        AzBotFacebook facebook,
        AzBotKik kik,
        AzBotMsteams msteams,
        AzBotSkype skype,
        AzBotSlack slack,
        AzBotSms sms,
        AzBotTelegram telegram,
        AzBotWebchat webchat,
        ICommand internalCommand
    )
    {
        Authsetting = authsetting;
        Directline = directline;
        Email = email;
        Facebook = facebook;
        Kik = kik;
        Msteams = msteams;
        Skype = skype;
        Slack = slack;
        Sms = sms;
        Telegram = telegram;
        Webchat = webchat;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBotAuthsetting Authsetting { get; }

    public AzBotDirectline Directline { get; }

    public AzBotEmail Email { get; }

    public AzBotFacebook Facebook { get; }

    public AzBotKik Kik { get; }

    public AzBotMsteams Msteams { get; }

    public AzBotSkype Skype { get; }

    public AzBotSlack Slack { get; }

    public AzBotSms Sms { get; }

    public AzBotTelegram Telegram { get; }

    public AzBotWebchat Webchat { get; }

    public async Task<CommandResult> Create(AzBotCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzBotDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Download(AzBotDownloadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PrepareDeploy(AzBotPrepareDeployOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PreparePublish(AzBotPreparePublishOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Publish(AzBotPublishOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzBotShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzBotUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}