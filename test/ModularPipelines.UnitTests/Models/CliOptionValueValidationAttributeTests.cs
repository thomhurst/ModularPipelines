using ModularPipelines.Models;

namespace ModularPipelines.UnitTests.Models;

public class CliOptionValueValidationAttributeTests
{
    [Test]
    public async Task Range_Validates_Explicit_Values()
    {
        var attribute = new CliOptionValueRangeAttribute(1, 3);
        CliOptionValue valid = "2";
        CliOptionValue invalid = "4";

        using (Assert.Multiple())
        {
            await Assert.That(attribute.IsValid(valid)).IsTrue();
            await Assert.That(attribute.IsValid(invalid)).IsFalse();
            await Assert.That(attribute.IsValid(CliOptionValue.Bare)).IsTrue();
            await Assert.That(attribute.IsValid(new[] { valid, CliOptionValue.Bare })).IsTrue();
            await Assert.That(attribute.IsValid(new[] { valid, invalid })).IsFalse();
        }
    }

    [Test]
    public async Task RegularExpression_Validates_Explicit_Values()
    {
        var attribute = new CliOptionValueRegularExpressionAttribute("^[a-z]+$");
        CliOptionValue valid = "value";
        CliOptionValue invalid = "123";

        using (Assert.Multiple())
        {
            await Assert.That(attribute.IsValid(valid)).IsTrue();
            await Assert.That(attribute.IsValid(invalid)).IsFalse();
            await Assert.That(attribute.IsValid(CliOptionValue.Bare)).IsTrue();
            await Assert.That(attribute.IsValid(new[] { valid, CliOptionValue.Bare })).IsTrue();
            await Assert.That(attribute.IsValid(new[] { valid, invalid })).IsFalse();
        }
    }
}
