using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RosylnHello.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosylnHello.Rules
{
    internal class InternalToPublicClassRule : IRefactorRule
    {
        public string Description => "Convert internal classes to public";

        public SyntaxNode Apply(SyntaxNode root)
        {
            var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            var newRoot = root;

            foreach (var cls in classes)
            {
                if (cls.Modifiers.Any(m => m.IsKind(SyntaxKind.InternalKeyword)))
                {
                    var newModifiers = SyntaxFactory.TokenList(
                        cls.Modifiers.Select(m =>
                            m.IsKind(SyntaxKind.InternalKeyword)
                                ? SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(SyntaxFactory.Space)
                                : m)
                    );

                    var newClass = cls.WithModifiers(newModifiers);
                    newRoot = newRoot.ReplaceNode(cls, newClass);
                }
            }

            return newRoot;
        }
    }
}
