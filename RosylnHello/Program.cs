using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RosylnHello.Interfaces;
using RosylnHello.Rules;

namespace RosylnHello
{
    internal class Program
    {
        static void Main(string[] args)
        {

            #region changing internal to public
            //string projectPath = @"C:\Users\soha mohamed\source\repos\RosylnHello";

            //foreach (var file in Directory.GetFiles(projectPath, "*.cs", SearchOption.AllDirectories))
            //{
            //    // Skip auto-generated or designer files
            //    if (file.Contains("GlobalUsings.g.cs") || file.Contains(".Designer.cs"))
            //        continue;

            //    string code = File.ReadAllText(file);
            //    SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            //    var root = tree.GetRoot();

            //    // Get all class declarations
            //    var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

            //    var newRoot = root;
            //    int count = 0;

            //    foreach (var cls in classes)
            //    {
            //        // Check if class is internal
            //        if (cls.Modifiers.Any(m => m.IsKind(SyntaxKind.InternalKeyword)))
            //        {
            //            // Create new modifier list: replace internal → public
            //            var newModifiers = SyntaxFactory.TokenList(
            //                cls.Modifiers.Select(m =>
            //                    m.IsKind(SyntaxKind.InternalKeyword)
            //                        ? SyntaxFactory.Token(SyntaxKind.PublicKeyword).WithTriviaFrom(m)
            //                        : m
            //                ));

            //            // Create new class node with updated modifiers
            //            var newClass = cls.WithModifiers(newModifiers);

            //            // Replace old class node with new one
            //            newRoot = newRoot.ReplaceNode(cls, newClass);
            //            count++;

            //            Console.WriteLine($"Changed class: {cls.Identifier.Text} in {Path.GetFileName(file)}");
            //        }
            //    }

            //    // Only write a new file if any classes were changed
            //    if (count > 0)
            //    {
            //        var newFilePath = Path.Combine(
            //            Path.GetDirectoryName(file)! + "/RefactorOutput/",
            //            Path.GetFileNameWithoutExtension(file) + "_Modified.cs"
            //        );

            //        File.WriteAllText(newFilePath, newRoot.ToFullString());
            //        Console.WriteLine($"✔ Modified file saved: {Path.GetFileName(newFilePath)}");
            //    }
            //}

            //Console.WriteLine("\nAll done! 🚀");
            #endregion
            string confirm;
            string rangeChoice;
            string path;
            string target;
            string ruleChoice;
            do
            {
                Console.WriteLine("Welcome to The Code Refactor Tool!: Reflo");
                rangeChoice = AskForRange();

                if (rangeChoice == "1")
                    Console.Write("Enter file path: ");
                else
                    Console.Write("Enter folder path: ");
                    path = Console.ReadLine();

                   target = AskForTarget();
                   ruleChoice = AskForRule();

                Console.WriteLine($"\nYou selected:");
                Console.WriteLine($"📁 Range: {GetRangeName(rangeChoice)}");
                Console.WriteLine($"🎯 Target: {GetTargetName(target)}");
                Console.WriteLine($"⚙️ Rule: {GetRuleName(ruleChoice)}");

                Console.Write("\nProceed? (y/n): ");
                confirm = Console.ReadLine()?.ToLower();

                if (confirm == "n")
                {
                    Console.Clear();
                    Console.WriteLine("Let's try again...\n");
                }

            } while (confirm != "y");





            // Load rule
            var rule = RuleFactory.GetRule(ruleChoice);
            if (rule == null)
            {
                Console.WriteLine("❌ Invalid rule choice.");
                return;
            }

            // Collect target files
            var files = FileCollector.GetFiles(rangeChoice, path);
            if (!files.Any())
            {
                Console.WriteLine("⚠️ No .cs files found at that location.");
                return;
            }

            Console.WriteLine($"\nFound {files.Count()} file(s). Applying rule...\n");

            // Run the engine
            RefactorEngine.ApplyRule(rule, files);

            Console.WriteLine("✅ Refactoring complete!");
        }


        #region Get User Input
        public static string AskForRange()
        {

            Console.WriteLine("\n1️⃣ Choose Range:");
            Console.WriteLine("[1] Single File");
            Console.WriteLine("[2] Whole Folder");
            Console.WriteLine("[3] Entire Solution");
            Console.Write("👉 Your choice: ");
            return Console.ReadLine();
        }

        public static string AskForTarget()
        {
            Console.WriteLine("\n1️⃣ Choose Target:");
            Console.WriteLine("[1] Classes");
            Console.WriteLine("[2] Methods");
            Console.WriteLine("[3] Variables");
            Console.Write("👉 Your choice: ");
            return Console.ReadLine();
        }
        public static string AskForRule()
        {
            Console.WriteLine("\n1️⃣ Choose Range:");
            Console.WriteLine("[1] Internal → Public (classes)");
            Console.WriteLine("[2] Internal → Public (methods)");
            Console.WriteLine("[3] Rename Variable Prefix");
            Console.Write("👉 Your choice: ");
            return Console.ReadLine();
        } 
        #endregion


        #region Switch User Input
        static string GetRangeName(string choice) => choice switch
        {
            "1" => "Single File",
            "2" => "Whole Folder",
            "3" => "Entire Solution",
            _ => "Unknown"
        };

        static string GetTargetName(string choice) => choice switch
        {
            "1" => "Classes",
            "2" => "Methods",
            "3" => "Variables",
            _ => "Unknown"
        };

        static string GetRuleName(string choice) => choice switch
        {
            "1" => "Internal → Public",
            "2" => "Add Author Comment",
            "3" => "Rename Variable Prefix",
            _ => "Unknown"
        };

        #endregion

        public static class RuleFactory
        {
            public static IRefactorRule GetRule(string choice)
            {
                return choice switch
                {
                    "1" => new InternalToPublicClassRule(),
                    "2" => new InternalToPublicMethodRule(),
                    _ => null
                };
            }
        }

        public static class FileCollector
        {
            public static IEnumerable<string> GetFiles(string rangeChoice, string path)
            {
                if (rangeChoice == "1" && File.Exists(path))
                    return new List<string> { path };

                if (rangeChoice == "2" && Directory.Exists(path))
                    return Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);
                if (rangeChoice == "3" && Directory.Exists(path))
                    return Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);

                return new List<string>();
            }
        }

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

                        // Apply the rule (returns modified root)
                        var newRoot = rule.Apply(root);

                        if (newRoot != null && !newRoot.IsEquivalentTo(root))
                        {
                            var modifiedPath = Path.Combine(
                                Path.GetDirectoryName(file)!,
                                Path.GetFileNameWithoutExtension(file) + "_Modified.cs"
                            );

                            File.WriteAllText(modifiedPath, newRoot.ToFullString());
                            Console.WriteLine($"✔ Modified: {Path.GetFileName(modifiedPath)}");

                   
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Error processing {file}: {ex.Message}");
                    }
                }
            }
        }
    }
}

