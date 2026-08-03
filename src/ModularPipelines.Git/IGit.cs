namespace ModularPipelines.Git;

public interface IGit
{
    IGitCommands Commands { get; }

    IGitChanges Changes { get; }

    IGitInformation Information { get; }

    IGitVersioning Versioning { get; }
}
