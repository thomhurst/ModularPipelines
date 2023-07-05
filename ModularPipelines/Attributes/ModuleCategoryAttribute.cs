namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ModuleCategoryAttribute : Attribute
{
    public string Category { get; }

    public ModuleCategoryAttribute(string category)
    {
        Category = category ?? throw new ArgumentNullException(nameof(category));
    }
}
