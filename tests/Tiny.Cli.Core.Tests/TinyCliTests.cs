namespace Tiny.Cli.Core.Tests;

public class TinyCliTests
{
    [Fact]
    public void Add_TinyCommand_ShouldAddCommand()
    {
        const string testArgs = "build testProjectName";
        
        var tinyCli = new TinyCli();
        
        tinyCli.Command("build", "Builds the project")
            .With("name", "The name of the project")
            .Run(arguments =>
            {
                Assert.Equal("testProjectName", arguments["name"]);
            });
        
        var (_, ok) = tinyCli.Run(testArgs.Split(' '));
        Assert.True(ok);
    }
    
    [Fact]
    public void Add_TinyCommandWithOption_ShouldAddCommand()
    {
        var tinyCli = new TinyCli();
        
        tinyCli.Command("build", "Builds the project")
            .With("name", "The name of the project")
            .Run(arguments =>
            {
                Assert.Equal("testProjectName", arguments["name"]);
            });
        
        tinyCli.Command("test", "Tests the project")
            .With("-name", "The name of the project", TinyArgumentType.Option)
            .Run(arguments =>
            {
                // @Note: If argument of command is optional, it will be empty string if no value was provided
                Assert.Equal(string.Empty, arguments["-name"]);
            });
        
        var (_, ok2) = tinyCli.Run("test".Split(' '));
        Assert.True(ok2);
    }

    [Fact]
    public void Run_HandleStringInQuotes_ShouldCorrectlyParseString()
    {
        var tinyCli = new TinyCli();
        
        tinyCli.Command("test")
            .With("-test", type: TinyArgumentType.Option)
            .Run(args =>
            {
                Assert.Equal("Hello, World", args["-test"]);
            });
        
        var (_, ok) = tinyCli.Run("test -test \"Hello, World\"".Split(' '));
        Assert.True(ok);
    }
}