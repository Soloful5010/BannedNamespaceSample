using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class SimpleAnalyzer : DiagnosticAnalyzer {
    public readonly static string[] BannedNamespaces = [
        "MNGuiTest.MNGui",
        "ConsoleApp1.MyLib"
    ];

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        id: "BANNMSP001",
        title: "Banned namespace",
        messageFormat: "Namespace {0} can't be used",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
        );

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

    public override void Initialize(AnalysisContext context) {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        // Register action
        //context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        context.RegisterSyntaxNodeAction(AnalyzeUsing, SyntaxKind.UsingDirective);
        context.RegisterSyntaxNodeAction(AnalyzeDeclareNamespace, SyntaxKind.NamespaceDeclaration, SyntaxKind.FileScopedNamespaceDeclaration);
    }

    private static void AnalyzeDeclareNamespace(SyntaxNodeAnalysisContext context) {
        NameSyntax? nameSyn = null;
        if (context.Node is NamespaceDeclarationSyntax nds) {
            nameSyn = nds.Name;
        }
        else if (context.Node is FileScopedNamespaceDeclarationSyntax fsnds) {
            nameSyn = fsnds.Name;
        }

        CheckBannedNameSyntax(context, nameSyn);
    }

    private static void AnalyzeUsing(SyntaxNodeAnalysisContext context) {
        if (context.Node is UsingDirectiveSyntax usingNode) {
            CheckBannedNameSyntax(context, usingNode.Name);
        }
    }

    private static void CheckBannedNameSyntax(SyntaxNodeAnalysisContext context, NameSyntax? nameSyn) {
        if (nameSyn == null) return;

        var name = nameSyn.ToString();
        foreach (var banned in BannedNamespaces) {
            if (name.StartsWith(banned)) {
                var diag = Diagnostic.Create(Rule, nameSyn.GetLocation(), banned);
                context.ReportDiagnostic(diag);
            }
        }
    }

    //private static void AnalyzeSymbol(SymbolAnalysisContext context) {
    //    // ここを適当に書き換える（これはサンプル通りの全部Lowerじゃないクラス名があった場合に警告を出す）
    //    var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

    //    if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower)) {
    //        // Diagnosticを作ってReportDiagnosticに詰める。
    //        var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
    //        context.ReportDiagnostic(diagnostic);
    //    }
    //}
}
