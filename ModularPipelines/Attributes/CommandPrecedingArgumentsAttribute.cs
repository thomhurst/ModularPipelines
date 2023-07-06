namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class CommandPrecedingArgumentsAttribute : Attribute
{
    public string[] PrecedingArguments { get; }

    public CommandPrecedingArgumentsAttribute(params string[] precedingArguments)
    {
        PrecedingArguments = precedingArguments;
    }
}
