namespace GUISandbox.SQLite.Service;

internal sealed class SQLiteCommandWithoutQuery : SQLiteCommandBase
{
    #region Constructors

    public SQLiteCommandWithoutQuery(string command)
        : base(command)
    {
    }

    #endregion

    #region Properties

    public override bool IsNonQuery => true;

    #endregion
}
