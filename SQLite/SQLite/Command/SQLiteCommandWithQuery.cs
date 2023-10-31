namespace GUISandbox.SQLite.Service;

internal sealed class SQLiteCommandWithQuery : SQLiteCommandBase
{
    #region Constructors

    public SQLiteCommandWithQuery(string command)
        : base(command)
    {
    }

    #endregion

    #region Properties

    public override bool IsNonQuery => false;

    #endregion
}
