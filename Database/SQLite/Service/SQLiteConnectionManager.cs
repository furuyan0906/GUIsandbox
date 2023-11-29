using System;
using System.Collections.Generic;
using System.Data.SQLite;
using GUISandbox.DataBase.SQLite.Command;

namespace GUISandbox.DataBase.SQLite.Service;

internal sealed class SQLiteConnectionManager : IDisposable
{
    #region Fields

    private bool isDisposed = false;

    private readonly SQLiteConnection connection;

    private readonly SQLiteConnectionStringBuilder connectionStringBuilder;

    #endregion

    #region Constructors
        
    public SQLiteConnectionManager(string dbFilePath)
    {
        // DataSource = ":memory:"とすると, メモリ上にデータベースを作成する
        this.connectionStringBuilder = new SQLiteConnectionStringBuilder()
        {
            DataSource = dbFilePath,
        };

        this.connection = new SQLiteConnection(this.connectionStringBuilder.ToString());
        this.connection.Open();
    }

    #endregion

    #region Properties

    public string ConnectionString => this.connectionStringBuilder.ConnectionString;

    #endregion

    #region Public Methods

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public string ExecuteCommand(SQLiteCommandWithQuery commandWithQuery)
    {
        var ret = string.Empty;
        using (var cmd = new SQLiteCommand(this.connection))
        {
            cmd.CommandText = commandWithQuery.Command;
            ret = cmd.ExecuteScalar().ToString() ?? throw new InvalidOperationException($"command={commandWithQuery.Command}");
        }

        return ret;
    }

    public void ExecuteCommand(SQLiteCommandWithoutQuery commandWithoutQuery)
    {
        using (var cmd = new SQLiteCommand(this.connection))
        {
            cmd.CommandText = commandWithoutQuery.Command;
            cmd.ExecuteNonQuery();
        }
    }

    public bool TryGetDataReader(SQLiteCommandWithQuery commandWithQuery, out SQLiteDataReader? reader)
    {
        reader = null;
        bool ret = false;

        using (var cmd = new SQLiteCommand(this.connection))
        {
            cmd.CommandText = commandWithQuery.Command;
            reader = cmd.ExecuteReader();
            ret = true;
        }

        return ret;
    }

    #endregion

    #region Private Methods

    private void Dispose(bool disposing)
    {
        if (!this.isDisposed)
        {
            if (disposing)
            {
                this.connection?.Close();
                this.connection?.Dispose();
            }

            this.isDisposed = true;
        }
    }

    #endregion
}
