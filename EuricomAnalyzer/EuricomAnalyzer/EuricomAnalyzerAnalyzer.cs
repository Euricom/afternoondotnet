using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace EuricomAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class EuricomAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "EuricomAnalyzer";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            //// TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            //// See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            //context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.);

            context.RegisterSyntaxNodeAction(AnalyzeSymbol, SyntaxKind.SimpleMemberAccessExpression);
        }

        private static void AnalyzeSymbol(SyntaxNodeAnalysisContext context)
        {
            if(context.Node is MemberAccessExpressionSyntax syntax
                && syntax.Name.Identifier.Text == "Now"
                && syntax.Expression is IdentifierNameSyntax expression
                && expression.Identifier.Text == "DateTime")
            {
                var diagnostic = Diagnostic.Create(Rule, syntax.Name.GetLocation(), "Use UtcNow");
                context.ReportDiagnostic(diagnostic);
            }
            // TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
            //var propertySymbol = (IPropertySymbol)context.Symbol;

            //// Find just those named type symbols with names containing lowercase letters.
            //if (propertySymbol.Name == "Now")
            //{
            //    // For all such symbols, produce a diagnostic.
            //    var diagnostic = Diagnostic.Create(Rule, propertySymbol.Locations[0], propertySymbol.Name);

            //    context.ReportDiagnostic(diagnostic);
            //}


        }
    }
}
