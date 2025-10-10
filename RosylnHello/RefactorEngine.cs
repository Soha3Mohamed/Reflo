using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RosylnHello.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RosylnHello
{
    public static class RefactorEngine
    {
        public static void ApplyRule(IRefactorRule rule, IEnumerable<string> filePaths)
        {
            foreach (var file in filePaths)
            {
                try
                {
                    string code = File.ReadAllText(file);
                    var tree = CSharpSyntaxTree.ParseText(code);
                    var root = tree.GetRoot();

                    var newRoot = rule.Apply(root);

                    if (newRoot != null && !newRoot.IsEquivalentTo(root))
                    {
                        // ✅ Check if this is a revert rule
                        bool isRevert = rule.GetType().Name.Contains("Revert", StringComparison.OrdinalIgnoreCase);

                        if (isRevert)
                        {
                            // Example: Backup\Employee.cs → ../Employee.cs
                            string originalPath = Path.Combine(
                                Directory.GetParent(Path.GetDirectoryName(file)!)!.FullName,
                                Path.GetFileName(file)
                            );

                            File.WriteAllText(originalPath, newRoot.ToFullString());
                            Console.WriteLine($"♻ Reverted: {Path.GetFileName(originalPath)}");
                        }
                        else
                        {
                            // 🔹 Normal forward rule (like internal → public)
                            var backupDir = Path.Combine(Path.GetDirectoryName(file)!, "Backup");
                            Directory.CreateDirectory(backupDir);

                            var backupPath = Path.Combine(backupDir, Path.GetFileName(file));

                            if (!File.Exists(backupPath))
                            {
                                File.Copy(file, backupPath);
                            }

                            File.WriteAllText(file, newRoot.ToFullString());
                            Console.WriteLine($"✔ Updated: {Path.GetFileName(file)} (backup saved)");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error processing {file}: {ex.Message}");
                }
            }
        }

        public static void RevertAll(string projectRoot)
        {
            string backupDir = Path.Combine(projectRoot, "Backup");
            if (!Directory.Exists(backupDir))
            {
                Console.WriteLine("⚠ No backup folder found.");
                return;
            }

            var backupFiles = Directory.GetFiles(backupDir, "*.cs", SearchOption.AllDirectories);

            foreach (var backupFile in backupFiles)
            {
                string originalName = Path.GetFileName(backupFile); // e.g. Employee.cs
                string originalFile = Path.Combine(projectRoot, originalName);

                // ✅ Restore original file from backup text
                var text = File.ReadAllText(backupFile);

                // ✅ Parse and rename class inside the file if needed
                var tree = CSharpSyntaxTree.ParseText(text);
                var root = tree.GetRoot();

                var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
                foreach (var cls in classes)
                {
                    if (cls.Identifier.Text.EndsWith("_Backup"))
                    {
                        var renamed = cls.WithIdentifier(SyntaxFactory.Identifier(cls.Identifier.Text.Replace("_Backup", "")));
                        root = root.ReplaceNode(cls, renamed);
                    }
                }
                File.WriteAllText(originalFile, root.ToFullString());

                Console.WriteLine($"♻ Restored: {originalName} from backup.");
            }

            Console.WriteLine("✅ Revert complete!");
        }

        //C:\Users\soha mohamed\source\repos\RosylnHello\TestLibrary\


        //        var excludedFolders = new[] { "Backup", "bin", "obj" };
        //if (excludedFolders.Any(folder => file.Contains(folder, StringComparison.OrdinalIgnoreCase)))
        //    continue;

    }
}
