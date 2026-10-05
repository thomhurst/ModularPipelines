using System.ComponentModel.DataAnnotations;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    [Arguments("", false)]
    [Arguments(" ", false)]
    [Arguments("\t", false)]
    [Arguments(null, false)]
    [Arguments("development", true)]
    public async Task Validated_String_Collections_Reject_Blanks_And_Retain_Single_Use_Input(string? entry, bool valid)
    {
        var options = Compile(await Generate([new()
        {
            SwitchName = "--group",
            PropertyName = "Groups",
            CSharpType = "IEnumerable<string>?",
            AcceptsMultipleValues = true,
            RejectBlankCollectionValues = true,
        }])).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var enumerationCount = 0;
        IEnumerable<string> Values()
        {
            if (++enumerationCount > 1)
            {
                throw new InvalidOperationException("Input was enumerated twice.");
            }

            yield return "testing";
            yield return entry!;
        }

        options.GetProperty("Groups")!.SetValue(instance, Values());
        for (var attempt = 0; attempt < 2; attempt++)
        {
            var errors = ((IValidatableObject) instance).Validate(new(instance)).ToArray();
            await Assert.That(errors.Length == 0).IsEqualTo(valid);
            if (!valid)
            {
                await Assert.That(errors.Single().MemberNames).IsEquivalentTo(["Groups"]);
            }
        }

        await Assert.That(enumerationCount).IsEqualTo(1);
    }
}
