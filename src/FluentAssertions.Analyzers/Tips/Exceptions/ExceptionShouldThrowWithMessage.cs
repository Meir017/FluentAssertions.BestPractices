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
    public class ExceptionShouldThrowWithMessageAnalyzer : ExceptionAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.Exceptions.ExceptionShouldThrowWithMessage;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .WithMessage() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);

        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new ShouldThrowExactlyWhichMessageShouldContain();
                yield return new ShouldThrowWhichMessageShouldContain();
                yield return new ShouldThrowExactlyAndMessageShouldContain();
                yield return new ShouldThrowAndMessageShouldContain();
                yield return new ShouldThrowExactlyWhichMessageShouldBe();
                yield return new ShouldThrowWhichMessageShouldBe();
                yield return new ShouldThrowExactlyAndMessageShouldBe();
                yield return new ShouldThrowAndMessageShouldBe();
                yield return new ShouldThrowExactlyWhichMessageShouldStartWith();
                yield return new ShouldThrowWhichMessageShouldStartWith();
                yield return new ShouldThrowExactlyAndMessageShouldStartWith();
                yield return new ShouldThrowAndMessageShouldStartWith();
                yield return new ShouldThrowExactlyWhichMessageShouldEndWith();
                yield return new ShouldThrowWhichMessageShouldEndWith();
                yield return new ShouldThrowExactlyAndMessageShouldEndWith();
                yield return new ShouldThrowAndMessageShouldEndWith();
            }
        }

        public class ShouldThrowExactlyWhichMessageShouldContain : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowExactlyWhichMessageShouldContain() : base(MemberValidator.Should, new MemberValidator("ThrowExactly"), MemberValidator.Which, new MemberValidator("Message"), MemberValidator.Should, new MemberValidator("Contain"))
            {
            }
        }
        public class ShouldThrowWhichMessageShouldContain : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowWhichMessageShouldContain() : base(MemberValidator.Should, new MemberValidator("Throw"), MemberValidator.Which, new MemberValidator("Message"), MemberValidator.Should, new MemberValidator("Contain"))
            {
            }
        }
        public class ShouldThrowExactlyAndMessageShouldContain : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowExactlyAndMessageShouldContain() : base(MemberValidator.Should, new MemberValidator("ThrowExactly"), MemberValidator.And, new MemberValidator("Message"), MemberValidator.Should, new MemberValidator("Contain"))
            {
            }
        }
        public class ShouldThrowAndMessageShouldContain : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowAndMessageShouldContain() : base(MemberValidator.Should, new MemberValidator("Throw"), MemberValidator.And, new MemberValidator("Message"), MemberValidator.Should, new MemberValidator("Contain"))
            {
            }
        }
        public class ShouldThrowExactlyWhichMessageShouldBe : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowExactlyWhichMessageShouldBe() : base(MemberValidator.Should, new MemberValidator("ThrowExactly"), MemberValidator.Which, new MemberValidator("Message"), MemberValidator.Should, new MemberValidator("Be"))
            {
            }
        }
        public class ShouldThrowWhichMessageShouldBe : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowWhichMessageShouldBe() : base(MemberValidator.Should, new MemberValidator("Throw"), MemberValidator.Which, new MemberValidator("Message"), MemberValidator.Should, new MemberValidator("Be"))
            {
            }
        }
        public class ShouldThrowExactlyAndMessageShouldBe : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowExactlyAndMessageShouldBe() : base(MemberValidator.Should, new MemberValidator("ThrowExactly"), MemberValidator.And, new MemberValidator("Message"), MemberValidator.Should, new MemberValidator("Be"))
            {
            }
        }
        public class ShouldThrowAndMessageShouldBe : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowAndMessageShouldBe() : base(MemberValidator.Should, new MemberValidator("Throw"), MemberValidator.And, new MemberValidator("Message"), MemberValidator.Should, new MemberValidator("Be"))
            {
            }
        }
        public class ShouldThrowExactlyWhichMessageShouldStartWith : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowExactlyWhichMessageShouldStartWith() : base(MemberValidator.Should, new MemberValidator("ThrowExactly"), MemberValidator.Which, new MemberValidator("Message"), MemberValidator.Should, new MemberValidator("StartWith"))
            {
            }
        }
        public class ShouldThrowWhichMessageShouldStartWith : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowWhichMessageShouldStartWith() : base(MemberValidator.Should, new MemberValidator("Throw"), MemberValidator.Which, new MemberValidator("Message"), MemberValidator.Should, new MemberValidator("StartWith"))
            {
            }
        }
        public class ShouldThrowExactlyAndMessageShouldStartWith : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowExactlyAndMessageShouldStartWith() : base(MemberValidator.Should, new MemberValidator("ThrowExactly"), MemberValidator.And, new MemberValidator("Message"), MemberValidator.Should, new MemberValidator("StartWith"))
            {
            }
        }
        public class ShouldThrowAndMessageShouldEndWith : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowAndMessageShouldEndWith() : base(MemberValidator.Should, new MemberValidator("Throw"), MemberValidator.And, new MemberValidator("Message"), MemberValidator.Should, new MemberValidator("EndWith"))
            {
            }
        }
        public class ShouldThrowExactlyWhichMessageShouldEndWith : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowExactlyWhichMessageShouldEndWith() : base(MemberValidator.Should, new MemberValidator("ThrowExactly"), MemberValidator.Which, new MemberValidator("Message"), MemberValidator.Should, new MemberValidator("EndWith"))
            {
            }
        }
        public class ShouldThrowWhichMessageShouldEndWith : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowWhichMessageShouldEndWith() : base(MemberValidator.Should, new MemberValidator("Throw"), MemberValidator.Which, new MemberValidator("Message"), MemberValidator.Should, new MemberValidator("EndWith"))
            {
            }
        }
        public class ShouldThrowExactlyAndMessageShouldEndWith : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowExactlyAndMessageShouldEndWith() : base(MemberValidator.Should, new MemberValidator("ThrowExactly"), MemberValidator.And, new MemberValidator("Message"), MemberValidator.Should, new MemberValidator("EndWith"))
            {
            }
        }
        public class ShouldThrowAndMessageShouldStartWith : FluentAssertionsCSharpSyntaxVisitor
        {
            public ShouldThrowAndMessageShouldStartWith() : base(MemberValidator.Should, new MemberValidator("Throw"), MemberValidator.And, new MemberValidator("Message"), MemberValidator.Should, new MemberValidator("StartWith"))
            {
            }
        }
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ExceptionShouldThrowWithMessageCodeFix)), Shared]
    public class ExceptionShouldThrowWithMessageCodeFix : FluentAssertionsCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(ExceptionShouldThrowWithMessageAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            switch (properties.VisitorName)
            {
                case nameof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowWhichMessageShouldContain):
                case nameof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowExactlyWhichMessageShouldContain):
                    return ReplaceContainMessage(expression, "Which");
                case nameof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowAndMessageShouldContain):
                case nameof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowExactlyAndMessageShouldContain):
                    return ReplaceContainMessage(expression, "And");
                case nameof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowWhichMessageShouldBe):
                case nameof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowExactlyWhichMessageShouldBe):
                    return ReplaceBeMessage(expression, "Which");
                case nameof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowAndMessageShouldBe):
                case nameof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowExactlyAndMessageShouldBe):
                    return ReplaceBeMessage(expression, "And");
                case nameof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowWhichMessageShouldStartWith):
                case nameof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowExactlyWhichMessageShouldStartWith):
                    return ReplaceStartWithMessage(expression, "Which");
                case nameof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowAndMessageShouldStartWith):
                case nameof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowExactlyAndMessageShouldStartWith):
                    return ReplaceStartWithMessage(expression, "And");
                case nameof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowWhichMessageShouldEndWith):
                case nameof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowExactlyWhichMessageShouldEndWith):
                    return ReplaceEndWithMessage(expression, "Which");
                case nameof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowAndMessageShouldEndWith):
                case nameof(ExceptionShouldThrowWithMessageAnalyzer.ShouldThrowExactlyAndMessageShouldEndWith):
                    return ReplaceEndWithMessage(expression, "And");
                default:
                    throw new System.InvalidOperationException($"Invalid visitor name - {properties.VisitorName}");
            }
        }

        private ExpressionSyntax ReplaceContainMessage(ExpressionSyntax expression, string whichOrAnd)
            => ReplaceWithMessage(expression, whichOrAnd, renameMethod: "Contain", prefix: "*", postfix: "*");

        private ExpressionSyntax ReplaceBeMessage(ExpressionSyntax expression, string whichOrAnd)
        {
            var replacements = new[]
            {
                NodeReplacement.Remove(whichOrAnd),
                NodeReplacement.Remove("Message"),
                NodeReplacement.RemoveOccurrence("Should", occurrence: 2),
                NodeReplacement.Rename("Be", "WithMessage")
            };
            return GetNewExpression(expression, replacements);
        }

        private ExpressionSyntax ReplaceStartWithMessage(ExpressionSyntax expression, string whichOrAnd)
            => ReplaceWithMessage(expression, whichOrAnd, renameMethod: "StartWith", postfix: "*");

        private ExpressionSyntax ReplaceEndWithMessage(ExpressionSyntax expression, string whichOrAnd)
            => ReplaceWithMessage(expression, whichOrAnd, renameMethod: "EndWith", prefix: "*");

        private ExpressionSyntax ReplaceWithMessage(ExpressionSyntax expression, string whichOrAnd, string renameMethod, string prefix = "", string postfix = "")
        {
            var replacements = new[]
            {
                NodeReplacement.Remove(whichOrAnd),
                NodeReplacement.Remove("Message"),
                NodeReplacement.RemoveOccurrence("Should", occurrence: 2)
            };
            var newExpression = GetNewExpression(expression, replacements);
            var rename = NodeReplacement.RenameAndExtractArguments(renameMethod, "WithMessage");
            newExpression = GetNewExpression(newExpression, rename);

            ArgumentSyntax newArgument = null;
            switch (rename.Arguments[0].Expression)
            {
                case IdentifierNameSyntax identifier:
                    newArgument = SF.Argument(SF.ParseExpression($"$\"{prefix}{{{identifier.Identifier.Text}}}{postfix}\""));
                    break;
                case LiteralExpressionSyntax literal:
                    newArgument = SF.Argument(SF.ParseExpression($"\"{prefix}{literal.Token.ValueText}{postfix}\""));
                    break;
            }

            var replacement = NodeReplacement.WithArguments("WithMessage", rename.Arguments.Replace(rename.Arguments[0], newArgument));
            return GetNewExpression(newExpression, replacement);
        }
    }
}
