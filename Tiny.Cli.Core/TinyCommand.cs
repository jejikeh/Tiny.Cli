using System.Text;

namespace Tiny.Cli.Core;

public class TinyCommand(string name, string description)
{
    public string Name { get; } = name;
    public string Description { get; } = description;
    
    public Dictionary<string, TinyArgument> Arguments { get; } = new Dictionary<string, TinyArgument>();

    private Action<Dictionary<string, string>>? _action;
    
    public TinyCommand With(string name, string description = "", TinyArgumentType type = TinyArgumentType.Argument, bool isRequired = false)
    {
        var option = new TinyArgument(name, description, type, isRequired);
        
        Arguments.Add(name, option);
        
        return this;
    }

    public void Run(Action<Dictionary<string, string>> action)
    {
        _action = action;
    }

    public void InvokeUserAction()
    {
        if (Arguments.Any(arg => arg.Value.Value == null) || _action is null)
        {
            return;
        }

        var arguments = Arguments.ToDictionary(key => key.Key, arg => arg.Value.Value!);

        _action(arguments);
    }

    public string Help()
    {
        var sb = new StringBuilder();
        
        sb.AppendLine($"[{Name}] - {Description}");

        foreach (var arg in Arguments)
        {
            sb.AppendLine("\t" + arg.Value.Help());
        }
        
        return sb.ToString();
    }
}