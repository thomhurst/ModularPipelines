namespace ModularPipelines.UnitTests.FSharp.Modules

open System.Collections.Generic
open ModularPipelines.TestHelpers

type TestModule1() =
    inherit SimpleTestModule<IDictionary<string, obj> option>()
    override _.Result = None
