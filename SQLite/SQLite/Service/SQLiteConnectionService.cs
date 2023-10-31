using GUISandbox.SQLite.Extensions;
using System;
using System.Collections.Generic;

namespace GUISandbox.SQLite.Service;

public class SQLiteConnectionService
{
    #region Fields

    private readonly SQLiteConnectionManager connectionManager;

    private readonly SQLiteCommandFactory commandFactory;

    #endregion

    #region Constructors

    public SQLiteConnectionService(string dbFilePath)
    {
        this.connectionManager = new SQLiteConnectionManager(dbFilePath);
        this.commandFactory = new SQLiteCommandFactory();
    }

    #endregion

    #region Public Methods

    public Version GetSQLiteVersion()
    {
        var command = this.commandFactory.CreateVersionGetCommand();
        var version = this.connectionManager.ExecuteCommand(command);

        return new Version(version);
    }

    public void CreateTable(string tableName)
    {
        tableName.ThrowArgumentExceptionIfNullOrEmptyOrWhiteSpace(nameof(tableName));

        var command = this.commandFactory.CreateTableCreationCommand(tableName);
        this.connectionManager.ExecuteCommand(command);
    }

    public IReadOnlyList<SQLiteTableElement> GetTable(string tableName)
    {
        var command = this.commandFactory.CreateTableReadCommnad(tableName);
        this.connectionManager.TryGetDataReader(command, out var reader);

        if (reader is null)
        {
            throw new Exception(nameof(reader));
        }

        var tableElements = new List<SQLiteTableElement>();

        // SQLiteDataReader.Disposeメソッドを確実に呼ぶため, usingステートメントでラッピングする
        using (reader)
        {
            while (reader.Read())
            {
                var name = reader[SQLiteCommandFactory.NameKey]?.ToString() ?? throw new ArgumentException($"key={SQLiteCommandFactory.NameKey}");
                var value = reader[SQLiteCommandFactory.ValueKey]?.ToString() ?? throw new ArgumentException($"key={SQLiteCommandFactory.ValueKey}");
                var version = reader[SQLiteCommandFactory.VersionKey]?.ToString() ?? throw new ArgumentException($"key={SQLiteCommandFactory.VersionKey}");

                var tableElement = new SQLiteTableElement(name, int.Parse(value), new Version(version), new List<object>());
                tableElements.Add(tableElement);
            }

            reader.Close();
        }

        return tableElements.AsReadOnly();
    }

    public void InsertIntoTable(string tableName, IReadOnlyList<SQLiteTableElement> elements)
    {
        var command = this.commandFactory.CreateDataInsertionCommnad(tableName, elements);
        this.connectionManager.ExecuteCommand(command);
    }

    #endregion
}
