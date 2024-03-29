﻿using System;
using System.Collections.Generic;
using GUISandbox.DataBase.Common.Extensions;

namespace GUISandbox.DataBase.Common.Element;

public class TableElement
{
    #region Constructors

    public TableElement(string name, int value, Version version, IEnumerable<object> collection)
    {
        name.ThrowArgumentExceptionIfNullOrEmptyOrWhiteSpace(nameof(name));
        value.ThrowArgumentExceptionIfNull(nameof(value));
        version.ThrowArgumentExceptionIfNull(nameof(version));
        collection.ThrowArgumentExceptionIfNull(nameof(collection));

        this.Name = name;
        this.Value = value;
        this.Version = version;
        this.Collection = collection;
    }

    #endregion

    #region Properties

    public string Name { get; init; }

    public int Value { get; init; }

    public Version Version { get; init; }

    public IEnumerable<object> Collection { get; init; }

    #endregion

    #region Public Methods

    public override string ToString()
    {
        return $"(key={this.Name}) Value={this.Value}, Version={this.Version}, Collection={this.Collection}";
    }

    #endregion
}
