namespace ModularPipelines.Enums;

[Flags]
public enum CommandLogging
{
    None = 1,
    Input = 2,
    Output = 4,
    Error = 8
}