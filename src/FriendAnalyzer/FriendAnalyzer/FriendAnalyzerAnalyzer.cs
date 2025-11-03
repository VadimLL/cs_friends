//#define LAUNCH_DEBUGGER

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

//using FriendLib;

namespace FriendAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class FriendAnalyzerAnalyzer : DiagnosticAnalyzer
{
    const string OnlyYouAttribute = nameof(OnlyYouAttribute);
    const string OnlyAliasAttribute = nameof(OnlyAliasAttribute);
    const string FriendAliasAttribute = nameof(FriendAliasAttribute);

    // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
    // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
    //private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
    //private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
    //private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
    //private const string Category = "Naming";

    #region DiagnosticAnalyzer implementation 

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
        = ImmutableArray.Create([methodRule, propertyRule, typeRule]);

    public override void Initialize(AnalysisContext context)
    {
#if LAUNCH_DEBUGGER
        if (!System.Diagnostics.Debugger.IsAttached)
        {
            System.Diagnostics.Debugger.Launch();
        }
#endif
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information

        context.RegisterSyntaxNodeAction(analyzeInvocation, SyntaxKind.InvocationExpression);

        context.RegisterSyntaxNodeAction(analyzePropertyAssignment, SyntaxKind.SimpleAssignmentExpression);
        context.RegisterSyntaxNodeAction(analyzePropertyAssignment, SyntaxKind.AddAssignmentExpression);
        context.RegisterSyntaxNodeAction(analyzePropertyAssignment, SyntaxKind.SubtractAssignmentExpression);
        context.RegisterSyntaxNodeAction(analyzePropertyAssignment, SyntaxKind.MultiplyAssignmentExpression);
        context.RegisterSyntaxNodeAction(analyzePropertyAssignment, SyntaxKind.DivideAssignmentExpression);
        context.RegisterSyntaxNodeAction(analyzePropertyAssignment, SyntaxKind.ModuloAssignmentExpression);
        context.RegisterSyntaxNodeAction(analyzePropertyAssignment, SyntaxKind.AndAssignmentExpression);
        context.RegisterSyntaxNodeAction(analyzePropertyAssignment, SyntaxKind.ExclusiveOrAssignmentExpression);
        context.RegisterSyntaxNodeAction(analyzePropertyAssignment, SyntaxKind.OrAssignmentExpression);
        context.RegisterSyntaxNodeAction(analyzePropertyAssignment, SyntaxKind.LeftShiftAssignmentExpression);
        context.RegisterSyntaxNodeAction(analyzePropertyAssignment, SyntaxKind.RightShiftAssignmentExpression);
        context.RegisterSyntaxNodeAction(analyzePropertyAssignment, SyntaxKind.CoalesceAssignmentExpression);

        context.RegisterSyntaxNodeAction(analyzePrefixUnary, SyntaxKind.PreIncrementExpression);
        context.RegisterSyntaxNodeAction(analyzePrefixUnary, SyntaxKind.PreDecrementExpression);

        context.RegisterSyntaxNodeAction(analyzePostfixUnary, SyntaxKind.PostIncrementExpression);
        context.RegisterSyntaxNodeAction(analyzePostfixUnary, SyntaxKind.PostDecrementExpression);
    }

    #endregion DiagnosticAnalyzer implementation

    #region methodRule

#pragma warning disable RS2008 // Enable analyzer release tracking
#pragma warning disable RS1032 // Define diagnostic message correctly
    static readonly DiagnosticDescriptor methodRule = new DiagnosticDescriptor(
        id: "FAM_001",
        title: "Method should be a friend",
        messageFormat: "The '{0}' is not a friend for the '{1}' method.",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "The method should be a friend.");
#pragma warning restore RS2008 // Enable analyzer release tracking
#pragma warning restore RS1032 // Define diagnostic message correctly

    static void analyzeInvocation(SyntaxNodeAnalysisContext context)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;
        if (invocation.ToString().StartsWith("base."))
        {
            return;
        }

        var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation);
        var methodSymbol = symbolInfo.Symbol as IMethodSymbol
            ?? symbolInfo.CandidateSymbols.OfType<IMethodSymbol>().FirstOrDefault();        
        analyzeOperation(context, methodRule, methodSymbol, invocation);
    }

    #endregion methodRule

    #region propertyRule

#pragma warning disable RS2008 // Enable analyzer release tracking
#pragma warning disable RS1032 // Define diagnostic message correctly
    static readonly DiagnosticDescriptor propertyRule = new DiagnosticDescriptor(
        id: "FAP_001",
        title: "Property should be a friend",
        messageFormat: "The '{0}' is not a friend for the '{1}' property.",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "The property should be a friend.");
#pragma warning restore RS2008 // Enable analyzer release tracking
#pragma warning restore RS1032 // Define diagnostic message correctly

    static void analyzePropertyAssignment(SyntaxNodeAnalysisContext context)
    {
        var assignment = (AssignmentExpressionSyntax)context.Node;
        analyzeProperty(context, assignment.Left as MemberAccessExpressionSyntax);
    }

    static void analyzePrefixUnary(SyntaxNodeAnalysisContext context)
    {
        var prefixUnary = (PrefixUnaryExpressionSyntax)context.Node;
        analyzeProperty(context, prefixUnary.Operand as MemberAccessExpressionSyntax);
    }

    static void analyzePostfixUnary(SyntaxNodeAnalysisContext context)
    {
        var postfixUnary = (PostfixUnaryExpressionSyntax)context.Node;
        analyzeProperty(context, postfixUnary.Operand as MemberAccessExpressionSyntax);
    }

    static void analyzeProperty(
        in SyntaxNodeAnalysisContext context,
        in MemberAccessExpressionSyntax? memberAccess)
    {
        if (memberAccess is null)
        {
            return;
        }

        var symbolInfo = context.SemanticModel.GetSymbolInfo(memberAccess);
        var propertySymbol = symbolInfo.Symbol as IPropertySymbol
            ?? symbolInfo.CandidateSymbols.OfType<IPropertySymbol>().FirstOrDefault();
        analyzeOperation(context, propertyRule, propertySymbol, memberAccess);
    }

    #endregion propertyRule

    #region typeRule

#pragma warning disable RS2008 // Enable analyzer release tracking
#pragma warning disable RS1032 // Define diagnostic message correctly
    static readonly DiagnosticDescriptor typeRule = new DiagnosticDescriptor(
        id: "FAT_001",
        title: "Type should be a friend",
        messageFormat: "The '{0}' is not a friend for the '{1}' type.",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Type should be a friend.");
#pragma warning restore RS2008 // Enable analyzer release tracking
#pragma warning restore RS1032 // Define diagnostic message correctly

    #endregion typeRule

    #region attributeRule

#pragma warning disable RS2008 // Enable analyzer release tracking
#pragma warning disable RS1032 // Define diagnostic message correctly
    static readonly DiagnosticDescriptor attributeRule = new DiagnosticDescriptor(
        id: "FAA_001",
        title: "Attributes conflict",
        messageFormat: $"Not allowed to apply '{OnlyYouAttribute}' and '{OnlyAliasAttribute}' attributes at the same time.",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: $"Not allowed to apply '{OnlyYouAttribute}' and '{OnlyAliasAttribute}' attributes at the same time.");
#pragma warning restore RS2008 // Enable analyzer release tracking
#pragma warning restore RS1032 // Define diagnostic message correctly

    #endregion attributeRule

    #region helpers

    static void analyzeOperation(
        in SyntaxNodeAnalysisContext context,
        in DiagnosticDescriptor rule,
        in ISymbol? symbol,
        in SyntaxNode operationNode)
    {
        static OuterInfo? getOuterInfo(
                in SyntaxNodeAnalysisContext context,
                in ISymbol symbol,
                in SyntaxNode operationNode)
        {
            if (operationNode.Parent.SearchInParent(static n => n is MethodDeclarationSyntax)
                is not MethodDeclarationSyntax outerMethod)
            {
                return null;
            }

            if (outerMethod.Parent.SearchInParent(static n => n is ClassDeclarationSyntax)
                is not ClassDeclarationSyntax outerClass)
            {
                return null;
            }

            // scan base types: (instead of StartsWith("base.") and etc.)
            //foreach (INamedTypeSymbol t in context.SemanticModel.GetDeclaredSymbol(outerClass).GetAllBaseTypes())
            //{
            //}

            INamedTypeSymbol? outerClassSymbol = context.SemanticModel.GetDeclaredSymbol(outerClass);
            // allow a call inside the same class:
            if (ReferenceEquals(symbol.ContainingType, outerClassSymbol))
            {
                return null;
            }

            return new OuterInfo(outerMethod, outerClass, outerClassSymbol!);
        }

        static bool analyzeWhenType(
            in SyntaxNodeAnalysisContext context,
            in DiagnosticDescriptor rule,
            in ISymbol symbol,
            in SyntaxNode operationNode,
            IEnumerable<AttributeData> attrsOA)
        {
            var outerInfo = getOuterInfo(context, symbol, operationNode);
            if (outerInfo is null)
            {
                return false;
            }

            (MethodDeclarationSyntax outerMethod,
             ClassDeclarationSyntax outerClass,
             INamedTypeSymbol outerClassSymbol) = outerInfo;

            var types = attrsOA.Select(a => a.ConstructorArguments[0].Value).Cast<INamedTypeSymbol>();
            if (types.All(t => !ReferenceEquals(t, outerClassSymbol)))
            {
                var attractorType = symbol.ContainingType;
                reportDiagnostic(rule, context, operationNode, outerClassSymbol.Name, attractorType.Name);
                return true;
            }

            return false;
        }

        //System.Console.Beep(500, 100);

        if (symbol is null)
        {
            return;
        }

        var attrsT = symbol.ContainingType.GetAttributes()
            .Where(static a => a.AttributeClass?.Name is OnlyYouAttribute);
        if (attrsT.Count() > 0)
        {
            if(analyzeWhenType(context, typeRule, symbol, operationNode, attrsT))
            {
                return;
            }
        }

        var attrsO = symbol is IMethodSymbol
            ? symbol.GetAttributes()
                    .Where(static a => a.AttributeClass?.Name is OnlyYouAttribute or OnlyAliasAttribute)
            : symbol.GetAttributes()
                    .Where(static a => a.AttributeClass?.Name == OnlyYouAttribute);
        if (attrsO.Count() == 0)
        {
            return;
        }

        var outerInfo = getOuterInfo(context, symbol, operationNode);
        if (outerInfo is null)
        {
            return;
        }

        (MethodDeclarationSyntax outerMethod,
         ClassDeclarationSyntax outerClass,
         INamedTypeSymbol? outerClassSymbol) = outerInfo;

        // scan base types: (instead of StartsWith("base.") and etc.)
        //foreach (INamedTypeSymbol t in context.SemanticModel.GetDeclaredSymbol(outerClass).GetAllBaseTypes())
        //{
        //}

        string outerMethodName = outerMethod.Identifier.Text;

        attrsO = attrsO.Where(a => ReferenceEquals(a.ConstructorArguments[0].Value, outerClassSymbol));
        if (attrsO.Count() == 0) // outerClass is not "Only" type
        {
            reportDiagnostic(rule, context, operationNode, symbol.Name, outerMethodName);
            return;
        }

        attrsO = attrsO.Where(static a => a.ConstructorArguments[1].Values.Count() > 0);
        if (attrsO.Count() == 0)
        {
            // => all members of the outerClass is friends (i.e. allowed)
            return;
        }

        var attrsOU = attrsO.Where(static a => a.AttributeClass?.Name == OnlyYouAttribute);
        bool isAllowByOU = true; // is allow (by attrsOU) to invoke our member (symbol)?
        if (attrsOU.SelectMany(static attr => attr.ConstructorArguments[1].Values)
                 .All(v => v.Value?.ToString() != outerMethodName))
        {
            isAllowByOU = false;
        }

        bool isAllowByOA = true;
        do
        {
            if (symbol is not IMethodSymbol)
            {
                isAllowByOA = false;
                break;
            }

            var attrsOA = attrsO.Where(static a => a.AttributeClass?.Name == OnlyAliasAttribute);
            if(attrsOA.Count() == 0)
            {
                isAllowByOA = false;
                break;
            }

            // [OnlyAlias(type, aliases)]
            var aliases = attrsOA
                .SelectMany(attr => attr.ConstructorArguments[1].Values)
                .Select(a => a.Value?.ToString())
                .Where(a => a is not null);
            if (outerMethod.AttributeLists.Count == 0)
            {
                isAllowByOA = false;
                break;
            }

            // [FriendAlias(friendAlias)]
            var attrFA = context.SemanticModel
                .GetDeclaredSymbol(outerMethod)?
                .GetAttributes()
                .Where(static a => a.AttributeClass?.Name is FriendAliasAttribute)
                .FirstOrDefault();
            if (attrFA is null)
            {
                isAllowByOA = false;
                break;
            }

            string friendAlias = attrFA.ConstructorArguments.First().Value!.ToString();
            if (!aliases.Contains(friendAlias))
            {
                isAllowByOA = false;
                break;
            }
        } while (false);

        if (!isAllowByOU && !isAllowByOA)
        {
            reportDiagnostic(rule, context, operationNode, symbol.Name, outerMethodName);
        }
    }

    static void reportDiagnostic(
        in DiagnosticDescriptor rule,
        in SyntaxNodeAnalysisContext context,
        in SyntaxNode node,
        params object?[]? messageArgs)
    {
        var diagnostic = Diagnostic.Create(
            rule,
            Location.Create(node.SyntaxTree, node.Span),
            messageArgs
        );
        context.ReportDiagnostic(diagnostic);
    }

    #endregion helpers
}

file class OuterInfo
{
    public OuterInfo(
        in MethodDeclarationSyntax outerMethod,
        in ClassDeclarationSyntax outerClass,
        in INamedTypeSymbol outerClassSymbol)
    {
        OuterMethod = outerMethod;
        OuterClass = outerClass;
        OuterClassSymbol = outerClassSymbol;
    }

    public void Deconstruct(
        out MethodDeclarationSyntax outerMethod,
        out ClassDeclarationSyntax outerClass,
        out INamedTypeSymbol outerClassSymbol)
    {
        outerMethod = OuterMethod;
        outerClass = OuterClass;
        outerClassSymbol = OuterClassSymbol;
    }

    public MethodDeclarationSyntax OuterMethod { get; }
    public ClassDeclarationSyntax OuterClass { get; }
    public INamedTypeSymbol OuterClassSymbol { get; }
    
}
