namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, Inherited = false)]
internal class ModuleMethodMarkerAttribute : Attribute
{
}