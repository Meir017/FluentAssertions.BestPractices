using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace FluentAssertions.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ExceptionShouldThrowWithInnerExceptionAnalyzer : ExceptionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Exceptions.ExceptionShouldThrowWithInnerException;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .WithInnerException() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);

        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new ShouldThrowWhichInnerExceptionShouldBeOfType();
                yield return new ShouldThrowAndInnerExceptionShouldBeOfType();
                yield return new ShouldThrowExactlyAndInnerExceptionShouldBeOfType();
                yield return new ShouldThrowExactlyWhichInnerExceptionShouldBeOfType();
                yield return new ShouldThrowAndInnerExceptionShouldBeAssignableTo();
                yield return new ShouldThrowWhichInnerExceptionShouldBeAssignableTo();
                yield return new ShouldThrowExactlyAndInnerExceptionShouldBeAssignableTo();
                yield return new ShouldThrowExactlyWhichInnerExceptionShouldBeAssignableTo();
            }
        }

        public class ShouldThrowExactlyWhichInnerExceptionShouldBeOfType : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowExactlyWhichInnerExceptionShouldBeOfType() : base(MemberValidator.Should, new MemberValidator("ThrowExactly"), MemberValidator.Which, new MemberValidator("InnerException"), MemberValidator.Should, new MemberValidator("BeOfType"))
            {
            }
        }
        public class ShouldThrowExactlyAndInnerExceptionShouldBeOfType : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowExactlyAndInnerExceptionShouldBeOfType() : base(MemberValidator.Should, new MemberValidator("ThrowExactly"), MemberValidator.And, new MemberValidator("InnerException"), MemberValidator.Should, new MemberValidator("BeOfType"))
            {
            }
        }
        public class ShouldThrowWhichInnerExceptionShouldBeOfType : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowWhichInnerExceptionShouldBeOfType() : base(MemberValidator.Should, new MemberValidator("Throw"), MemberValidator.Which, new MemberValidator("InnerException"), MemberValidator.Should, new MemberValidator("BeOfType"))
            {
            }
        }        
        public class ShouldThrowAndInnerExceptionShouldBeOfType : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowAndInnerExceptionShouldBeOfType() : base(MemberValidator.Should, new MemberValidator("Throw"), MemberValidator.And, new MemberValidator("InnerException"), MemberValidator.Should, new MemberValidator("BeOfType"))
            {
            }
        }

        public class ShouldThrowExactlyAndInnerExceptionShouldBeAssignableTo : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowExactlyAndInnerExceptionShouldBeAssignableTo() : base(MemberValidator.Should, new MemberValidator("ThrowExactly"), MemberValidator.And, new MemberValidator("InnerException"), MemberValidator.Should, new MemberValidator("BeAssignableTo"))
            {
            }
        }
        public class ShouldThrowExactlyWhichInnerExceptionShouldBeAssignableTo : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowExactlyWhichInnerExceptionShouldBeAssignableTo() : base(MemberValidator.Should, new MemberValidator("ThrowExactly"), MemberValidator.Which, new MemberValidator("InnerException"), MemberValidator.Should, new MemberValidator("BeAssignableTo"))
            {
            }
        }
        public class ShouldThrowAndInnerExceptionShouldBeAssignableTo : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowAndInnerExceptionShouldBeAssignableTo() : base(MemberValidator.Should, new MemberValidator("Throw"), MemberValidator.And, new MemberValidator("InnerException"), MemberValidator.Should, new MemberValidator("BeAssignableTo"))
            {
            }
        }        
        public class ShouldThrowWhichInnerExceptionShouldBeAssignableTo : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowWhichInnerExceptionShouldBeAssignableTo() : base(MemberValidator.Should, new MemberValidator("Throw"), MemberValidator.Which, new MemberValidator("InnerException"), MemberValidator.Should, new MemberValidator("BeAssignableTo"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ExceptionShouldThrowWithInnerExceptionCodeFix)), Shared]
    public class ExceptionShouldThrowWithInnerExceptionCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(ExceptionShouldThrowWithInnerExceptionAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            switch (properties.VisitorName)
            {
                case nameof(ExceptionShouldThrowWithInnerExceptionAnalyzer.ShouldThrowWhichInnerExceptionShouldBeOfType):
                case nameof(ExceptionShouldThrowWithInnerExceptionAnalyzer.ShouldThrowExactlyWhichInnerExceptionShouldBeOfType):
                    return ReplaceBeOfTypeInnerException(expression, "Which");
                case nameof(ExceptionShouldThrowWithInnerExceptionAnalyzer.ShouldThrowAndInnerExceptionShouldBeOfType):
                case nameof(ExceptionShouldThrowWithInnerExceptionAnalyzer.ShouldThrowExactlyAndInnerExceptionShouldBeOfType):
                    return ReplaceBeOfTypeInnerException(expression, "And");
                case nameof(ExceptionShouldThrowWithInnerExceptionAnalyzer.ShouldThrowWhichInnerExceptionShouldBeAssignableTo):
                case nameof(ExceptionShouldThrowWithInnerExceptionAnalyzer.ShouldThrowExactlyWhichInnerExceptionShouldBeAssignableTo):
                    return ReplaceBeAssignableToInnerException(expression, "Which");
                case nameof(ExceptionShouldThrowWithInnerExceptionAnalyzer.ShouldThrowAndInnerExceptionShouldBeAssignableTo):
                case nameof(ExceptionShouldThrowWithInnerExceptionAnalyzer.ShouldThrowExactlyAndInnerExceptionShouldBeAssignableTo):
                    return ReplaceBeAssignableToInnerException(expression, "And");
                default:
                    throw new System.InvalidOperationException($"Invalid visitor name - {properties.VisitorName}");
            }
        }

        private ExpressionSyntax ReplaceBeOfTypeInnerException(ExpressionSyntax expression, string whichOrAnd)
            => ReplaceWithInnerException(expression, whichOrAnd, renameFrom: "BeOfType", renameTo: "WithInnerExceptionExactly");

        private ExpressionSyntax ReplaceBeAssignableToInnerException(ExpressionSyntax expression, string whichOrAnd)
            => ReplaceWithInnerException(expression, whichOrAnd, renameFrom: "BeAssignableTo", renameTo: "WithInnerException");
        
        private ExpressionSyntax ReplaceWithInnerException(ExpressionSyntax expression, string whichOrAnd, string renameFrom, string renameTo)
        {
            var replacements = new[]
            {
                NodeReplacement.Remove(whichOrAnd),
                NodeReplacement.Remove("InnerException"),
                NodeReplacement.RemoveOccurrence("Should", occurrence: 2)
            };
            var newExpression = GetNewExpression(expression, replacements);
            var rename = NodeReplacement.RenameAndExtractArguments(renameFrom, renameTo);
            return GetNewExpression(newExpression, rename);
        }
    }
}
