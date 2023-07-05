using ModularPipelines.Docker.Options;

namespace ModularPipelines.Docker;

public interface IDocker
{
    Task Login( DockerLoginOptions dockerLoginOptions );
    Task BuildFromDockerfile( DockerBuildOptions dockerBuildOptions );
    Task Logout( DockerOptions? options = null );
    Task Push( DockerPushOptions dockerPushOptions );
    Task<string> Version( DockerArgumentOptions? dockerArgumentOptions = null );
}
