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
            #region Parse a string of c# code to tree of nodes and printing it
            //    Console.WriteLine("Hello, World!");
            //    // 1) Some sample C# code as text
            //    string code = @"
            //public class Person 
            //{
            //    public string Name;
            //    public void SayHello() 
            //    {
            //        Console.WriteLine(""Hello "" + Name);
            //    }
            //}";

            //    // 2) Parse the code into a SyntaxTree
            //    SyntaxTree tree = CSharpSyntaxTree.ParseText(code);

            //    // 3) Get the root node (the top of the tree)
            //    SyntaxNode root = tree.GetRoot();

            //    // 4) Print the syntax tree
            //    Console.WriteLine(root.ToFullString());

            //    // Extra: Print the structure (tree format)
            //    Console.WriteLine("\n--- Tree Structure ---");
            //    Console.WriteLine(root);
            //    foreach (var node in root.DescendantNodes())
            //    {
            //        Console.WriteLine($"{node.Kind()} --> {node}");
            //    }

            #endregion

            #region Filtering the tree and focusing on classes only and printing its names and modifiers

            //    string code = @"
            //public class Person 
            //{
            //    public string Name;
            //    public void SayHello() 
            //    {
            //        Console.WriteLine(""Hello "" + Name);
            //    }
            //}

            //internal class Car
            //{
            //    public string Model;
            //}";

            //    // Parse the code into a tree
            //    SyntaxTree tree = CSharpSyntaxTree.ParseText(code);

            //    // Get the root node
            //    var root = tree.GetRoot();

            //    // Step 1: Find only the class declarations
            //    var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

            //    Console.WriteLine("Classes I found:");
            //    foreach (var cls in classes)
            //    {
            //        Console.WriteLine($"- {cls.Identifier.Text} (modifier: {string.Join(", ", cls.Modifiers)})");
            //    }

            #endregion

            //till now i was parsing pre-written string that is a c# code but the next region is parsing a real .cs file

            #region Parsing a Real .cs file

            //var filePath = @"C:\Users\soha mohamed\source\repos\RosylnHello\RosylnHello\Employee.cs";
            //var codeInFile = File.ReadAllText(filePath);

            //SyntaxTree tree = CSharpSyntaxTree.ParseText(codeInFile);

            //var root = tree.GetRoot();
            //var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

            //Console.WriteLine("Classes I found:");

            ////this line filters only the internal classes by checking if any of the modifiers of the class is internal
            ////iskind is a method that checks if the syntax kind of the modifier is internal keyword
            //var InternalClasses = classes.Where(c => c.Modifiers.Any(m => m.IsKind(SyntaxKind.InternalKeyword))).ToList();
            //foreach (var cls in InternalClasses)
            //{

            //    Console.WriteLine($"- {cls.Identifier.Text} (modifier: {string.Join(", ", cls.Modifiers)})");
            //}


            #endregion

            #region A Whole Solution or Project

            //string projectPath = @"C:\Users\soha mohamed\source\repos\Inventra";

            //foreach (var file in Directory.GetFiles(projectPath, "*.cs", SearchOption.AllDirectories))
            //{
            //    string code = File.ReadAllText(file);
            //    SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            //    var root = tree.GetRoot();

            //    var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

            //    foreach (var cls in classes)
            //    {
            //        Console.WriteLine($"{Path.GetFileName(file)} -> {cls.Identifier.Text} ({string.Join(", ", cls.Modifiers)})");
            //    }
            //}
            #endregion

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


            Console.WriteLine("Welcome to The Code Refactor Tool!: Reflo");
            string rangeChoice = AskForRange();

            if (rangeChoice == "1") Console.Write("Enter file path: ");
            else Console.Write("Enter folder path: ");
            string path = Console.ReadLine();

            string target = AskForTarget();
            string ruleChoice = AskForRule();

            Console.WriteLine($"\nYou selected:");
            Console.WriteLine($"📁 Range: {GetRangeName(rangeChoice)}");
            Console.WriteLine($"🎯 Target: {GetTargetName(target)}");
            Console.WriteLine($"⚙️ Rule: {GetRuleName(ruleChoice)}");
            Console.Write("\nProceed? (y/n): ");
            if (Console.ReadLine()?.ToLower() != "y") return;

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
            Console.WriteLine("[1] Internal → Public");
            Console.WriteLine("[2] Add Author Comment");
            Console.WriteLine("[3] Rename Variable Prefix");
            Console.Write("👉 Your choice: ");
            return Console.ReadLine();
        }
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


        public static class RuleFactory
        {
            public static IRefactorRule GetRule(string choice)
            {
                return choice switch
                {
                    "1" => new InternalToPublicRule(),
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
                                Path.GetDirectoryName(file),
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

