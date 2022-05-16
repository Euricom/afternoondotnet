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
        [DataRow("DateTime")]
        [DataRow("DateTimeOffset")]
        public async Task TestMethod2(string typeName)
        {
            var test = $@"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {{
        class TypeName
        {{   
                public {typeName} XNow {{ get; set; }} = {typeName}.{{|#0:Now|}};

        }}
    }}";

            var fixtest = $@"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {{
        class TypeName
        {{   
                public {typeName} XNow {{ get; set; }} = {typeName}.UtcNow;

        }}
    }}";

            var expected = VerifyCS.Diagnostic("EuricomAnalyzer").WithLocation(0).WithArguments("Use UtcNow");
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        [DataRow("DateTime")]
        [DataRow("DateTimeOffset")]
        public async Task TestMethod3(string typeName)
        {
            var test = $@"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {{
        class {typeName}
        {{   
            public System.{typeName} XNow {{ get; set; }} = System.{typeName}.{{|#0:Now|}};

        }}
    }}";

            var fixtest = $@"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {{
        class {typeName}
        {{   
            public System.{typeName} XNow {{ get; set; }} = System.{typeName}.UtcNow;

        }}
    }}";

            var expected = VerifyCS.Diagnostic("EuricomAnalyzer").WithLocation(0).WithArguments("Use UtcNow");
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        [DataRow("DateTime")]
        [DataRow("DateTimeOffset")]
        public async Task TestMethod4(string typeName)
        {
            var test = $@"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;
    using Hidden = System.{typeName};

    namespace ConsoleApplication1
    {{
        class {typeName}
        {{   
            public Hidden XNow {{ get; set; }} = Hidden.{{|#0:Now|}};

        }}
    }}";

            var fixtest = $@"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;
    using Hidden = System.{typeName};

    namespace ConsoleApplication1
    {{
        class {typeName}
        {{   
            public Hidden XNow {{ get; set; }} = Hidden.UtcNow;

        }}
    }}";

            var expected = VerifyCS.Diagnostic("EuricomAnalyzer").WithLocation(0).WithArguments("Use UtcNow");
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }
    }
}
