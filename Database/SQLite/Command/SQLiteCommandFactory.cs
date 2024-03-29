﻿using System.Collections.Generic;
using GUISandbox.DataBase.Common.Element;
using GUISandbox.DataBase.Common.Extensions;

namespace GUISandbox.DataBase.SQLite.Command;

internal class SQLiteCommandFactory
{
    #region Constructors
    
    public SQLiteCommandFactory()
    {
    }

    #endregion

    public static string NameKey => nameof(TableElement.Name).ToLowerInvariant();

    public static string ValueKey => nameof(TableElement.Value).ToLowerInvariant();

    public static string VersionKey => nameof(TableElement.Version).ToLowerInvariant();

    #region Public Methods

    public SQLiteCommandWithQuery CreateVersionGetCommand()
    {
        var command = "SELECT sqlite_version()";
        return new SQLiteCommandWithQuery(command);
    }

    public SQLiteCommandWithoutQuery CreateTableCreationCommand(string tableName)
    {
        tableName.ThrowArgumentExceptionIfNullOrEmptyOrWhiteSpace(nameof(tableName));

        var commnad = $"CREATE TABLE IF NOT EXISTS {tableName}(" +
                      $"ID INTEGER PRIMARY KEY AUTOINCREMENT," +           
                      $"{NameKey} TEXT ," +
                      $"{ValueKey} INTEGER NOT NULL ," +
                      $"{VersionKey} TEXT NOT NULL)";
        return new SQLiteCommandWithoutQuery(commnad);
    }

    public SQLiteCommandWithQuery CreateTableReadCommnad(string tableName)
    {
        tableName.ThrowArgumentExceptionIfNullOrEmptyOrWhiteSpace(nameof(tableName));

        var commad = @$"SELECT * FROM {tableName}";
        return new SQLiteCommandWithQuery(commad);
    }

    public SQLiteCommandWithoutQuery CreateDataInsertionCommnad(string tableName, IReadOnlyList<TableElement> elements)
    {
        tableName.ThrowArgumentExceptionIfNullOrEmptyOrWhiteSpace(nameof(tableName));

        var command = $"INSERT INTO {tableName}({NameKey}, {ValueKey}, {VersionKey}) VALUES";
        foreach (var element in elements)
        {
            command += @$" ('{element.Name}', {element.Value}, '{element.Version}'),";
        }
        command = command.TrimEnd(',') + ";";
                      
        return new SQLiteCommandWithoutQuery(command);
    }

    #endregion
}
