using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    [MatrixDataSource]
    public async Task Alternative_Ordinary_Collections_Require_A_NonNull_Rendered_Value(
        [Matrix(false, true)] bool positional,
        [Matrix("IEnumerable<string?>?", "IList<object>?", "List<object>?", "System.Collections.IEnumerable?", "System.Collections.ArrayList?")] string collectionType)
    {
        var options = Compile(await GenerateAlternativeCollection(positional, collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var property = options.GetProperty("Values")!;
        object?[][] states = [[null, null], [null, "value", null], []];
        foreach (var state in states)
        {
            object input = collectionType switch
            {
                "IEnumerable<string?>?" => state.Cast<string?>().ToArray(),
                "System.Collections.ArrayList?" => new ArrayList(state),
                _ => new List<object?>(state),
            };
            property.SetValue(instance, input);
            var hasValue = state.Any(item => item is not null);
            string[] expected = (hasValue, positional) switch
            {
                (false, _) => [],
                (true, true) => ["value"],
                (true, false) => ["--requirement", "value"],
            };
            for (var pass = 0; pass < 2; pass++)
            {
                var valid = !((IValidatableObject) instance).Validate(new(instance)).Any();
                await Assert.That(valid).IsEqualTo(hasValue);
                await Assert.That(RenderAlternativeCollection(instance, positional)).IsEquivalentTo(expected);
            }

            options.GetProperty("Fallback")!.SetValue(instance, "fallback");
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
            options.GetProperty("Fallback")!.SetValue(instance, null);
        }
    }
}
