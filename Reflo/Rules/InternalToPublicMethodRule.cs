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
    internal class InternalToPublicMethodRule : IRefactorRule
    {
        public string Description => "Convert internal methods to public";

        public SyntaxNode Apply(SyntaxNode root)
        {
            var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            var newRoot = root;

            foreach (var cls in classes)
            {
                var methods = cls.DescendantNodes().OfType<MethodDeclarationSyntax>();
                var newClass = cls;

                foreach (var method in methods)
                {
                    if (method.Modifiers.Any(m => m.IsKind(SyntaxKind.InternalKeyword)))
                    {
                        var newModifiers = SyntaxFactory.TokenList(
                            method.Modifiers.Select(m =>
                                m.IsKind(SyntaxKind.InternalKeyword)
                                    ? SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTrailingTrivia(SyntaxFactory.Space)
                                    : m)
                        );

                        var newMethod = method.WithModifiers(newModifiers);
                        newClass = newClass.ReplaceNode(method, newMethod);
                    }
                }
                newRoot = newRoot.ReplaceNode(cls, newClass);

            }
            return newRoot;
        }
    }
}
    

