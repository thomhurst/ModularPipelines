namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class CommandPrecedingArgumentsAttribute : Attribute
{
    public CommandPrecedingArgumentsAttribute(params string[] precedingArguments)
    {
        PrecedingArguments = precedingArguments;
    }

    public string[] PrecedingArguments { get; }
}
