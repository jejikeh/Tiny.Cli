using System.Text;

namespace Tiny.Cli.Core;

public class TinyCli()
{
    private readonly Dictionary<string, TinyCommand> _commands = new Dictionary<string, TinyCommand>();

    public TinyCommand Command(string name, string description = "")
    {
        var command = new TinyCommand(name, description);
        
        _commands.Add(name, command);
        
        return command;
    }

    public (string, bool) Run(string[] args)
    {
        if (args.Length == 0)
        {
            return ("", false);
        }

        if (!_commands.TryGetValue(args[0], out var command))
        {
            return ("expected command, but got: " + args[0], false);
        }
        
        // @Note: Consume the command name
        args = args.Skip(1).ToArray();

        foreach (var arg in command.Arguments)
        {
            switch (arg.Value.Type)
            {
                case TinyArgumentType.Argument:
                    if (args.Length < 1)
                    {
                        return ("expected argument: " + arg.Key, false);
                    }
                    
                    var argument = args[0];
                    
                    arg.Value.WithValue(argument);
                    
                    args = args.Skip(1).ToArray();
                    
                    break;
                case TinyArgumentType.Option:
                    if (!args.Contains(arg.Key))
                    {
                        if (arg.Value.IsRequired)
                        {
                            return ("expected option: " + arg.Key, false);
                        }
                        
                        arg.Value.WithValue(string.Empty);
                        
                        continue;
                    }

                    var argValueIndex = Array.IndexOf(args, arg.Key) + 1;
                    
                    if (argValueIndex >= args.Length)
                    {
                        return ("expected option value: " + arg.Key, false);
                    }
                    
                    arg.Value.WithValue(args[argValueIndex]);
                    
                    args = args.Where(x => x != arg.Key || x != args[argValueIndex]).ToArray();
                    
                    break;
                case TinyArgumentType.Flag:
                    arg.Value.WithValue(args.Contains(arg.Key).ToString());
                    
                    args = args.Where(x => x != arg.Key).ToArray();
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        command.InvokeUserAction();
        
        return ("", true);
    }

    public string Help(string title)
    {
        var sb = new StringBuilder(title);

        sb.AppendLine();

        foreach (var command in _commands)
        {
            sb.AppendLine(command.Value.Help());
            sb.AppendLine();
        }
        
        return sb.ToString();
    }
}