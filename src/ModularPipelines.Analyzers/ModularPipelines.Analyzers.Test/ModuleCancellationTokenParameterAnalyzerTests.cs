using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleCancellationTokenParameterAnalyzerTests
{
    [TestMethod]
    [DataRow("Task")]
    [DataRow("ValueTask")]
    public async Task Async_Helper_Unrelated_Collections_Do_Not_Replace_Returned_Scalars(string taskType)
    {
        var helper = $"static async {taskType}<CancellationToken> Preserve(CancellationToken token) {{ await Task.Yield(); var unused = new CancellationToken[1]; return token; }} ";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(helper + "cancellationToken = await Preserve(cancellationToken);", "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("Task", "out")]
    [DataRow("ValueTask", "out")]
    [DataRow("Task", "ref")]
    [DataRow("ValueTask", "ref")]
    public async Task Completed_Void_Calls_Allow_Awaitable_Storage_Restoration(string taskType, string reference)
    {
        var helper = $"static void Clear({reference} {taskType}<CancellationToken> value) => value = {taskType}.FromResult(CancellationToken.None); ";
        var statements = helper + $"var saved = {taskType}.FromResult(cancellationToken); Clear({reference} saved); saved = {taskType}.FromResult(cancellationToken); cancellationToken = await saved;";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("Task")]
    [DataRow("ValueTask")]
    public async Task Non_Async_Converted_Awaitable_Returns_Do_Not_Prove_Payloads(string taskType)
    {
        var helper = $"static {taskType}<CancellationToken> Replace(CancellationToken token) => ({taskType}<CancellationToken>)(ReplacingAwaitable){taskType}.FromResult(token); ";
        var source = AsyncSource(helper + "cancellationToken = await Replace(cancellationToken);", "{|#0:Task.Delay(1, cancellationToken)|}") + $$"""

            public struct ReplacingAwaitable
            {
                public static implicit operator ReplacingAwaitable({{taskType}}<CancellationToken> value) => new();
                public static implicit operator {{taskType}}<CancellationToken>(ReplacingAwaitable value) => {{taskType}}.FromResult(CancellationToken.None);
            }
            """;
        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Stable_Readonly_Array_References_Invalidate_Only_Their_Allocation(bool affected)
    {
        var statements = "CancellationToken[] tokens = new[] { cancellationToken }; CancellationToken[] other = new[] { cancellationToken }; ref readonly CancellationToken[] alias = ref other; alias[0] = default; "
            + $"cancellationToken = CancellationTokenSource.CreateLinkedTokenSource({(affected ? "other" : "tokens")}).Token;";
        var call = affected ? "{|#0:Task.Delay(1, cancellationToken)|}" : "Task.Delay(1, cancellationToken)";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, call), affected
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }

    [TestMethod]
    [DataRow("Task")]
    [DataRow("ValueTask")]
    public async Task Non_Async_Helper_Payloads_Reject_User_Defined_Conversions(string taskType)
    {
        var helper = $"static {taskType}<CancellationToken> Replace(CancellationToken token) => {taskType}.FromResult((CancellationToken)(ReplacingToken)token); ";
        var source = AsyncSource(helper + "cancellationToken = await Replace(cancellationToken);", "{|#0:Task.Delay(1, cancellationToken)|}") + """

            public struct ReplacingToken
            {
                public static implicit operator ReplacingToken(CancellationToken value) => new();
                public static implicit operator CancellationToken(ReplacingToken value) => CancellationToken.None;
            }
            """;
        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("Task", false)]
    [DataRow("ValueTask", false)]
    [DataRow("Task", true)]
    [DataRow("ValueTask", true)]
    public async Task Generic_Helper_Payloads_Reject_Parameter_Writes(string taskType, bool isAsync)
    {
        var helper = isAsync
            ? $"static async {taskType}<T> Replace<T>(T token) where T : struct {{ await Task.Yield(); token = default; return token; }} "
            : $"static {taskType}<T> Replace<T>(T token) where T : struct {{ token = default; return {taskType}.FromResult(token); }} ";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(helper + "cancellationToken = await Replace(cancellationToken);", "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("Task")]
    [DataRow("ValueTask")]
    public async Task Generic_Non_Async_Helper_Factories_Preserve_Scalar_Payloads(string taskType)
    {
        var helper = $"static {taskType}<T> Preserve<T>(T token) => {taskType}.FromResult(token); ";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(helper + "cancellationToken = await Preserve(cancellationToken);", "Task.Delay(1, cancellationToken)"));
    }


    [TestMethod]
    [DataRow("ref (context != null ? ref first : ref second)", "", "first", true)]
    [DataRow("ref (context != null ? ref first : ref second)", "", "untouched", false)]
    [DataRow("ref first", "alias = ref second;", "first", false)]
    public async Task Array_Reference_Targets_Follow_Conditional_Bindings_And_Reassignment(string binding, string rebind, string source, bool warns)
    {
        var statements = "CancellationToken[] first = new[] { cancellationToken }; CancellationToken[] second = new[] { cancellationToken }; CancellationToken[] untouched = new[] { cancellationToken }; "
            + $"ref readonly CancellationToken[] alias = {binding}; {rebind} alias[0] = default; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource({source}).Token;";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, warns ? "{|#0:Task.Delay(1, cancellationToken)|}" : "Task.Delay(1, cancellationToken)"), warns
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }

    [TestMethod]
    [DataRow("Task")]
    [DataRow("ValueTask")]
    public async Task Unrelated_Collections_Do_Not_Disprove_Unchanged_Helper_Returns(string taskType)
    {
        var helper = $"static async {taskType}<CancellationToken> Preserve(CancellationToken token) {{ await Task.Yield(); var unused = new CancellationToken[1]; return token; }} ";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(helper + "cancellationToken = await Preserve(cancellationToken);", "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("Task", "out")]
    [DataRow("ValueTask", "out")]
    [DataRow("Task", "ref")]
    [DataRow("ValueTask", "ref")]
    public async Task Completed_Reference_Calls_Allow_Payload_Restoration(string taskType, string reference)
    {
        var statements = $"static void Clear({reference} {taskType}<CancellationToken> value) => value = {taskType}.FromResult(CancellationToken.None); "
            + $"var saved = {taskType}.FromResult(cancellationToken); Clear({reference} saved); saved = {taskType}.FromResult(cancellationToken); cancellationToken = await saved;";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow(false, false)]
    [DataRow(true, false)]
    [DataRow(false, true)]
    [DataRow(true, true)]
    public async Task Readonly_Reference_Array_Writes_Track_Current_Target_Storage(bool retargetStorage, bool affected)
    {
        var statements = "CancellationToken[] tokens = new[] { cancellationToken }; CancellationToken[] other = new[] { cancellationToken }; ref readonly CancellationToken[] alias = ref other; "
            + (retargetStorage ? "other = tokens; " : "") + "alias[0] = default; "
            + $"cancellationToken = CancellationTokenSource.CreateLinkedTokenSource({(affected ? "other" : "tokens")}).Token;";
        var warns = affected || retargetStorage;
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, warns ? "{|#0:Task.Delay(1, cancellationToken)|}" : "Task.Delay(1, cancellationToken)"), warns
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }

    [TestMethod]
    [DataRow("new ValueTask<CancellationToken>(token)")]
    [DataRow("new ValueTask<CancellationToken>(Task.FromResult(token))")]
    public async Task Non_Async_ValueTask_Constructors_Preserve_Helper_Payloads(string result)
    {
        var helper = $"static ValueTask<CancellationToken> Preserve(CancellationToken token) => {result}; ";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(helper + "cancellationToken = await Preserve(cancellationToken);", "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("Task")]
    [DataRow("ValueTask")]
    public async Task Generic_Async_Helpers_Do_Not_Prove_Mutated_Local_Collections(string taskType)
    {
        var helper = $"static async {taskType}<CancellationToken> Replace<T>(T token) {{ await Task.Yield(); var tokens = new T[] {{ token }}; tokens[0] = default!; return (CancellationToken)(object)tokens[0]!; }} ";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(helper + "cancellationToken = await Replace(cancellationToken);", "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("Task", false, false)]
    [DataRow("ValueTask", false, false)]
    [DataRow("Task", true, false)]
    [DataRow("ValueTask", true, false)]
    [DataRow("Task", false, true)]
    [DataRow("ValueTask", false, true)]
    [DataRow("Task", true, true)]
    [DataRow("ValueTask", true, true)]
    public async Task Source_Helpers_Preserve_Only_Original_Scalar_Payloads(string taskType, bool genericAsync, bool replaces)
    {
        var value = replaces ? "default!" : "token";
        var helper = genericAsync
            ? $"static async {taskType}<T> Preserve<T>(T token) {{ await Task.Yield(); return {value}; }}"
            : $"static {taskType}<CancellationToken> Preserve(CancellationToken token) => {taskType}.FromResult<CancellationToken>({value});";
        var call = replaces ? "{|#0:Task.Delay(1, cancellationToken)|}" : "Task.Delay(1, cancellationToken)";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(helper + " cancellationToken = await Preserve(cancellationToken);", call), replaces
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }

    [TestMethod]
    [DataRow("Task", false)]
    [DataRow("ValueTask", false)]
    [DataRow("Task", true)]
    [DataRow("ValueTask", true)]
    public async Task Conditional_Tuple_References_Preserve_Only_Unaffected_Payloads(string taskType, bool affected)
    {
        var field = affected ? "Item2" : "Item1";
        var statements = $"({taskType}<CancellationToken>, {taskType}<CancellationToken>) saved = ({taskType}.FromResult(cancellationToken), {taskType}.FromResult(cancellationToken)); "
            + $"{taskType}<CancellationToken> other = {taskType}.FromResult(cancellationToken); ref {taskType}<CancellationToken> alias = ref (context != null ? ref saved.Item2 : ref other); "
            + $"saved.Item2 = {taskType}.FromResult(cancellationToken); alias = {taskType}.FromResult(CancellationToken.None); cancellationToken = await saved.{field};";
        var call = affected ? "{|#0:Task.Delay(1, cancellationToken)|}" : "Task.Delay(1, cancellationToken)";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, call), affected
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }

    [TestMethod]
    [DataRow("ref")]
    [DataRow("ref readonly")]
    public async Task Array_References_Invalidate_Backing_Facts_Before_Storage_Escape(string reference)
    {
        var statements = $"CancellationToken[] tokens = new[] {{ cancellationToken }}; var copy = tokens; {reference} CancellationToken[] alias = ref tokens; alias[0] = default; "
            + "cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(copy).Token;";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Readonly_Span_References_Invalidate_Only_On_Content_Writes(bool writes)
    {
        var mutation = writes ? "alias[0] = default;" : "";
        var statements = "var tokens = new[] { cancellationToken }; var span = new Span<CancellationToken>(tokens); ref readonly Span<CancellationToken> alias = ref span; "
            + $"{mutation} cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;";
        var call = writes ? "{|#0:Task.Delay(1, cancellationToken)|}" : "Task.Delay(1, cancellationToken)";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, call), writes
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }

    [TestMethod]
    public async Task Readonly_Array_References_Preserve_Unchanged_Contents()
    {
        var statements = "CancellationToken[] tokens = new[] { cancellationToken }; ref readonly CancellationToken[] alias = ref tokens; "
            + "cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("Task", "tokens[0] = default;")]
    [DataRow("ValueTask", "tokens[0] = default;")]
    [DataRow("Task", "var alias = tokens; alias[0] = default;")]
    [DataRow("ValueTask", "var alias = tokens; alias[0] = default;")]
    public async Task Async_Collection_Helpers_Do_Not_Prove_Mutated_Input_Payloads(string taskType, string mutation)
    {
        var statements = $"static async {taskType}<CancellationToken> Link(CancellationToken[] tokens) {{ await Task.Yield(); {mutation} return CancellationTokenSource.CreateLinkedTokenSource(tokens).Token; }} "
            + "var tokens = new[] { cancellationToken }; cancellationToken = await Link(tokens);";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("Task")]
    [DataRow("ValueTask")]
    public async Task Async_Scalar_Helpers_Do_Not_Prove_Mutated_Local_Collections(string taskType)
    {
        var statements = $"static async {taskType}<CancellationToken> Link(CancellationToken token) {{ await Task.Yield(); var tokens = new[] {{ token }}; tokens[0] = default; return CancellationTokenSource.CreateLinkedTokenSource(tokens).Token; }} "
            + "cancellationToken = await Link(cancellationToken);";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Conditional_Parameter_References_Track_Writable_Escapes(bool writable)
    {
        var reference = writable ? "ref CancellationToken" : "ref readonly CancellationToken";
        var mutation = writable ? "alias = default;" : "";
        var statements = $"CancellationToken other = default; {reference} alias = ref (context != null ? ref cancellationToken : ref other); {mutation}";
        var call = writable ? "{|#0:Task.Delay(1, cancellationToken)|}" : "Task.Delay(1, cancellationToken)";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, call), writable
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }

    [TestMethod]
    [DataRow("Task")]
    [DataRow("ValueTask")]
    public async Task Returned_Awaitable_References_Invalidate_Argument_Storage(string taskType)
    {
        var statements = $"static ref {taskType}<CancellationToken> Keep(ref {taskType}<CancellationToken> value) => ref value; "
            + $"{taskType}<CancellationToken> saved = {taskType}.FromResult(cancellationToken); ref {taskType}<CancellationToken> alias = ref Keep(ref saved); "
            + $"saved = {taskType}.FromResult(cancellationToken); alias = {taskType}.FromResult(CancellationToken.None); cancellationToken = await saved;";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("Task")]
    [DataRow("ValueTask")]
    public async Task Conditional_Awaitable_References_Invalidate_Captured_Storage(string taskType)
    {
        var statements = $"{taskType}<CancellationToken> saved = {taskType}.FromResult(cancellationToken); {taskType}<CancellationToken> other = {taskType}.FromResult(cancellationToken); "
            + $"ref {taskType}<CancellationToken> alias = ref (context != null ? ref saved : ref other); alias = {taskType}.FromResult(CancellationToken.None); cancellationToken = await saved;";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("Task")]
    [DataRow("ValueTask")]
    public async Task Readonly_Awaitable_Aliases_Do_Not_Retain_Stale_Payloads(string taskType)
    {
        var statements = $"{taskType}<CancellationToken> saved = {taskType}.FromResult(cancellationToken); ref readonly {taskType}<CancellationToken> alias = ref saved; "
            + $"saved = {taskType}.FromResult(CancellationToken.None); cancellationToken = await alias;";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("Task", "alias = replacement;", "saved")]
    [DataRow("ValueTask", "alias = replacement;", "saved")]
    [DataRow("Task", "saved = Task.FromResult(cancellationToken); alias = replacement;", "saved")]
    [DataRow("ValueTask", "saved = ValueTask.FromResult(cancellationToken); alias = replacement;", "saved")]
    [DataRow("Task", "saved = replacement;", "alias")]
    [DataRow("ValueTask", "saved = replacement;", "alias")]
    public async Task Writable_Awaitable_References_Invalidate_Storage(string taskType, string mutation, string awaited)
    {
        var statements = $"{taskType}<CancellationToken> saved = {taskType}.FromResult(cancellationToken); var replacement = {taskType}.FromResult(CancellationToken.None); "
            + $"ref {taskType}<CancellationToken> alias = ref saved; {mutation} cancellationToken = await {awaited};";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("Task")]
    [DataRow("ValueTask")]
    public async Task Writable_Awaitable_Reference_Reassignment_Invalidates_Storage(string taskType)
    {
        var statements = $"{taskType}<CancellationToken> saved = {taskType}.FromResult(cancellationToken); {taskType}<CancellationToken> other = {taskType}.FromResult(cancellationToken); "
            + $"ref {taskType}<CancellationToken> alias = ref other; alias = ref saved; alias = {taskType}.FromResult(CancellationToken.None); cancellationToken = await saved;";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("Task")]
    [DataRow("ValueTask")]
    public async Task Writable_Tuple_Awaitable_References_Invalidate_Storage(string taskType)
    {
        var statements = $"({taskType}<CancellationToken>, int) saved = ({taskType}.FromResult(cancellationToken), 1); ref {taskType}<CancellationToken> alias = ref saved.Item1; "
            + $"saved.Item1 = {taskType}.FromResult(cancellationToken); alias = {taskType}.FromResult(CancellationToken.None); cancellationToken = await saved.Item1;";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("Task", "")]
    [DataRow("ValueTask", "")]
    [DataRow("Task", "alias = ref saved;")]
    [DataRow("ValueTask", "alias = ref saved;")]
    public async Task Readonly_Awaitable_References_Preserve_Storage(string taskType, string reassignment)
    {
        var statements = $"{taskType}<CancellationToken> saved = {taskType}.FromResult(cancellationToken); ref readonly {taskType}<CancellationToken> alias = ref saved; "
            + $"{reassignment} cancellationToken = await saved;";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("Task")]
    [DataRow("ValueTask")]
    public async Task Copied_Awaitable_Payload_Survives_Reference_Escape(string taskType)
    {
        var statements = $"{taskType}<CancellationToken> saved = {taskType}.FromResult(cancellationToken); var copy = saved; ref {taskType}<CancellationToken> alias = ref saved; "
            + $"alias = {taskType}.FromResult(CancellationToken.None); cancellationToken = await copy;";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("Task", "{ await Task.Yield(); return token; }", false)]
    [DataRow("ValueTask", "{ await Task.Yield(); return token; }", false)]
    [DataRow("Task", "=> token;", false)]
    [DataRow("ValueTask", "=> token;", false)]
    [DataRow("Task", "{ await Task.Yield(); return token; }", true)]
    [DataRow("ValueTask", "{ await Task.Yield(); return token; }", true)]
    public async Task Async_Source_Helpers_Preserve_Original_Payload(string taskType, string body, bool configured)
    {
        var expression = configured ? "PreserveAsync(cancellationToken).ConfigureAwait(false)" : "PreserveAsync(cancellationToken)";
        var statements = $"static async {taskType}<CancellationToken> PreserveAsync(CancellationToken token) {body} "
            + $"var saved = {expression}; cancellationToken = default; cancellationToken = await saved;";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [Timeout(30000, CooperativeCancellation = true)]
    [DataRow("Task", "await Task.Yield(); return CancellationToken.None;")]
    [DataRow("ValueTask", "await Task.Yield(); return CancellationToken.None;")]
    [DataRow("Task", "await Task.Yield(); token = default; return token;")]
    [DataRow("ValueTask", "await Task.Yield(); token = default; return token;")]
    [DataRow("Task", "await Task.Yield(); if (DateTime.Now.Ticks > 0) return token; return CancellationToken.None;")]
    [DataRow("ValueTask", "await Task.Yield(); if (DateTime.Now.Ticks > 0) return token; return CancellationToken.None;")]
    [DataRow("Task", "await Task.Yield(); return await ReplaceAsync(token);")]
    [DataRow("ValueTask", "await Task.Yield(); return await ReplaceAsync(token);")]
    public async Task Async_Source_Helpers_Report_Replaced_Or_Unproven_Payload(string taskType, string body)
    {
        var statements = $"static async {taskType}<CancellationToken> ReplaceAsync(CancellationToken token) {{ {body} }} "
            + "cancellationToken = await ReplaceAsync(cancellationToken);";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("Task", "tokens[0] = default;")]
    [DataRow("ValueTask", "tokens[0] = default;")]
    [DataRow("Task", "var alias = tokens; alias[0] = default;")]
    [DataRow("ValueTask", "var alias = tokens; alias[0] = default;")]
    [DataRow("Task", "Array.Clear(tokens, 0, tokens.Length);")]
    [DataRow("ValueTask", "Array.Clear(tokens, 0, tokens.Length);")]
    public async Task Async_Source_Helpers_Report_Mutated_Collection_Payload(string taskType, string mutation)
    {
        var statements = $"static async {taskType}<CancellationToken> Link(CancellationToken[] tokens) "
            + $"{{ await Task.Yield(); {mutation} return CancellationTokenSource.CreateLinkedTokenSource(tokens).Token; }} "
            + "var tokens = new[] { cancellationToken }; cancellationToken = await Link(tokens);";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Awaitable_Payload_Joins_Require_Original_On_Every_Path(bool replaced)
    {
        var replacement = replaced ? "Task.FromResult(CancellationToken.None)" : "Task.FromResult(cancellationToken)";
        var statements = "var saved = Task.FromResult(cancellationToken); "
            + $"if (context != null) saved = {replacement}; "
            + "cancellationToken = await saved;";
        var call = replaced ? "{|#0:Task.Delay(1, cancellationToken)|}" : "Task.Delay(1, cancellationToken)";
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, call), replaced
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }

    [TestMethod]
    public async Task Direct_Await_Preserves_Token_Payload()
    {
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(
            "cancellationToken = await Task.FromResult(cancellationToken);", "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("Task.FromResult(cancellationToken)")]
    [DataRow("Task.FromResult(cancellationToken).ConfigureAwait(false)")]
    [DataRow("ValueTask.FromResult(cancellationToken)")]
    [DataRow("new ValueTask<CancellationToken>(cancellationToken)")]
    [DataRow("new ValueTask<CancellationToken>(Task.FromResult(cancellationToken)).ConfigureAwait(false)")]
    [DataRow("new ValueTask<CancellationToken>(cancellationToken).AsTask()")]
    public async Task Awaitable_Wrappers_Preserve_Token_Payload(string expression)
    {
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(
            $"var saved = {expression}; cancellationToken = default; cancellationToken = await saved;", "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("Task.FromResult(CancellationToken.None)")]
    [DataRow("Task.FromResult(CancellationToken.None).ConfigureAwait(false)")]
    [DataRow("ValueTask.FromResult(CancellationToken.None)")]
    [DataRow("new ValueTask<CancellationToken>(CancellationToken.None)")]
    [DataRow("new ValueTask<CancellationToken>(Task.FromResult(CancellationToken.None)).ConfigureAwait(false)")]
    [DataRow("new ValueTask<CancellationToken>(CancellationToken.None).AsTask()")]
    public async Task Awaitable_Wrappers_Report_Replaced_Payload(string expression)
    {
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(
            $"var saved = {expression}; cancellationToken = await saved;", "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("cancellationToken = await Task.FromResult(cancellationToken);")]
    [DataRow("cancellationToken = await Task.FromResult(cancellationToken).ConfigureAwait(false);")]
    [DataRow("cancellationToken = await ValueTask.FromResult(cancellationToken);")]
    [DataRow("cancellationToken = await ValueTask.FromResult(cancellationToken).ConfigureAwait(false);")]
    [DataRow("cancellationToken = await new ValueTask<CancellationToken>(cancellationToken);")]
    [DataRow("cancellationToken = await new ValueTask<CancellationToken>(Task.FromResult(cancellationToken));")]
    [DataRow("cancellationToken = await new ValueTask<CancellationToken>(cancellationToken).ConfigureAwait(false);")]
    [DataRow("var saved = Task.FromResult(cancellationToken); cancellationToken = CancellationToken.None; cancellationToken = await saved;")]
    [DataRow("var saved = ValueTask.FromResult(cancellationToken); cancellationToken = CancellationToken.None; cancellationToken = await saved;")]
    [DataRow("var saved = context != null ? Task.FromResult(cancellationToken) : Task.FromResult(cancellationToken); cancellationToken = await saved;")]
    [DataRow("static bool Reset(out CancellationToken token) { token = default; return false; } var saved = Task.FromResult(cancellationToken); cancellationToken = await saved.ConfigureAwait(Reset(out cancellationToken));")]
    public async Task Awaitable_Wrappers_Preserve_Original_Parameter(string statements)
    {
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("cancellationToken = await Task.FromResult(CancellationToken.None);")]
    [DataRow("cancellationToken = await ValueTask.FromResult(CancellationToken.None);")]
    [DataRow("cancellationToken = await new ValueTask<CancellationToken>(CancellationToken.None);")]
    [DataRow("cancellationToken = await new ValueTask<CancellationToken>(Task.FromResult(CancellationToken.None));")]
    [DataRow("var saved = context != null ? Task.FromResult(cancellationToken) : Task.FromResult(CancellationToken.None); cancellationToken = await saved;")]
    [DataRow("static Task<CancellationToken> ReplaceAsync(CancellationToken token) => Task.FromResult(CancellationToken.None); cancellationToken = await ReplaceAsync(cancellationToken);")]
    [DataRow("cancellationToken = await default(ValueTask<CancellationToken>);")]
    public async Task Reports_Replacement_Through_Awaitable_Wrappers(string statements)
    {
        await VerifyCS.VerifyAnalyzerAsync(AsyncSource(statements, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("cancellationToken = await (ReplacingAwaitable)Task.FromResult(cancellationToken);")]
    [DataRow("cancellationToken = await (Task<CancellationToken>)(ReplacingAwaitable)Task.FromResult(cancellationToken);")]
    [DataRow("static Task<CancellationToken> Replace(CancellationToken token) => (Task<CancellationToken>)(ReplacingAwaitable)Task.FromResult(token); cancellationToken = await Replace(cancellationToken);")]
    [DataRow("static ValueTask<CancellationToken> Replace(CancellationToken token) => new ValueTask<CancellationToken>((Task<CancellationToken>)(ReplacingAwaitable)Task.FromResult(token)); cancellationToken = await Replace(cancellationToken);")]
    public async Task Reports_Replacement_Through_Custom_Awaitable_Conversions(string statements)
    {
        var source = AsyncSource(statements, "{|#0:Task.Delay(1, cancellationToken)|}") + """

            public class ReplacingAwaitable
            {
                public static implicit operator ReplacingAwaitable(Task<CancellationToken> value) => new();
                public static implicit operator Task<CancellationToken>(ReplacingAwaitable value) => Task.FromResult(CancellationToken.None);
                public System.Runtime.CompilerServices.TaskAwaiter<CancellationToken> GetAwaiter() => Task.FromResult(CancellationToken.None).GetAwaiter();
            }
            """;
        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [Timeout(30000, CooperativeCancellation = true)]
    public async Task Awaitable_Boxing_Has_Bounded_Cost()
    {
        await VerifyCS.VerifyAnalyzerAsync(Source(
            "cancellationToken = cancellationToken; object saved = cancellationToken; while (DateTime.Now.Ticks < 0) saved = Task.FromResult(saved);",
            "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("Span")]
    [DataRow("ReadOnlySpan")]
    public async Task Reports_Replacement_Read_From_Nonzero_Slice(string spanType)
    {
        var statements = $"var tokens = new[] {{ cancellationToken, CancellationToken.None }}; var slice = new {spanType}<CancellationToken>(tokens, 1, 1); cancellationToken = slice[0];";
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("ref readonly CancellationToken first = ref tokens[0];")]
    [DataRow("CancellationToken other = default; ref readonly CancellationToken first = ref other; first = ref tokens[0];")]
    [DataRow("var span = new Span<CancellationToken>(tokens); ref readonly CancellationToken first = ref span[0];")]
    [DataRow("var span = new Span<CancellationToken>(tokens); CancellationToken other = default; ref readonly CancellationToken first = ref other; first = ref span[0];")]
    public async Task Readonly_References_Preserve_Collection_Provenance(string reference)
    {
        var statements = $"var tokens = new[] {{ cancellationToken }}; {reference} cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;";
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow("var tokens = new[] { cancellationToken }; var span = new Span<CancellationToken>(tokens); ref CancellationToken first = ref span[0]; first = default; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(span).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; ref CancellationToken first = ref tokens[0]; first = default; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("cancellationToken = CancellationToken.None;")]
    [DataRow("cancellationToken = new CancellationToken();")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; if (context != null) { cancellationToken = saved; }")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; if (context != null) { cancellationToken = saved; } else { cancellationToken = CancellationToken.None; }")]
    [DataRow("if (DateTime.Now.Ticks > 0) cancellationToken = CancellationToken.None;")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; saved = cancellationToken; cancellationToken = saved;")]
    [DataRow("if (context != null) cancellationToken = CancellationToken.None; else cancellationToken = new CancellationToken();")]
    [DataRow("(cancellationToken, _) = (CancellationToken.None, 1);")]
    [DataRow("var saved = cancellationToken; (saved, _) = (CancellationToken.None, 1); cancellationToken = saved;")]
    [DataRow("var saved = cancellationToken; (cancellationToken, saved) = (CancellationToken.None, cancellationToken); cancellationToken = CancellationToken.None;")]
    [DataRow("cancellationToken = Replace(cancellationToken);")]
    [DataRow("cancellationToken = Recurse(cancellationToken);")]
    [DataRow("cancellationToken = MutatingRecurse(cancellationToken);")]
    [DataRow("cancellationToken = Pass(CancellationToken.None);")]
    [DataRow("var tokens = new[] { cancellationToken }; tokens[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = tokens; alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = context != null ? tokens : new CancellationToken[1]; alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var saved = context != null ? (tokens, 1) : default; saved.Item1[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = tokens; tokens = new[] { cancellationToken }; alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;")]
    [DataRow("var saved = (cancellationToken, 1); saved.Item1 = CancellationToken.None; (cancellationToken, _) = saved;")]
    [DataRow("var saved = (cancellationToken, 1); var copy = saved; copy.Item1 = CancellationToken.None; (cancellationToken, _) = copy;")]
    [DataRow("var saved = (cancellationToken, 1); if (context != null) saved.Item1 = CancellationToken.None; (cancellationToken, _) = saved;")]
    [DataRow("var tokens = new[] { cancellationToken }; tokens[context != null ? 0 : 0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var saved = (tokens, 1); saved.Item1[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; Array.Clear(tokens, 0, 1); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; tokens.SetValue(CancellationToken.None, 0); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; Span<CancellationToken> alias = tokens; alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; object saved = (tokens, 1); ((ValueTuple<CancellationToken[], int>)saved).Item1[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var saved = new object[] { tokens }; ((CancellationToken[])saved[0])[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var saved = (cancellationToken, 1); saved = (CancellationToken.None, 1); (cancellationToken, _) = saved;")]
    [DataRow("var saved = (cancellationToken, 1); saved.Item1 = CancellationToken.None; cancellationToken = saved.Item1;")]
    [DataRow("var saved = (cancellationToken, 1); if (context != null) saved = (CancellationToken.None, 1); (cancellationToken, _) = saved;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); saved.Item1 = (CancellationToken.None, 1); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); static void Clear(ref CancellationToken token) => token = CancellationToken.None; Clear(ref saved.Item1.Item1); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); static void Clear(out CancellationToken token) => token = CancellationToken.None; Clear(out saved.Item1.Item1); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); (saved.Item1, _) = ((CancellationToken.None, 1), 2); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); saved.Item1 = (CancellationToken.None, 1); cancellationToken = saved.Item1.Item1;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); static void Clear(ref (CancellationToken, int) pair) => pair = (CancellationToken.None, 1); Clear(ref saved.Item1); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); static void Clear(out (CancellationToken, int) pair) => pair = (CancellationToken.None, 1); Clear(out saved.Item1); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = (cancellationToken, 1); (saved.Item1, _) = (CancellationToken.None, 2); cancellationToken = saved.Item1;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = context != null ? tokens : new[] { cancellationToken }; alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; static void Clear(ref CancellationToken token) => token = CancellationToken.None; Clear(ref tokens[0]); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; (tokens[0], _) = (CancellationToken.None, 1); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = tokens; static void Clear(ref CancellationToken[] values) => values[0] = CancellationToken.None; Clear(ref alias); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var saved = (tokens, 1); tokens[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(saved.Item1).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; static CancellationToken[] PassArray(CancellationToken[] value) => value; var alias = PassArray(tokens); alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; static CancellationToken[] PassArray(CancellationToken[] value) => value; var alias = PassArray(tokens); alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new CancellationToken[1]; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new CancellationToken[1]; tokens[0] = cancellationToken; tokens[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = tokens.PassArray(); alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; static object PassObject(object value) => value; var alias = (CancellationToken[])PassObject(tokens); alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; static CancellationToken[] Clear(CancellationToken[] value) { value[0] = CancellationToken.None; return value; } var alias = Clear(tokens); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; static object Box((CancellationToken[], int) pair) => pair; var saved = Box((tokens, 1)); ((ValueTuple<CancellationToken[], int>)saved).Item1[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("(CancellationToken First, CancellationToken Second) saved = default; saved.First = cancellationToken; cancellationToken = saved.Second;")]
    [DataRow("(CancellationToken Token, int Count) saved = default; if (context != null) saved.Token = cancellationToken; cancellationToken = saved.Token;")]
    [DataRow("(CancellationToken Token, int Count) saved = default; saved.Token = cancellationToken; saved = default; cancellationToken = saved.Token;")]
    [DataRow("(CancellationToken[] Tokens, int Count) saved = default; saved.Tokens = new[] { cancellationToken }; saved.Tokens[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(saved.Tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; static CancellationToken Link(ref CancellationToken token, CancellationToken[] values) { token = default; return CancellationTokenSource.CreateLinkedTokenSource(values).Token; } cancellationToken = Link(ref tokens[0], tokens);")]
    [DataRow("var tokens = new[] { cancellationToken }; static CancellationToken Link(out CancellationToken token, CancellationToken[] values) { token = default; return CancellationTokenSource.CreateLinkedTokenSource(values).Token; } cancellationToken = Link(out tokens[0], tokens);")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = new Span<CancellationToken>(tokens); alias[0] = default; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = new ReadOnlySpan<CancellationToken>(tokens); tokens[0] = default; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; static CancellationToken Clear(ref CancellationToken token) { token = default; return default; } static CancellationToken Link(CancellationToken[] values, CancellationToken ignored) => CancellationTokenSource.CreateLinkedTokenSource(values).Token; cancellationToken = Link(tokens, Clear(ref tokens[0]));")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = new Span<CancellationToken>(tokens); static void Clear(ref CancellationToken token) => token = default; Clear(ref alias[0]); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = new Span<CancellationToken>(tokens, 0, 1); alias[0] = default; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    public async Task Reports_Replaced_Parameter(string statements)
    {
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("cancellationToken = cancellationToken;")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; if (context != null) { cancellationToken = saved; } else { cancellationToken = saved; }")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; if (context != null) { if (DateTime.Now.Ticks > 0) { cancellationToken = saved; } else { cancellationToken = saved; } } else { cancellationToken = saved; }")]
    [DataRow("var saved = cancellationToken; cancellationToken = saved;")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; cancellationToken = saved;")]
    [DataRow("if (false) cancellationToken = CancellationToken.None;")]
    [DataRow("var saved = cancellationToken; if (context != null) { cancellationToken = CancellationToken.None; cancellationToken = saved; }")]
    [DataRow("var saved = cancellationToken; cancellationToken = saved; saved = cancellationToken; cancellationToken = saved;")]
    [DataRow("var saved = cancellationToken; if (context != null) { saved = CancellationToken.None; saved = cancellationToken; } cancellationToken = saved;")]
    [DataRow("var saved = CancellationToken.None; if (context != null) { saved = cancellationToken; } else { saved = cancellationToken; } cancellationToken = saved;")]
    [DataRow("var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken); cancellationToken = linked.Token;")]
    [DataRow("var saved = cancellationToken; try { cancellationToken = CancellationToken.None; } finally { cancellationToken = saved; }")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; if (true) cancellationToken = saved;")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; if (false) {} else cancellationToken = saved;")]
    [DataRow("cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(new[] { cancellationToken, CancellationToken.None }).Token;")]
    [DataRow("cancellationToken = CancellationTokenSource.CreateLinkedTokenSource([cancellationToken, CancellationToken.None]).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource([..tokens, CancellationToken.None]).Token;")]
    [DataRow("static CancellationToken Pass(CancellationToken token) => token; cancellationToken = Pass(cancellationToken);")]
    [DataRow("var saved = cancellationToken; (cancellationToken, saved) = (CancellationToken.None, cancellationToken); cancellationToken = saved;")]
    [DataRow("cancellationToken = Pass(cancellationToken);")]
    [DataRow("cancellationToken = cancellationToken.PassToken();")]
    [DataRow("var saved = (cancellationToken, 1); (cancellationToken, _) = saved;")]
    [DataRow("static (CancellationToken, int) PassPair((CancellationToken, int) pair) => pair; var saved = PassPair((cancellationToken, 1)); (cancellationToken, _) = saved;")]
    [DataRow("var saved = (cancellationToken, 1); var copy = saved; (cancellationToken, _) = copy;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = (1, cancellationToken); (_, cancellationToken) = saved;")]
    [DataRow("var saved = (number: 1, token: cancellationToken); cancellationToken = saved.token;")]
    [DataRow("var saved = (cancellationToken, 1); var copy = saved; copy.Item1 = CancellationToken.None; (cancellationToken, _) = saved;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = tokens; alias = new[] { CancellationToken.None }; alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken, cancellationToken }; tokens[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; tokens[0] = cancellationToken; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken, cancellationToken }; if (context != null) tokens[0] = CancellationToken.None; else tokens[1] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("cancellationToken = cancellationToken; var values = new object[] { cancellationToken }; values[0] = values; GC.KeepAlive(values);")]
    [DataRow("var tokens = new[] { cancellationToken }; while (DateTime.Now.Ticks < 0) tokens = [..tokens, cancellationToken]; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("cancellationToken = cancellationToken; object saved = cancellationToken; while (DateTime.Now.Ticks < 0) saved = (saved, 1);")]
    [DataRow("var saved = ((1, cancellationToken), 2); ((_, cancellationToken), _) = saved;")]
    [DataRow("var saved = (cancellationToken, 1); var copy = saved; cancellationToken = CancellationToken.None; (cancellationToken, _) = copy;")]
    [DataRow("var saved = context != null ? (cancellationToken, 1) : (cancellationToken, 2); (cancellationToken, _) = saved;")]
    [DataRow("var tokens = new[] { cancellationToken }; var other = new[] { cancellationToken }; other[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var linked = CancellationTokenSource.CreateLinkedTokenSource(tokens); tokens[0] = CancellationToken.None; cancellationToken = linked.Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = tokens; var linked = CancellationTokenSource.CreateLinkedTokenSource(tokens); alias[0] = CancellationToken.None; cancellationToken = linked.Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = tokens; alias = new[] { cancellationToken }; alias[0] = CancellationToken.None; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; tokens[0] = CancellationToken.None; tokens = new[] { cancellationToken }; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var saved = ((CancellationToken.None, 1), 2); saved.Item1 = (cancellationToken, 1); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = ((CancellationToken.None, 1), 2); (saved.Item1, _) = ((cancellationToken, 1), 2); ((cancellationToken, _), _) = saved;")]
    [DataRow("var saved = ((cancellationToken, 1), (cancellationToken, 2)); saved.Item1 = (CancellationToken.None, 1); cancellationToken = saved.Item2.Item1;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); var pair = saved.Item1; cancellationToken = CancellationToken.None; (cancellationToken, _) = pair;")]
    [DataRow("var saved = ((cancellationToken, 1), 2); saved.Item1 = saved.Item1; ((cancellationToken, _), _) = saved;")]
    [DataRow("var tokens = new CancellationToken[1]; tokens[0] = cancellationToken; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new CancellationToken[int.MaxValue]; tokens[1000000] = cancellationToken; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new CancellationToken[context != null ? 1 : 2]; tokens[0] = cancellationToken; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; GC.KeepAlive(tokens); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; static CancellationToken[] PassArray(CancellationToken[] value) => value; var alias = PassArray(tokens); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;")]
    [DataRow("var tokens = new CancellationToken[1]; static CancellationToken[] PassArray(CancellationToken[] value) { return value; } var alias = PassArray(tokens); alias[0] = cancellationToken; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = tokens.PassArray(); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; static object PassObject(object value) => value; var alias = (CancellationToken[])PassObject(tokens); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;")]
    [DataRow("(CancellationToken Token, int Count) saved = default; saved.Token = cancellationToken; cancellationToken = saved.Token;")]
    [DataRow("((CancellationToken Token, int Count) Pair, int Count) saved = default; saved.Pair.Token = cancellationToken; cancellationToken = saved.Pair.Token;")]
    [DataRow("(CancellationToken Token, int Count) saved = default; (saved.Token, _) = (cancellationToken, 1); (cancellationToken, _) = saved;")]
    [DataRow("(CancellationToken First, CancellationToken Second) saved = default; saved.First = cancellationToken; saved.Second = CancellationToken.None; cancellationToken = saved.First;")]
    [DataRow("(CancellationToken Token, int Count) saved = default; if (context != null) saved.Token = cancellationToken; else saved.Token = cancellationToken; cancellationToken = saved.Token;")]
    [DataRow("(CancellationToken[] Tokens, int Count) saved = default; saved.Tokens = new[] { cancellationToken }; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(saved.Tokens).Token;")]
    [DataRow("static CancellationToken Reset(ref CancellationToken token, CancellationToken saved) { token = default; return saved; } cancellationToken = Reset(ref cancellationToken, cancellationToken);")]
    [DataRow("static CancellationToken Reset(out CancellationToken token, CancellationToken saved) { token = default; return saved; } cancellationToken = Reset(out cancellationToken, cancellationToken);")]
    [DataRow("static CancellationToken Reset(ref CancellationToken token, CancellationToken saved) { token = default; return saved; } var saved = (cancellationToken, 1); cancellationToken = Reset(ref saved.Item1, saved.Item1);")]
    [DataRow("var tokens = new[] { cancellationToken }; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(new ReadOnlySpan<CancellationToken>(tokens)).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = new Span<CancellationToken>(tokens); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(alias).Token;")]
    [DataRow("var tokens = new CancellationToken[1]; var alias = new Span<CancellationToken>(tokens); alias[0] = cancellationToken; cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; _ = new Tuple<CancellationToken[], CancellationToken>(tokens, cancellationToken = tokens[0]);")]
    [DataRow("var tokens = new[] { cancellationToken }; var slice = new Span<CancellationToken>(tokens, 0, 1); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var slice = new ReadOnlySpan<CancellationToken>(tokens, 0, 1); cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(tokens).Token;")]
    [DataRow("var tokens = new[] { cancellationToken }; var alias = new ReadOnlySpan<CancellationToken>(tokens); cancellationToken = alias[0];")]
    public async Task Recognizes_Original_Parameter_Value(string statements)
    {
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Reports_Awaited_And_Returned_Replacement(bool awaited)
    {
        var source = Source("cancellationToken = CancellationToken.None;", "{|#0:Task.Delay(1, cancellationToken)|}");
        if (awaited)
        {
            source = source.Replace("override Task OnBeforeExecuteAsync", "override async Task OnBeforeExecuteAsync")
                .Replace("return {|#0:", "await {|#0:");
        }

        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [Timeout(30000, CooperativeCancellation = true)]
    [DataRow("static object Box((object, int) pair) => pair;", "Box((saved, 1))")]
    [DataRow("static object Box((object, int) pair) { return pair; }", "Box((saved, 1))")]
    [DataRow("static object Box((object, int) pair) => (object)pair;", "Box((saved, 1))")]
    [DataRow("static (object, int) PassPair((object, int) pair) => pair;", "PassPair((saved, 1))")]
    public async Task Tuple_Helper_Boxing_Has_Bounded_Cost(string helper, string expression)
    {
        var statements = $"{helper} cancellationToken = cancellationToken; object saved = cancellationToken; "
            + $"while (DateTime.Now.Ticks < 0) saved = {expression};";
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [Timeout(30000, CooperativeCancellation = true)]
    public async Task Conditional_Copies_Have_Bounded_Cost()
    {
        var statements = "var saved = cancellationToken; "
            + string.Concat(Enumerable.Range(0, 40).Select(i =>
                $"var copy{i} = context != null ? saved : saved; saved = copy{i}; "))
            + "cancellationToken = CancellationToken.None; cancellationToken = saved;";
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    [Timeout(30000, CooperativeCancellation = true)]
    public async Task Many_Task_Calls_Share_Parameter_Analysis()
    {
        var calls = string.Concat(Enumerable.Range(0, 100).Select(_ => "await Task.Delay(1, cancellationToken); "));
        var source = Source("cancellationToken = cancellationToken; " + calls, "Task.Delay(1, cancellationToken)")
            .Replace("override Task OnBeforeExecuteAsync", "override async Task OnBeforeExecuteAsync")
            .Replace("return Task.Delay", "await Task.Delay");
        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    private static string AsyncSource(string statements, string call) => Source(statements, call)
        .Replace("override Task OnBeforeExecuteAsync", "override async Task OnBeforeExecuteAsync")
        .Replace("return " + call, "await " + call);

    private static string Source(string statements, string call) => $$"""
        {{TestSourceConstants.StandardModuleHeader}}
        public class BuildModule : Module<List<string>>
        {
            private static CancellationToken Pass(CancellationToken token) => token;
            private static CancellationToken Replace(CancellationToken token) => CancellationToken.None;
            private static CancellationToken Recurse(CancellationToken token) => Recurse(token);
            private static CancellationToken MutatingRecurse(CancellationToken token)
            {
                token = Pass(token);
                return MutatingRecurse(token);
            }
            {{TestSourceConstants.SimpleAsyncExecuteBody}}
            protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            {
                {{statements}}
                return {{call}};
            }
        }
        public static class TokenExtensions
        {
            public static CancellationToken PassToken(this CancellationToken token) => token;
            public static CancellationToken[] PassArray(this CancellationToken[] tokens) => tokens;
        }
        """;
}
