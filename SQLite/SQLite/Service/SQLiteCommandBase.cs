using GUISandbox.SQLite.Extensions;

namespace GUISandbox.SQLite.Service;

internal abstract class SQLiteCommandBase
{
    #region Constructors

    protected SQLiteCommandBase(string command)
    {
        command.ThrowArgumentExceptionIfNullOrEmptyOrWhiteSpace(nameof(command));
        this.Command = command;
    }

    #endregion

    #region Properties

    public string Command { get; init; }

    public abstract bool IsNonQuery { get; }

    #endregion
}
