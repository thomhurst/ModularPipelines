using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.TypeDetection;

public class CliTypeMapperTests
{
    #region FromCobraTypeHint Tests

    [Test]
    public async Task FromCobraTypeHint_Returns_Bool_For_Empty_Input()
    {
        var result = CliTypeMapper.FromCobraTypeHint(null);

        await Assert.That(result).IsEqualTo(CliOptionType.Bool);
    }

    [Test]
    public async Task FromCobraTypeHint_Returns_Bool_For_Whitespace()
    {
        var result = CliTypeMapper.FromCobraTypeHint("   ");

        await Assert.That(result).IsEqualTo(CliOptionType.Bool);
    }

    [Test]
    [Arguments("bool")]
    [Arguments("boolean")]
    [Arguments("Bool")]
    [Arguments("BOOLEAN")]
    public async Task FromCobraTypeHint_Returns_Bool_For_Boolean_Types(string typeHint)
    {
        var result = CliTypeMapper.FromCobraTypeHint(typeHint);

        await Assert.That(result).IsEqualTo(CliOptionType.Bool);
    }

    [Test]
    [Arguments("int")]
    [Arguments("int32")]
    [Arguments("int64")]
    [Arguments("uint")]
    [Arguments("uint32")]
    [Arguments("uint64")]
    public async Task FromCobraTypeHint_Returns_Int_For_Integer_Types(string typeHint)
    {
        var result = CliTypeMapper.FromCobraTypeHint(typeHint);

        await Assert.That(result).IsEqualTo(CliOptionType.Int);
    }

    [Test]
    [Arguments("float")]
    [Arguments("float32")]
    [Arguments("float64")]
    public async Task FromCobraTypeHint_Returns_Decimal_For_Float_Types(string typeHint)
    {
        var result = CliTypeMapper.FromCobraTypeHint(typeHint);

        await Assert.That(result).IsEqualTo(CliOptionType.Decimal);
    }

    [Test]
    [Arguments("list")]
    [Arguments("array")]
    [Arguments("strings")]
    [Arguments("stringarray")]
    [Arguments("stringslice")]
    public async Task FromCobraTypeHint_Returns_StringList_For_List_Types(string typeHint)
    {
        var result = CliTypeMapper.FromCobraTypeHint(typeHint);

        await Assert.That(result).IsEqualTo(CliOptionType.StringList);
    }

    [Test]
    [Arguments("map")]
    [Arguments("stringtostring")]
    public async Task FromCobraTypeHint_Returns_KeyValue_For_Map_Types(string typeHint)
    {
        var result = CliTypeMapper.FromCobraTypeHint(typeHint);

        await Assert.That(result).IsEqualTo(CliOptionType.KeyValue);
    }

    [Test]
    [Arguments("string")]
    [Arguments("duration")]
    [Arguments("bytes")]
    public async Task FromCobraTypeHint_Returns_String_For_StringLike_Types(string typeHint)
    {
        var result = CliTypeMapper.FromCobraTypeHint(typeHint);

        await Assert.That(result).IsEqualTo(CliOptionType.String);
    }

    [Test]
    public async Task FromCobraTypeHint_Returns_String_For_Unknown_Types()
    {
        var result = CliTypeMapper.FromCobraTypeHint("somethingweird");

        await Assert.That(result).IsEqualTo(CliOptionType.String);
    }

    #endregion

    #region FromTypeName Tests

    [Test]
    public async Task FromTypeName_Returns_Unknown_For_Null()
    {
        var result = CliTypeMapper.FromTypeName(null);

        await Assert.That(result).IsEqualTo(CliOptionType.Unknown);
    }

    [Test]
    public async Task FromTypeName_Returns_Unknown_For_Empty()
    {
        var result = CliTypeMapper.FromTypeName("");

        await Assert.That(result).IsEqualTo(CliOptionType.Unknown);
    }

    [Test]
    [Arguments("bool", CliOptionType.Bool)]
    [Arguments("boolean", CliOptionType.Bool)]
    [Arguments("string", CliOptionType.String)]
    [Arguments("int", CliOptionType.Int)]
    [Arguments("integer", CliOptionType.Int)]
    [Arguments("decimal", CliOptionType.Decimal)]
    [Arguments("float", CliOptionType.Decimal)]
    [Arguments("double", CliOptionType.Decimal)]
    [Arguments("stringlist", CliOptionType.StringList)]
    [Arguments("list", CliOptionType.StringList)]
    [Arguments("array", CliOptionType.StringList)]
    [Arguments("keyvalue", CliOptionType.KeyValue)]
    [Arguments("map", CliOptionType.KeyValue)]
    [Arguments("dictionary", CliOptionType.KeyValue)]
    [Arguments("enum", CliOptionType.Enum)]
    [Arguments("enumeration", CliOptionType.Enum)]
    public async Task FromTypeName_Returns_Correct_Type(string typeName, CliOptionType expected)
    {
        var result = CliTypeMapper.FromTypeName(typeName);

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task FromTypeName_Returns_Unknown_For_Unrecognized_Type()
    {
        var result = CliTypeMapper.FromTypeName("custom_type");

        await Assert.That(result).IsEqualTo(CliOptionType.Unknown);
    }

    #endregion

    #region ToCSharpType Tests

    [Test]
    public async Task ToCSharpType_Returns_Bool_Nullable()
    {
        var result = CliTypeMapper.ToCSharpType(CliOptionType.Bool);

        await Assert.That(result).IsEqualTo("bool?");
    }

    [Test]
    public async Task ToCSharpType_Returns_Int_Nullable()
    {
        var result = CliTypeMapper.ToCSharpType(CliOptionType.Int);

        await Assert.That(result).IsEqualTo("int?");
    }

    [Test]
    public async Task ToCSharpType_Returns_Decimal_Nullable()
    {
        var result = CliTypeMapper.ToCSharpType(CliOptionType.Decimal);

        await Assert.That(result).IsEqualTo("decimal?");
    }

    [Test]
    public async Task ToCSharpType_Returns_String_Array()
    {
        var result = CliTypeMapper.ToCSharpType(CliOptionType.StringList);

        await Assert.That(result).IsEqualTo("string[]?");
    }

    [Test]
    public async Task ToCSharpType_Returns_KeyValue_Enumerable()
    {
        var result = CliTypeMapper.ToCSharpType(CliOptionType.KeyValue);

        await Assert.That(result).IsEqualTo("IEnumerable<KeyValue>?");
    }

    [Test]
    public async Task ToCSharpType_Returns_String_For_Unknown()
    {
        var result = CliTypeMapper.ToCSharpType(CliOptionType.Unknown);

        await Assert.That(result).IsEqualTo("string?");
    }

    [Test]
    public async Task ToCSharpType_Returns_String_For_Enum_Without_Definition()
    {
        var result = CliTypeMapper.ToCSharpType(CliOptionType.Enum);

        await Assert.That(result).IsEqualTo("string?");
    }

    [Test]
    public async Task ToCSharpType_Returns_EnumName_For_Enum_With_Definition()
    {
        var enumDef = new CliEnumDefinition
        {
            EnumName = "MyEnum",
            Values = [],
        };

        var result = CliTypeMapper.ToCSharpType(CliOptionType.Enum, enumDef);

        await Assert.That(result).IsEqualTo("MyEnum?");
    }

    [Test]
    public async Task ToCSharpType_Returns_EnumName_When_EnumDef_Provided_For_Any_Type()
    {
        var enumDef = new CliEnumDefinition
        {
            EnumName = "OutputFormat",
            Values = [],
        };

        var result = CliTypeMapper.ToCSharpType(CliOptionType.String, enumDef);

        await Assert.That(result).IsEqualTo("OutputFormat?");
    }

    #endregion

    #region ToTypeName Tests

    [Test]
    [Arguments(CliOptionType.Bool, "bool")]
    [Arguments(CliOptionType.String, "string")]
    [Arguments(CliOptionType.Int, "int")]
    [Arguments(CliOptionType.Decimal, "decimal")]
    [Arguments(CliOptionType.StringList, "stringlist")]
    [Arguments(CliOptionType.KeyValue, "keyvalue")]
    [Arguments(CliOptionType.Enum, "enum")]
    [Arguments(CliOptionType.Unknown, "unknown")]
    public async Task ToTypeName_Returns_Correct_String(CliOptionType type, string expected)
    {
        var result = CliTypeMapper.ToTypeName(type);

        await Assert.That(result).IsEqualTo(expected);
    }

    #endregion

    #region IsKnownCobraType Tests

    [Test]
    public async Task IsKnownCobraType_Returns_True_For_Null()
    {
        var result = CliTypeMapper.IsKnownCobraType(null);

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task IsKnownCobraType_Returns_True_For_Empty()
    {
        var result = CliTypeMapper.IsKnownCobraType("");

        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments("string")]
    [Arguments("int")]
    [Arguments("bool")]
    [Arguments("list")]
    [Arguments("map")]
    [Arguments("duration")]
    public async Task IsKnownCobraType_Returns_True_For_Known_Types(string typeHint)
    {
        var result = CliTypeMapper.IsKnownCobraType(typeHint);

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task IsKnownCobraType_Returns_False_For_Unknown_Types()
    {
        var result = CliTypeMapper.IsKnownCobraType("custom_type");

        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task IsKnownCobraType_Is_Case_Insensitive()
    {
        var result = CliTypeMapper.IsKnownCobraType("STRING");

        await Assert.That(result).IsTrue();
    }

    #endregion
}
