using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleHelperRefCancellationAnalyzerTests
{
    [TestMethod]
    [DataRow("ref token, ref token", "", "PreserveLinkedArray(ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "PreserveLinkedCollection(ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "PreserveLinkedSpread(ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "PreserveLinkedLocalArray(ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "DiscardLinkedArray(ref first); return first;", true)]
    [DataRow("ref token, ref token", "", "DiscardLinkedLocalArray(ref first); return first;", true)]
    [DataRow("ref token, ref token", "", "PreserveLinkedLocalArrayWrite(ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "PreserveLinkedConditional(ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "DiscardLinkedConditional(ref first); return first;", true)]
    [DataRow("ref token, ref token", "", "PreserveLinked(ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "PreserveLinkedNested(ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "ClearThenLink(ref first); return first;", true)]
    [DataRow("ref token, ref token", "", "DiscardLinked(ref first); return first;", true)]
    [DataRow("ref token, ref token", "", "PreserveViaValueHelper(ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "ClearPairLocal(ref second); return first;", true)]
    [DataRow("ref token, ref token", "", "ClearPairCall(ref first); return first;", true)]
    [DataRow("ref token, ref token", "", "ClearNestedPair(ref second); return first;", true)]
    [DataRow("ref token, ref token", "", "ObservePair(ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "ObserveDiscard(ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "ObserveFreshArray(ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "ObserveFreshArrayNested(ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "ObserveFreshArrayAndClear(ref first); return first;", true)]
    [DataRow("ref token, ref token", "", "Observe(ref first, new int[1]); return first;", false)]
    [DataRow("ref token, ref token", "", "ObserveNested(ref first, new int[1]); return first;", false)]
    [DataRow("ref token, ref token", "", "ObserveAndClear(ref first, new int[1]); return first;", true)]
    [DataRow("ref token, ref token", "", "second = default; return first;", true)]
    [DataRow("second: ref token, first: ref token", "", "second = default; return first;", true)]
    [DataRow("ref token, ref alias", "ref CancellationToken alias = ref token;", "second = default; return first;", true)]
    [DataRow("ref alias, ref token", "ref CancellationToken alias = ref token;", "second = default; return first;", true)]
    [DataRow("ref token, ref alias", "ref CancellationToken alias = ref token; ref CancellationToken chained = ref alias; alias = ref chained;", "second = default; return first;", true)]
    [DataRow("ref token, ref token", "", "return first;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "second = default; return first;", false)]
    [DataRow("second: ref other, first: ref token", "CancellationToken other = default;", "second = default; return first;", false)]
    [DataRow("ref token, ref alias", "CancellationToken other = default; ref CancellationToken alias = ref other;", "second = default; return first;", false)]
    [DataRow("ref token, ref token", "", "var original = first; second = default; return original;", false)]
    [DataRow("ref token, ref token", "", "if (false) second = default; return first;", false)]
    [DataRow("ref token, ref alias", "CancellationToken other = default; ref CancellationToken alias = ref token; alias = ref other;", "second = default; return first;", false)]
    [DataRow("ref token, ref alias", "CancellationToken other = default; ref CancellationToken alias = ref other; alias = ref token;", "second = default; return first;", true)]
    [DataRow("ref token, ref token", "", "second = first; return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = saved; return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; saved = default; second = saved; return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; second = saved; return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; var copy = saved; second = copy; return first;", false)]
    [DataRow("ref token, ref token", "", "second = Forward(ref first, ref second); return first;", true)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref second; return first;", false)]
    [DataRow("ref token, ref token", "", "var stored = first; ref CancellationToken alias = ref stored; alias = default; second = stored; return first;", true)]
    [DataRow("ref token, ref token", "", "var stored = first; ref CancellationToken alias = ref stored; second = stored; return first;", false)]
    [DataRow("ref token, ref token", "", "var stored = first; ref CancellationToken alias = ref stored; alias = default; stored = first; second = stored; return first;", false)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref second; alias = default; return first;", true)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref first; alias = ref second; alias = default; return first;", true)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref second; var saved = first; alias = default; return saved;", false)]
    [DataRow("ref token, ref token", "", "ref CancellationToken alias = ref second; alias = default; var saved = first; return saved;", true)]
    [DataRow("ref token, ref token", "", "int count = 0; ref int alias = ref count; alias = 0; return first;", false)]
    [DataRow("ref token, ref token", "", "ref int count = ref Count; count = 0; return first;", false)]
    [DataRow("ref token, ref token", "", "if (Flag) { var saved = first; second = default; return saved; } return first;", false)]
    [DataRow("ref token, ref token", "", "if (Flag) { second = default; return first; } return first;", true)]
    [DataRow("ref token, ref token", "", "if (Flag) { second = default; } return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; if (Flag) return first; second = default; return saved;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "int count = 0; (first, count) = (first, 1); return first;", false)]
    [DataRow("ref token, ref other", "CancellationToken other = default;", "(first, second) = (second, first); return first;", true)]
    [DataRow("ref token, ref Field", "", "second = default; return first;", false)]
    [DataRow("ref token, ref token", "", "int count = 0; (second, count) = (first, 1); return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; int count = 0; (count, (second, count)) = (1, (saved, 2)); return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; saved = default; int count = 0; (second, count) = (saved, 1); return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; ref CancellationToken alias = ref saved; alias = default; return saved;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; switch (Flag) { case true: second = default; return saved; default: return first; }", false)]
    [DataRow("ref token, ref token", "", "CancellationToken extra = default; ref CancellationToken alias = ref extra; alias = default; return CancellationTokenSource.CreateLinkedTokenSource(first, extra).Token;", false)]
    [DataRow("ref token, ref token", "", "return Identity(ref second, ref first);", false)]
    [DataRow("ref token, ref token", "", "var saved = first; ref CancellationToken alias = ref saved; alias = first; return saved;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; ref CancellationToken alias = ref saved; second = default; alias = first; return saved;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; ref CancellationToken alias = ref saved; alias = default; return CancellationTokenSource.CreateLinkedTokenSource(saved, first).Token;", false)]
    [DataRow("ref token, ref token", "", "switch (Flag) { case true: second = default; goto default; default: return first; }", true)]
    [DataRow("ref token, ref token", "", "switch (Flag) { case true: second = default; break; default: break; } return first;", true)]
    [DataRow("ref token, ref token", "", "return IdentityValue(second = default, first);", true)]
    [DataRow("ref token, ref token", "", "second = CancellationTokenSource.CreateLinkedTokenSource(first, CancellationToken.None).Token; return first;", false)]
    [DataRow("ref token, ref token", "", "second = IdentityValue(default, first); return first;", false)]
    [DataRow("ref token, ref token", "", "second = default; second = IdentityValue(default, first); return first;", true)]
    [DataRow("ref token, ref token", "", "second = Flag ? first : CancellationTokenSource.CreateLinkedTokenSource(first, CancellationToken.None).Token; return first;", false)]
    [DataRow("ref token, ref token", "", "second = Flag ? first : default; return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; return IdentityValue(first, saved);", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; return IdentityValue(value: saved, ignored: first);", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; return IdentityValue(saved, first);", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; return IdentityValue(first, IdentityValue(first, saved));", false)]
    [DataRow("ref token, ref token", "", "var saved = first; ref CancellationToken alias = ref saved; alias = default; return IdentityValue(first, saved);", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; second = IdentityValue(first, saved); return first;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; var result = IdentityValue(first, saved); return result;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; var result = IdentityValue(saved, first); return result;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; var result = Flag ? saved : CancellationTokenSource.CreateLinkedTokenSource(first, saved).Token; return result;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; var result = Flag switch { true => saved, false => CancellationTokenSource.CreateLinkedTokenSource(first, saved).Token }; return result;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; var result = Flag ? saved : IdentityValue(first, saved); return result;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; var result = Flag ? saved : first; return result;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; var result = Flag switch { true => saved, false => first }; return result;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; var result = true ? saved : first; return result;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; var result = Flag ? saved : throw new System.InvalidOperationException(); return result;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; var result = Flag ? saved : (Flag switch { true => saved, false => IdentityValue(first, saved) }); return result;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; return CancellationTokenSource.CreateLinkedTokenSource(new[] { first, saved }).Token;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; return CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[] { first, saved }).Token;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; return CancellationTokenSource.CreateLinkedTokenSource([first, saved]).Token;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; CancellationToken[] inputs = [first, saved]; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; CancellationToken[] inputs = [saved]; return CancellationTokenSource.CreateLinkedTokenSource([first, ..inputs]).Token;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; ref CancellationToken alias = ref saved; alias = default; return CancellationTokenSource.CreateLinkedTokenSource(new[] { first, saved }).Token;", true)]
    [DataRow("ref token, ref token", "", "second = default; return CancellationTokenSource.CreateLinkedTokenSource(new[] { first, CancellationToken.None }).Token;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; return CancellationTokenSource.CreateLinkedTokenSource([first, Flag ? saved : CancellationToken.None]).Token;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; (saved, second) = (first, default(CancellationToken)); return saved;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; (second, saved) = (default(CancellationToken), first); return saved;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; (saved, second) = (first, default(CancellationToken)); return saved;", true)]
    [DataRow("ref token, ref token", "", "Identity(ref second, ref first); return first;", false)]
    [DataRow("ref token, ref token", "", "Identity(ref first, ref second); return first;", false)]
    [DataRow("ref token, ref token", "", "Clear(ref second); return first;", true)]
    [DataRow("ref token, ref token", "", "Preserve(ref second); return first;", false)]
    [DataRow("ref token, ref token", "", "ClearNested(ref second); return first;", true)]
    [DataRow("ref token, ref token", "", "IdentityNested(ref second); return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; CancellationToken[] inputs = [first, saved]; inputs[1] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; CancellationToken[] inputs = [first, saved]; inputs[0] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; CancellationToken[] inputs = [first, saved]; var alias = inputs; alias[1] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; CancellationToken[] inputs = [first, saved]; inputs[1] = default; inputs[1] = saved; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; CancellationToken[] inputs = [first, saved]; var alias = inputs; inputs[1] = default; return CancellationTokenSource.CreateLinkedTokenSource(alias).Token;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; CancellationToken[] inputs = [first, saved]; var alias = inputs; inputs[0] = default; return CancellationTokenSource.CreateLinkedTokenSource(alias).Token;", false)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first]; inputs[0] = default; second = CancellationTokenSource.CreateLinkedTokenSource(inputs).Token; return first;", true)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first]; inputs[0] = first; second = CancellationTokenSource.CreateLinkedTokenSource(inputs).Token; return first;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; second = default; CancellationToken[] inputs = [first, saved]; inputs[Flag ? 0 : 1] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("ref token, ref token", "", "CancellationToken[] prefix = [default, default]; CancellationToken[] inputs = [..prefix, first]; inputs[1] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "CancellationToken[] prefix = [default, default]; CancellationToken[] inputs = [..prefix, first]; inputs[2] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("ref token, ref token", "", "CancellationToken[] prefix = []; CancellationToken[] inputs = [..prefix, first]; inputs[0] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("ref token, ref token", "", "CancellationToken[] prefix = [first, default]; prefix[0] = default; CancellationToken[] inputs = [..prefix, default]; inputs[2] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("ref token, ref token", "", "CancellationToken[] prefix = [first, default]; CancellationToken[] middle = [default, ..prefix]; CancellationToken[] inputs = [..middle, default]; inputs[1] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("ref token, ref token", "", "CancellationToken[] prefix = [first, default]; CancellationToken[] inputs = [default, ..prefix, first]; inputs[1] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "CancellationToken[] prefix = [default, default]; var alias = prefix; CancellationToken[] inputs = [..alias, first]; inputs[1] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "var prefix = Enumerable.Repeat(default(CancellationToken), Count); CancellationToken[] inputs = [..prefix, first]; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "var unknown = Enumerable.Repeat(default(CancellationToken), Count); CancellationToken[] prefix = [..unknown]; CancellationToken[] inputs = [..prefix, first]; inputs[1] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; CancellationToken extra = default; (extra, extra) = (second = default(CancellationToken), saved = first); return saved;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; CancellationToken extra = default; (extra, extra) = (saved = first, second = default(CancellationToken)); return saved;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; CancellationToken extra = default; (extra, extra) = (second = first, saved = first); return saved;", false)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first]; var saved = inputs; inputs = [default]; inputs[0] = default; return CancellationTokenSource.CreateLinkedTokenSource(saved).Token;", false)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first]; var saved = inputs; inputs = [default]; saved[0] = default; return CancellationTokenSource.CreateLinkedTokenSource(saved).Token;", true)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first]; void ClearLater() { inputs[0] = default; } return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first]; Action clearLater = () => inputs[0] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; return Flag switch { true => IdentityValue(second = default, saved), false => first };", false)]
    [DataRow("ref token, ref token", "", "var saved = first; return Flag switch { true => IdentityValue(second = default, first), false => saved };", true)]
    [DataRow("ref token, ref token", "", "var saved = first; return Flag switch { _ when (second = default) == default && Flag => saved, _ => first };", true)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first, default]; if (Flag) inputs[1] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first, default]; if (Flag) inputs[0] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first, first]; if (Flag) inputs[0] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first]; if (Flag) inputs[0] = first; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "var saved = first; CancellationToken[] inputs = [first]; if (Flag) inputs[0] = saved; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [default]; if (Flag) inputs[0] = first; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first]; Other[0] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [default]; Other[0] = first; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first]; Other = inputs; Other[0] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first]; CancellationToken[] other = [default]; if (Flag) other = inputs; other[0] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [default]; CancellationToken[] other = [default]; if (Flag) other = inputs; other[0] = first; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [default]; if (Flag) Other = inputs; Other[0] = first; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first, default]; var alias = inputs; if (Flag) alias[1] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first, default]; switch (Flag) { case true: inputs[1] = default; break; } return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first, default]; if (Flag) inputs[Count] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("ref token, ref token", "", "var saved = first; CancellationToken[] inputs = [first, default]; if (Flag) inputs[0] = default; inputs[0] = saved; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first, default]; inputs[0] = default; if (Flag) inputs[0] = first; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("ref token, ref token", "", "CancellationToken[] prefix = [default]; CancellationToken[] inputs = [..prefix, first]; if (Flag) inputs[0] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first]; if (Flag) inputs[0] = CancellationTokenSource.CreateLinkedTokenSource(first, CancellationToken.None).Token; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [default]; Other = Flag ? inputs : new CancellationToken[1]; Other[0] = first; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first, default]; if (Flag) inputs[0] = default; inputs[0] = first; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first]; var unused = IdentityValue(first, default); Other[0] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first]; Other = Flag ? inputs : new CancellationToken[1]; Other[0] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("ref token, ref token", "", "CancellationToken[] inputs = [first]; var unused = Other; Other[0] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    public async Task Checks_Helper_Ref_Argument_Storage(
        string arguments, string setup, string body, bool reports)
        => await VerifyStorage(arguments, setup, body, reports, firstByRef: true);

    [TestMethod]
    [DataRow("token, ref token", "", "second = default; return first;", false)]
    [DataRow("token, ref Field", "", "second = default; return first;", false)]
    [DataRow("token, ref Field", "", "first = default; return first;", true)]
    [DataRow("token, ref Field", "", "ref CancellationToken alias = ref first; alias = default; return first;", true)]
    [DataRow("token, ref Field", "", "var saved = first; ref CancellationToken alias = ref first; alias = default; return saved;", false)]
    [DataRow("token, ref Field", "", "var saved = first; second = saved; return first;", false)]
    [DataRow("token, ref Field", "", "int count = 0; (second, count) = (default(CancellationToken), 1); return first;", false)]
    [DataRow("token, ref Field", "", "int count = 0; (first, count) = (default(CancellationToken), 1); return first;", true)]
    [DataRow("token, ref Field", "", "var saved = first; int count = 0; (first, count) = (saved, 1); return first;", false)]
    [DataRow("token, ref Field", "", "first = CancellationTokenSource.CreateLinkedTokenSource(first, CancellationToken.None).Token; return first;", false)]
    [DataRow("token, ref Field", "", "first = IdentityValue(default, first); return first;", false)]
    [DataRow("token, ref Field", "", "first = default; first = CancellationTokenSource.CreateLinkedTokenSource(first, CancellationToken.None).Token; return first;", true)]
    [DataRow("token, ref Field", "", "return Flag ? first : CancellationTokenSource.CreateLinkedTokenSource(Forward(first, ref second), first).Token;", false)]
    [DataRow("token, ref Field", "", "return Link(first, 2);", false)]
    [DataRow("token, ref Field", "", "return Mutual(first, 2);", false)]
    [DataRow("token, ref Field", "", "first = IdentityValue(first, default); return first;", true)]
    [DataRow("token, ref Field", "", "first = Flag ? first : CancellationTokenSource.CreateLinkedTokenSource(first, CancellationToken.None).Token; return first;", false)]
    [DataRow("token, ref Field", "", "first = Flag ? first : default; return first;", true)]
    [DataRow("token, ref Field", "", "first = true ? first : default; return first;", false)]
    [DataRow("token, ref Field", "", "first = Flag ? first : throw new System.InvalidOperationException(); return first;", false)]
    [DataRow("token, ref Field", "", "first = Flag switch { true => first, false => CancellationTokenSource.CreateLinkedTokenSource(first, CancellationToken.None).Token }; return first;", false)]
    [DataRow("token, ref Field", "", "first = Flag switch { true => first, false => default }; return first;", true)]
    [DataRow("token, ref Field", "", "first = true switch { true => first, false => default }; return first;", false)]
    [DataRow("token, ref Field", "", "first = Flag switch { true => first, false => throw new System.InvalidOperationException() }; return first;", false)]
    [DataRow("token, ref Field", "", "first = Flag ? first : (Flag switch { true => first, false => CancellationTokenSource.CreateLinkedTokenSource(first, CancellationToken.None).Token }); return first;", false)]
    [DataRow("token, ref Field", "", "first = CancellationTokenSource.CreateLinkedTokenSource(new[] { CancellationToken.None, first }).Token; return first;", false)]
    [DataRow("token, ref Field", "", "first = CancellationTokenSource.CreateLinkedTokenSource([CancellationToken.None, first]).Token; return first;", false)]
    [DataRow("token, ref Field", "", "first = default; first = CancellationTokenSource.CreateLinkedTokenSource(new[] { CancellationToken.None, first }).Token; return first;", true)]
    [DataRow("token, ref Field", "", "CancellationToken[] inputs = [first]; CancellationToken[] alias = []; (alias = inputs)[0] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("token, ref Field", "", "CancellationToken[] inputs = [first]; var alias = IdentityArray(inputs); alias[0] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", true)]
    [DataRow("token, ref Field", "", "CancellationToken[] inputs = [first]; CancellationToken[] alias = []; (alias = new CancellationToken[1])[0] = default; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    [DataRow("token, ref Field", "", "CancellationToken[] inputs = [first]; var alias = IdentityArray(inputs); alias[0] = first; return CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;", false)]
    public async Task Checks_Helper_Private_Value_Parameter_Storage(
        string arguments, string setup, string body, bool reports)
        => await VerifyStorage(arguments, setup, body, reports, firstByRef: false);

    private static async Task VerifyStorage(string arguments, string setup, string body, bool reports, bool firstByRef)
    {
        var call = "Task.Delay(1, snapshot)";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}

            public class BuildModule : Module<List<string>>
            {
                private static int Count;
                private static bool Flag;
                private static CancellationToken Field;
                private static CancellationToken[] Other = new CancellationToken[1];

                private static CancellationToken Forward({{(firstByRef ? "scoped ref " : "")}}CancellationToken first, scoped ref CancellationToken second)
                {
                    {{body}}
                }

                private static CancellationToken Identity(scoped ref CancellationToken ignored, scoped ref CancellationToken value) => value;

                private static void Observe(scoped ref CancellationToken value, int[] counts) => counts[0] = 1;
                private static void ObserveNested(scoped ref CancellationToken value, int[] counts) => Observe(ref value, counts);
                private static void ObserveAndClear(scoped ref CancellationToken value, int[] counts)
                {
                    counts[0] = 1;
                    value = default;
                }

                private static (CancellationToken, int) GetPair() => (default, 0);
                private static void ClearPairLocal(scoped ref CancellationToken value)
                {
                    var pair = GetPair();
                    int count;
                    (value, count) = pair;
                }
                private static void ClearPairCall(scoped ref CancellationToken value) => (value, _) = GetPair();
                private static void ClearNestedPair(scoped ref CancellationToken value)
                {
                    var pair = (0, GetPair());
                    int count;
                    (_, (value, count)) = pair;
                }
                private static void ObserveDiscard(scoped ref CancellationToken value)
                {
                    var pair = GetPair();
                    int count;
                    (_, count) = pair;
                }
                private static void ObserveFreshArray(scoped ref CancellationToken value)
                {
                    CancellationToken[] scratch = [default];
                    scratch[0] = default;
                }
                private static void ObserveFreshArrayNested(scoped ref CancellationToken value) => ObserveFreshArray(ref value);
                private static void ObserveFreshArrayAndClear(scoped ref CancellationToken value)
                {
                    ObserveFreshArray(ref value);
                    value = default;
                }
                private static void ObservePair(scoped ref CancellationToken value)
                {
                    int first, second;
                    (first, second) = (1, 2);
                }

                private static void PreserveLinkedArray(scoped ref CancellationToken value)
                    => value = CancellationTokenSource.CreateLinkedTokenSource(new[] { value, CancellationToken.None }).Token;
                private static void PreserveLinkedCollection(scoped ref CancellationToken value)
                    => value = CancellationTokenSource.CreateLinkedTokenSource([value, CancellationToken.None]).Token;
                private static void PreserveLinkedSpread(scoped ref CancellationToken value)
                    => value = CancellationTokenSource.CreateLinkedTokenSource([.. new[] { value }, CancellationToken.None]).Token;
                private static void PreserveLinkedLocalArray(scoped ref CancellationToken value)
                {
                    CancellationToken[] inputs = [value, CancellationToken.None];
                    value = CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;
                }
                private static void DiscardLinkedArray(scoped ref CancellationToken value)
                    => value = CancellationTokenSource.CreateLinkedTokenSource(new[] { CancellationToken.None }).Token;
                private static void DiscardLinkedLocalArray(scoped ref CancellationToken value)
                {
                    CancellationToken[] inputs = [value];
                    inputs[0] = default;
                    value = CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;
                }
                private static void PreserveLinkedLocalArrayWrite(scoped ref CancellationToken value)
                {
                    CancellationToken[] inputs = [default];
                    inputs[0] = value;
                    value = CancellationTokenSource.CreateLinkedTokenSource(inputs).Token;
                }

                private static void PreserveLinked(scoped ref CancellationToken value)
                    => value = CancellationTokenSource.CreateLinkedTokenSource(value, CancellationToken.None).Token;
                private static void PreserveLinkedConditional(scoped ref CancellationToken value)
                    => value = Flag ? value : CancellationTokenSource.CreateLinkedTokenSource(value, CancellationToken.None).Token;
                private static void DiscardLinkedConditional(scoped ref CancellationToken value) => value = Flag ? value : default;
                private static void PreserveLinkedNested(scoped ref CancellationToken value) => PreserveLinked(ref value);
                private static void ClearThenLink(scoped ref CancellationToken value)
                {
                    value = default;
                    PreserveLinked(ref value);
                }
                private static void DiscardLinked(scoped ref CancellationToken value)
                    => value = CancellationTokenSource.CreateLinkedTokenSource(default(CancellationToken), CancellationToken.None).Token;
                private static void PreserveViaValueHelper(scoped ref CancellationToken value) => value = IdentityValue(default, value);

                private static void Clear(scoped ref CancellationToken value) => value = default;
                private static void Preserve(scoped ref CancellationToken value) => value = value;
                private static void ClearNested(scoped ref CancellationToken value) => Clear(ref value);
                private static void IdentityNested(scoped ref CancellationToken value) => Identity(ref value, ref value);

                private static CancellationToken[] IdentityArray(CancellationToken[] value) => value;

                private static CancellationToken IdentityValue(CancellationToken ignored, CancellationToken value) => value;

                private static CancellationToken Link(CancellationToken token, int remaining)
                    => remaining == 0 ? token : CancellationTokenSource.CreateLinkedTokenSource(Link(token, remaining - 1), token).Token;

                private static CancellationToken Mutual(CancellationToken token, int remaining)
                    => remaining == 0 ? token : CancellationTokenSource.CreateLinkedTokenSource(MutualOther(token, remaining - 1), token).Token;

                private static CancellationToken MutualOther(CancellationToken token, int remaining) => Mutual(token, remaining);


                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context, CancellationToken cancellationToken)
                {
                    var token = cancellationToken;
                    {{setup}}
                    var snapshot = Forward({{arguments}});
                    for (var i = 0; i < 2; i++)
                        await {{(reports ? "{|#0:" + call + "|}" : call)}};
                    return null!;
                }
            }
            """;

        if (reports)
        {
            await VerifyCS.VerifyAnalyzerAsync(source,
                VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                    .WithLocation(0).WithArguments("Delay"));
        }
        else
        {
            await VerifyCS.VerifyAnalyzerAsync(source);
        }
    }
}
