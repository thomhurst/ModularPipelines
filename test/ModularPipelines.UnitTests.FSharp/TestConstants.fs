namespace ModularPipelines.UnitTests.FSharp

module TestConstants =
    [<Literal>]
    let TestString = "Foo bar!"

    [<Literal>]
    let ErrorPrefix = "Error: "

    [<Literal>]
    let RequirementErrorMessage = ErrorPrefix + TestString