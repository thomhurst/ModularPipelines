using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp")]
public class AzWebappAuth
{
    public AzWebappAuth(
        AzWebappAuthApple apple,
        AzWebappAuthConfigVersion configVersion,
        AzWebappAuthFacebook facebook,
        AzWebappAuthGithub github,
        AzWebappAuthGoogle google,
        AzWebappAuthMicrosoft microsoft,
        AzWebappAuthOpenidConnect openidConnect,
        AzWebappAuthShow show,
        AzWebappAuthTwitter twitter,
        AzWebappAuthUpdate update,
        ICommand internalCommand
    )
    {
        Apple = apple;
        ConfigVersion = configVersion;
        Facebook = facebook;
        Github = github;
        Google = google;
        Microsoft = microsoft;
        OpenidConnect = openidConnect;
        ShowCommands = show;
        Twitter = twitter;
        UpdateCommands = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzWebappAuthApple Apple { get; }

    public AzWebappAuthConfigVersion ConfigVersion { get; }

    public AzWebappAuthFacebook Facebook { get; }

    public AzWebappAuthGithub Github { get; }

    public AzWebappAuthGoogle Google { get; }

    public AzWebappAuthMicrosoft Microsoft { get; }

    public AzWebappAuthOpenidConnect OpenidConnect { get; }

    public AzWebappAuthShow ShowCommands { get; }

    public AzWebappAuthTwitter Twitter { get; }

    public AzWebappAuthUpdate UpdateCommands { get; }

    public async Task<CommandResult> Set(AzWebappAuthSetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappAuthSetOptions(), token);
    }

    public async Task<CommandResult> Show(AzWebappAuthShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappAuthShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzWebappAuthUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappAuthUpdateOptions(), token);
    }
}