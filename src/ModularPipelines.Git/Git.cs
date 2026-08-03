namespace ModularPipelines.Git;

internal class Git : IGit
{
    public Git(
        IGitCommands commands,
        IGitChanges changes,
        IGitInformation information,
        IGitVersioning versioning)
    {
        Commands = commands;
        Changes = changes;
        Information = information;
        Versioning = versioning;
    }

    public IGitCommands Commands { get; }

    public IGitChanges Changes { get; }

    public IGitInformation Information { get; }

    public IGitVersioning Versioning { get; }
}
