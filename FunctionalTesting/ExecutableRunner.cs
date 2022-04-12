using System.Diagnostics;
using System.Text;

namespace FunctionalTesting;

/// <summary>
/// 
/// </summary>
public class ExecutableRunner : IRunner
{
    private string _command;
    private List<string> _output;
    private List<string> _errors;
    private int _exitCode;

    public ExecutableRunner(string command)
    {
        _command = command ?? throw new ArgumentNullException(nameof(command));
        _output = new List<string>();
        _errors = new List<string>();
        _exitCode = 0;
    }

    public int ExitCode => _exitCode;
    
    public int Lines => _output.Count;

    public int Errors => _errors.Count;

    public string GetError(int index) => _errors[index];

    public string GetLine(int index) => _output[index];

    /// <summary>
    /// Run the command with no arguments and wait indefinitely,
    /// </summary>
    /// <returns>The exit code from the command.</returns>
    public int Run()
    {
        var n = Run(Array.Empty<string>(), -1);
        return n;
    }
    
    
    public int Run(string[] args, int timeoutSeconds)
    {
        _output.Clear();
        _errors.Clear();
        
        var p = new Process();

        p.StartInfo.CreateNoWindow = true;
        
        // Capture output and error console text locally.
        p.OutputDataReceived += (sender, eventArgs) => _output.Add(eventArgs.Data ?? "\n");
        p.ErrorDataReceived += (sender, eventArgs) => _errors.Add(eventArgs.Data ?? "\n");

        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.RedirectStandardOutput = true;
        
        // Add in the args before we run the command.
        p.StartInfo.FileName = _command;
        foreach (var arg in args)
        {
            p.StartInfo.ArgumentList.Add(arg);
        }

        try
        {
            p.Start();

            // Required to actually get the events.
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();

            //var timeoutMillis = timeoutSeconds * 1000;
            var timeoutMillis = -1;
            var exited = p.WaitForExit(timeoutMillis);
            if (exited == false)
            {
                p.Kill();
                throw new Exception("Did not exit");
            }

            _exitCode = p.ExitCode;
            return _exitCode;
        }
        catch (SystemException e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            p.Close();
        }
    }
}