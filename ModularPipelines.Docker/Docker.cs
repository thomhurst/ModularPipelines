using ModularPipelines.Context;
using ModularPipelines.Docker.Options;
using ModularPipelines.Extensions;

namespace ModularPipelines.Docker;

public class Docker : IDocker
{
    private readonly ICommand _command;

    public Docker(ICommand command)
    {
        _command = command;
    }

    public async Task Login(DockerLoginOptions dockerLoginOptions)
    {
        var arguments = new List<string>
        {
            "login"
        };

        arguments.AddNonNullOrEmpty(dockerLoginOptions.Server?.AbsolutePath);

        await _command.ExecuteCommandLineTool(dockerLoginOptions.WithArguments(arguments));
    }

    public async Task BuildFromDockerfile(DockerBuildOptions dockerBuildOptions)
    {
        var workingDirectory =
            dockerBuildOptions.DockerfileFolder == dockerBuildOptions.WorkingDirectory ? "." : dockerBuildOptions.DockerfileFolder;

        var arguments = new List<string>
        {
            "build",
            workingDirectory,
        };

        await _command.ExecuteCommandLineTool(dockerBuildOptions.WithArguments(arguments));
    }

    public Task Logout(DockerOptions? options = null)
    {
        options ??= new DockerOptions();

        return _command.ExecuteCommandLineTool(options.WithArguments("logout"));
    }

    public Task Push(DockerPushOptions dockerPushOptions)
    {
        var arguments = new List<string>
        {
            "push",
            $"{dockerPushOptions.Name}:{dockerPushOptions.Tag}"
        };

        return _command.ExecuteCommandLineTool(dockerPushOptions.WithArguments(arguments));
    }

    public async Task<string> Version(DockerArgumentOptions? dockerArgumentOptions = null)
    {
        dockerArgumentOptions ??= new DockerArgumentOptions();

        var result = await _command.ExecuteCommandLineTool(dockerArgumentOptions.WithArguments("version"));

        return result.StandardOutput;
    }
}
