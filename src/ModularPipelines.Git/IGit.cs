namespace ModularPipelines.Git;

public interface IGit
{
    IGitCommands Commands { get; }
    IGitInformation Information { get; }
    IGitVersioning Versioning { get; }
}
