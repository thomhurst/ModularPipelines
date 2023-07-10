namespace ModularPipelines.Git;

public class Git : IGit
{
    public Git(IGitCommands commands, IGitInformation information)
    {
        Commands = commands;
        Information = information;
    }

    public IGitCommands Commands { get; }
    public IGitInformation Information { get; }
}
