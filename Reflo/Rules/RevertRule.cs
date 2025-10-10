using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RosylnHello.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosylnHello.Rules
{
    internal class RevertRule : IRefactorRule
    {
        public string Description => "Revert last change made to a file";

        public SyntaxNode Apply(SyntaxNode root)
        {
            var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();
            var newRoot = root;

            foreach (var cls in classes)
            {
                if (cls.Identifier.Text.EndsWith("_Backup"))
                {
                    var originalName = cls.Identifier.Text.Replace("_Backup", "");
                    var renamed = cls.WithIdentifier(SyntaxFactory.Identifier(originalName));
                    newRoot = newRoot.ReplaceNode(cls, renamed);
                }
            }
            return newRoot;


        }
    }
}
