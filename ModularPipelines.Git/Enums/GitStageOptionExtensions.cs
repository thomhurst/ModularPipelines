namespace ModularPipelines.Git.Enums;

public static class GitStageOptionExtensions
{
    public static string GetCommandLineSwitch(this GitStageOption? option)
    {
        if (option is null)
        {
            return "-A";
        }

        return option.Value.GetCommandLineSwitch();
    }

    public static string GetCommandLineSwitch(this GitStageOption option) =>
        option switch
        {
            GitStageOption.All => "-A",
            GitStageOption.ModifiedOnly => "-u",
            GitStageOption.CurrentWorkingDirectory => ".",
            _ => throw new ArgumentOutOfRangeException(nameof(option))
        };
}
