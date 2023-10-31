using GUISandbox.DataBase.Common.Element;
using GUISandbox.DataBase.Main.Constants;
using GUISandbox.DataBase.SQLite.Service;

namespace GUISandbox.DataBase.Main;

class MainProgram
{
    #region Fields

    private static readonly string dbFilePath = $"{SQLiteConstants.SQLiteTestDBDiretory}/{SQLiteConstants.SQLiteTestDBName}.db";

    private static readonly SQLiteConnectionService connectionService = new SQLiteConnectionService(dbFilePath);

    private static readonly string tableName = "test_table";

    #endregion

    #region Public Methods

    static void Main()
    {
        var version = SQLiteConnectionService.GetSQLiteVersion();
        Console.WriteLine($"version : {version}");

        SQLiteConnectionService.CreateTable(tableName);

        var elements = new List<SQLiteTableElement>(){};
        for (int i = 0; i < 1; ++i)
        {
            elements.Add(new SQLiteTableElement($"Test{i:D3}", i, new Version(1, 0, i), new List<object>{}));
        }

        SQLiteConnectionService.InsertIntoTable(tableName, elements);
        var tableElements = SQLiteConnectionService.GetTable(tableName);

        foreach (var element in tableElements)
        {
            Console.WriteLine(element);
        }
    }

    #endregion

    #region Properties

    static private SQLiteConnectionService SQLiteConnectionService => connectionService ?? throw new InvalidOperationException(nameof(SQLiteConnectionService));

    #endregion
}
