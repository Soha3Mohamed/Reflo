namespace Reflo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //dotnet run --project "C:\Users\soha mohamed\source\repos\RosylnHello\Reflo\" -- apply InternalToPublic "C:\Users\soha mohamed\source\repos\RosylnHello\TestLibrary"

            if (args.Length == 0)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  reflo apply <RuleName> <Path>");
                Console.WriteLine("  reflo revert <Path>");
                Console.WriteLine("  reflo list-rules");
                return;
            }

            string command = args[0].ToLower();

            switch (command)
            {
                case "apply":
                    if (args.Length < 3)
                    {
                        Console.WriteLine("❌ Missing arguments. Use: reflo apply <RuleName> <Path>");
                        return;
                    }
                    string ruleName = args[1];
                    string projectPath = args[2];
                    RefactorEngine.ApplyRuleByName(ruleName, projectPath);
                    break;

                case "revert":
                    RefactorEngine.RevertAll(args.Length > 1 ? args[1] : Directory.GetCurrentDirectory());
                    break;

                case "list-rules":
                    RefactorEngine.ListRules();
                    break;

                default:
                    Console.WriteLine($"Unknown command '{command}'");
                    break;
            }
        }

    }
}
