using System.Diagnostics;

namespace FunctionalTesting;

/// <summary>
/// 
/// </summary>
public class ExecutableRunner : IRunner
{
    private string _command;
    private List<string> _lines;

    public ExecutableRunner(string command)
    {
        _command = command ?? throw new ArgumentNullException(nameof(command));
        _lines = new List<string>();
    }

    public int Lines => _lines.Count;

    public string GetLine(int index) => _lines[index];

    public int Run(string[] args, int timeoutSeconds = 3)
    {
        var p = Process.Start(_command, args);
        p.WaitForExit(timeoutSeconds * 1000);
        return p.ExitCode;
    }
}