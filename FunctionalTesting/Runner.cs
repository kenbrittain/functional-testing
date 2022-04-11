namespace FunctionalTesting;

/// <summary>
/// Represents the ability to run programs for testing. The output will be
/// indexed and made accessible to be used for testing message, etc.
/// </summary>
public interface IRunner
{
    /// <summary>
    /// Returns the number of lines indexed from the console.
    /// </summary>
    public int Lines { get; }
    
    /// <summary>
    /// Gets the indexed text from the console output.
    /// </summary>
    /// <param name="index">The line to get.</param>
    /// <returns>The line of text from the output.</returns>
    /// <exception cref="IndexOutOfRangeException">The requested index does not exist in the output.</exception>
    string GetLine(int index);

    /// <summary>
    /// Run the system under test and indexes the console output.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    /// <para name="timeout">Seconds before timeout when running.</para>
    /// <returns>Returns the exit code from the program run.</returns>
    int Run(string[] args, int timeout);
}