using ModularPipelines.Options;

namespace ModularPipelines.Docker.Options;

public record DockerOptions : CommandEnvironmentOptions;

public record DockerArgumentOptions() : CommandLineToolOptions("docker");