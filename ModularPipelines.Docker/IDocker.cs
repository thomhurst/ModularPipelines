using ModularPipelines.Context;
using ModularPipelines.Docker.Options;
using ModularPipelines.Extensions;

namespace ModularPipelines.Docker;

public interface IDocker
{
    Task Login(DockerLoginOptions dockerLoginOptions);
    Task BuildFromDockerfile(DockerBuildOptions dockerBuildOptions);
    Task Logout(DockerOptions? options = null);
    Task Push(DockerPushOptions dockerPushOptions);
    Task<string> Version(DockerArgumentOptions? dockerArgumentOptions = null);
}

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
            "login",
            $"--username={dockerLoginOptions.Username}",
            $"--password={dockerLoginOptions.Password}"
        };

        arguments.AddNonNullOrEmpty(dockerLoginOptions.Server?.AbsolutePath);
        
        await _command.UsingCommandLineTool(dockerLoginOptions.ToCommandLineToolOptions("docker", arguments));
    }

    public async Task BuildFromDockerfile(DockerBuildOptions dockerBuildOptions)
    {
        var workingDirectory =
            dockerBuildOptions.DockerfileFolder.Path == dockerBuildOptions.WorkingDirectory ? "." : dockerBuildOptions.DockerfileFolder.Path;
        
        var arguments = new List<string>
        {
            "build",
            workingDirectory,
        };

        arguments.AddNonNullOrEmptyArgumentWithSwitch("-t", dockerBuildOptions.Tag);

        arguments.AddRangeNonNullOrEmptyArgumentWithSwitch("--build-arg", dockerBuildOptions.BuildArguments);

        arguments.AddNonNullOrEmpty(dockerBuildOptions.Dockerfile?.Path);
        
        await _command.UsingCommandLineTool(dockerBuildOptions.ToCommandLineToolOptions("docker", arguments));
    }

    public Task Logout(DockerOptions? options = null)
    {
        options ??= new DockerOptions();

        return _command.UsingCommandLineTool(options.ToCommandLineToolOptions("docker", new[] { "logout" }));
    }

    public Task Push(DockerPushOptions dockerPushOptions)
    {
        var arguments = new List<string>
        {
            "push",
            $"--disable-content-trust={dockerPushOptions.DisableContentTrust.ToString().ToLower()}",
            $"{dockerPushOptions.Name}:{dockerPushOptions.Tag}"
        };
        
        return _command.UsingCommandLineTool(dockerPushOptions.ToCommandLineToolOptions("docker", arguments));
    }

    public async Task<string> Version(DockerArgumentOptions? dockerArgumentOptions = null)
    {
        dockerArgumentOptions ??= new DockerArgumentOptions();
        
        var result = await _command.UsingCommandLineTool(dockerArgumentOptions.ToCommandLineToolOptions("docker", new []{ "version" }));
        
        return result.StandardOutput;
    }
}