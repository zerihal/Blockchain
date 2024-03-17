using BlockchainTestApp.Interfaces;
using BlockchainTestApp.RunTests;
using BlockchainUtils;
using BlockchainUtils.Validation;
using System.Text;

namespace BlockchainTestApp
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Blockchain Test App");
            Console.WriteLine("-------------------");
            Run(true);
        }

        private static void ShowMenu()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Select an action:");
            sb.AppendLine("1: Run basic hash test");
            sb.AppendLine("2: Run PoW test");
            sb.AppendLine("3: Run transaction test");
            sb.AppendLine("4: Run custom test");
            sb.AppendLine("5: Change hash algorithm");
            sb.AppendLine("6: Change mine difficulty");
            sb.AppendLine("7: Exit");

            Console.WriteLine(sb.ToString());
        }

        private static void Run(bool showMenu)
        {
            ShowMenu();

            IRunTest? runTest = null;
            var selection = Console.ReadKey();
            Console.WriteLine();

            switch (selection.KeyChar) 
            {
                case '1':
                    runTest = new BasicHashTest();
                    runTest.Run([]);

                    // The blockchain should be valid at this point so confirm that now
                    if (BlockchainHelper.IsValidBlockchain(runTest.RunTestBlockchain, out _))
                    {
                        Console.WriteLine("Blockchain is valid");

                        // Tamper with a Block
                        #pragma warning disable CS8604 // Possible null reference argument.
                        TamperValidate.Tamper(runTest.RunTestBlockchain);
                        #pragma warning restore CS8604 // Possible null reference argument.
                    }

                    break;

                case '2':
                    runTest = new PoWTest();
                    runTest.Run([]);
                    break;

                case '3':
                    runTest = new TransactionTest();
                    runTest.Run([]);
                    break;

                case '4':
                    runTest = new CustomTest();
                    runTest.Run([]);
                    break;

                case '5':
                    var changedHash = ChangeHash();
                    break;

                case '6':
                    Console.WriteLine($"\nCurrent mine difficulty: {BlockchainSettings.MineDifficulty}\nEnter new difficulty:\n");
                    var difficulty = Console.ReadKey();

                    if (int.TryParse(difficulty.KeyChar.ToString(), out int newDifficulty))
                        BlockchainSettings.MineDifficulty = newDifficulty;

                    Console.WriteLine($"\nMine difficulty set to: {BlockchainSettings.MineDifficulty}");

                    break;

                case '7':
                    runTest = new ExitTest();
                    break;
            }

            var nextAction = runTest != null ? runTest.RunAction : RunAction.Continue;

            if (nextAction == RunAction.Exit)
                Environment.Exit(0);
            else
                Run(nextAction == RunAction.ContinueShowMenu ? true : false);
        }

        private static bool ChangeHash()
        {
            Console.WriteLine($"\nCurrent hash algorithm is {Enum.GetName(typeof(HashAlorithmImp), BlockchainSettings.BlockchainHashAlgorithm)}\n");
            Console.WriteLine("Select new hash algorithm:\n1: SHA256\n2: SHA1\n3: SHA384\n4: SHA512\n");

            var selection = Console.ReadKey();

            switch (selection.KeyChar)
            {
                case '1':
                    BlockchainSettings.BlockchainHashAlgorithm = HashAlorithmImp.SHA256;
                    break;

                case '2':
                    BlockchainSettings.BlockchainHashAlgorithm = HashAlorithmImp.SHA1;
                    break;

                case '3':
                    BlockchainSettings.BlockchainHashAlgorithm = HashAlorithmImp.SHA384;
                    break;

                case '4':
                    BlockchainSettings.BlockchainHashAlgorithm = HashAlorithmImp.SHA512;
                    break;
            }

            Console.WriteLine($"\nNew hash algorithm is {Enum.GetName(typeof(HashAlorithmImp), BlockchainSettings.BlockchainHashAlgorithm)}");
            return true;
        }
    }
}
