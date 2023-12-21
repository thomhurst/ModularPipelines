using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp")]
public class AzContainerappAuth
{
    public AzContainerappAuth(
        AzContainerappAuthApple apple,
        AzContainerappAuthFacebook facebook,
        AzContainerappAuthGithub github,
        AzContainerappAuthGoogle google,
        AzContainerappAuthMicrosoft microsoft,
        AzContainerappAuthOpenidConnect openidConnect,
        AzContainerappAuthTwitter twitter,
        ICommand internalCommand
    )
    {
        Apple = apple;
        Facebook = facebook;
        Github = github;
        Google = google;
        Microsoft = microsoft;
        OpenidConnect = openidConnect;
        Twitter = twitter;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzContainerappAuthApple Apple { get; }

    public AzContainerappAuthFacebook Facebook { get; }

    public AzContainerappAuthGithub Github { get; }

    public AzContainerappAuthGoogle Google { get; }

    public AzContainerappAuthMicrosoft Microsoft { get; }

    public AzContainerappAuthOpenidConnect OpenidConnect { get; }

    public AzContainerappAuthTwitter Twitter { get; }

    public async Task<CommandResult> Show(AzContainerappAuthShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappAuthShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzContainerappAuthUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappAuthUpdateOptions(), token);
    }
}