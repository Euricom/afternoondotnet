using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using VerifyCS = EuricomAnalyzer.Test.CSharpCodeFixVerifier<
    EuricomAnalyzer.EuricomAnalyzerAnalyzer,
    EuricomAnalyzer.EuricomAnalyzerCodeFixProvider>;

namespace EuricomAnalyzer.Test
{
    [TestClass]
    public class EuricomAnalyzerUnitTest
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task TestMethod1()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task TestMethod2()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
                public DateTime XNow { get; set; } = DateTime.{|#0:Now|};

        }
    }";

            var fixtest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
                public DateTime XNow { get; set; } = DateTime.UtcNow;

        }
    }";

            var expected = VerifyCS.Diagnostic("EuricomAnalyzer").WithLocation(0).WithArguments("Use UtcNow");
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }
    }
}
