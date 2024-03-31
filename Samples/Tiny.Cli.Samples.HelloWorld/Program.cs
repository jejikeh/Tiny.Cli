using Tiny.Cli.Core;

var tinyCli = new TinyCli();

tinyCli.Command("hello", "print hello to someone")
    .With("name", "name of someone")
    .With("-h", "show help", TinyArgumentType.Flag)
    .Run(arguments =>
    {
        if (arguments["-h"] == "True")
        {
            Console.WriteLine("This command will print hello to someone");
            
            return;
        }
        
        var name = arguments["name"];
        
        Console.WriteLine($"Hello, {name}!");
    });

tinyCli.Command("help", "show help")
    .Run(_ =>
    {
        Console.WriteLine(tinyCli.Help("Usage: "));
    });

var (message, ok) = tinyCli.Run(args);

if (!ok)
{
    Console.WriteLine(message);
}