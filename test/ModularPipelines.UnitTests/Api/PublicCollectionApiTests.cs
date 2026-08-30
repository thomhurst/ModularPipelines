using ModularPipelines.Secrets;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Options;
using PipelineNotInParallelAttribute = ModularPipelines.Attributes.NotInParallelAttribute;

namespace ModularPipelines.UnitTests.Api;

public class PublicCollectionApiTests
{
    private static readonly HashSet<Type> MutableCollectionDefinitions =
    [
        typeof(ICollection<>),
        typeof(IDictionary<,>),
        typeof(IList<>),
        typeof(ISet<>),
        typeof(Dictionary<,>),
        typeof(HashSet<>),
        typeof(List<>),
    ];

    [Test]
    public async Task PublicMembers_DoNotReturnMutableCollections()
    {
        var findings = typeof(PipelineOptions).Assembly
            .GetExportedTypes()
            .SelectMany(GetPublicCollectionMembers)
            .Order()
            .ToArray();

        await Assert.That(findings).IsEmpty();
    }

    [Test]
    public async Task CollectionAttributes_DefensivelyCopyConstructorArguments()
    {
        var values = new[] { "original" };
        var notInParallel = new PipelineNotInParallelAttribute(values);
        var commandAlias = new CliCommandAliasAttribute(values);
        var subCommand = new CliSubCommandAttribute(values);
        var secretValue = new SecretValueAttribute(values);

        values[0] = "changed";

        await Assert.That(notInParallel.ConstraintKeys).IsEquivalentTo(["original"]);
        await Assert.That(commandAlias.CommandParts).IsEquivalentTo(["original"]);
        await Assert.That(subCommand.SubCommands).IsEquivalentTo(["original"]);
        await Assert.That(secretValue.Keys).IsEquivalentTo(["original"]);
    }

    private static IEnumerable<string> GetPublicCollectionMembers(Type type)
    {
        const BindingFlags Flags =
            BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;

        foreach (var field in type.GetFields(Flags))
        {
            if (IsMutableCollection(field.FieldType))
            {
                yield return $"{type.FullName}.{field.Name}: {field.FieldType}";
            }
        }

        foreach (var property in type.GetProperties(Flags))
        {
            if (IsMutableCollection(property.PropertyType))
            {
                yield return $"{type.FullName}.{property.Name}: {property.PropertyType}";
            }
        }

        foreach (var method in type.GetMethods(Flags).Where(method => !method.IsSpecialName))
        {
            if (IsMutableCollection(method.ReturnType))
            {
                yield return $"{type.FullName}.{method.Name}(): {method.ReturnType}";
            }
        }
    }

    private static bool IsMutableCollection(Type type)
    {
        if (type.IsArray)
        {
            return type.GetElementType() != typeof(byte);
        }

        return type.IsGenericType
               && MutableCollectionDefinitions.Contains(type.GetGenericTypeDefinition());
    }
}
