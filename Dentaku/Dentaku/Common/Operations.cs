namespace Dentaku.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Dentakuの演算子 一覧
    /// </summary>
    public enum Operators
    {
        /// <summary>
        /// 加算
        /// </summary>
        Addition,
        
        /// <summary>
        /// 引き算
        /// </summary>
        Substraction,

        /// <summary>
        /// 掛け算
        /// </summary>
        Multiplication,

        /// <summary>
        /// 除算
        /// </summary>
        Division,

        /// <summary>
        /// 余り
        /// </summary>
        Residue,

        /// <summary>
        /// 左括弧
        /// </summary>
        LeftBracket,

        /// <summary>
        /// 右括弧
        /// </summary>
        RightBracket,
    }
}
