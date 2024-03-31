namespace Tiny.Cli.Core;

public class TinyArgument(
    string name,
    string description,
    TinyArgumentType type = TinyArgumentType.Argument,
    bool isRequired = false)
{
    public string Name { get; } = name;
    public string Description { get; } = description;
    public bool IsRequired { get; } = isRequired;
    public TinyArgumentType Type { get; } = type;
    
    public string? Value { get; private set; }

    public void WithValue(string value)
    {
        Value = value;
    }

    public string Help()
    {
        return $"[{Name}] - {Description} " + (IsRequired ? "(required)" : "(optional)");
    }
}