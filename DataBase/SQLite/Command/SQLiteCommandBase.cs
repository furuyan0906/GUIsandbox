using GUISandbox.DataBase.Common.Extensions;

namespace GUISandbox.DataBase.SQLite.Command;

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
