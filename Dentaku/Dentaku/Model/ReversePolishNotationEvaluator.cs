namespace Dentaku.Model
{
    using System;
    using System.Collections.Generic;
    using Dentaku.Common;
    
    /// <summary>
    /// ニ語長命令
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public delegate double TwoWordInstruction(double x, double y);

    /// <summary>
    /// Dentaku モデル
    /// </summary>
    public class ReversePolishNotationEvaluator
    {
        #region コンストラクタ

        /// <summary>
        /// ctor
        /// </summary>
        public ReversePolishNotationEvaluator()
        {
            // do nothing
        }

        #endregion

        #region 内部データ

        /// <summary>
        /// 演算子 -> 関数
        /// </summary>
        private IReadOnlyDictionary<Operators, TwoWordInstruction> toTwoWordInstructions = new Dictionary<Operators, TwoWordInstruction>()
        {
            {
                Operators.Addition, (x, y) => x + y
            },
            {
                Operators.Substraction, (x, y) => x - y
            },
            {
                Operators.Multiplication, (x, y) => x * y
            },
            {
                Operators.Division, (x, y) => {
                        var ans = x / y;
                        if (Double.IsInfinity(ans)) { throw new DivideByZeroException($"Error: divided by zero ({x} / {y})"); }
                        return ans;
                    }
            },
            {
                Operators.Residue, (x, y) => {
                        var ans = x % y;
                        if (Double.IsInfinity(ans)) { throw new DivideByZeroException($"Error: divided by zero ({x} % {y})"); }
                        return ans;
                    }
            },
        };

        /// <summary>
        /// 文字列 → 演算子
        /// </summary>
        private IReadOnlyDictionary<string, Operators> toOperators = new Dictionary<string, Operators>()
        {
            {
                "+", Operators.Addition
            },
            {
                "-", Operators.Substraction
            },
            {
                "*", Operators.Multiplication
            },
            {
                "/", Operators.Division
            },
            {
                "%", Operators.Residue
            },
        };

        /// <summary>
        /// オペランド用スタック
        /// </summary>
        /// <remarks>メモリ確保に時間を取らせないため, メンバ変数として持つ</remarks>
        private Stack<double> operandStack = new Stack<double>() { };

        /// <summary>
        /// 逆ポーランド記法 デバッグ用
        /// </summary>
        private string rpn_ = "";

        /// <summary>
        /// 計算結果 デバッグ用
        /// </summary>
        private double ans_ = 0;

        #endregion

        #region 処理

        #region 内部処理

        /// <summary>
        /// 演算子か判定する
        /// </summary>
        /// <param name="s">文字列</param>
        /// <returns></returns>
        private bool isOperator(string s)
        {
            Operators op;
            if (!this.toOperators.TryGetValue(s, out op))
            {
                return false;
            }

            return true;
        }

        #endregion

        /// <summary>
        /// 逆ポーランド記法の数式を計算する
        /// </summary>
        /// <param name="rpn">逆ポーランド記法の数式</param>
        /// <returns>計算結果</returns>
        /// <remarks>入力はスペース区切り. また, ニ語長命令にしか対応していない</remarks>
        public double Calculate(string rpn)
        {
            int end = 0;
            int nRPN = rpn.Length;

            while (end < nRPN)
            {
                // 最初の空白を飛ばす
                while ((end < nRPN) && (rpn[end] == ' ')) { end++; }

                var start = end;

                // 最後に空白が続いていた場合の保護
                if (start >= nRPN) { break; }

                while ((end < nRPN) && (rpn[end] != ' ')) { end++; }

                // 処理対象
                var s = rpn.Substring(start, end - start);

                var isOperator = this.isOperator(s);
                if (isOperator)
                {
                    Operators op;
                    if (!this.toOperators.TryGetValue(s, out op))
                    {
                        throw new ArgumentException($"Argument error: unsupported operator = {op}");
                    }

                    var f = this.toTwoWordInstructions[this.toOperators[s]];

                    var operand2 = this.operandStack.Pop();
                    var operand1 = this.operandStack.Pop();

                    var result = f(operand1, operand2);
                    this.operandStack.Push(result);

                    continue;
                }

                double x = double.Parse(s);
                this.operandStack.Push(x);
            }

            var ans = this.operandStack.Pop();

            this.rpn_ = rpn;
            this.ans_ = ans;

            if (this.operandStack.Count > 0)
            {
                throw new ArgumentException($"Internal error: The rest of operand stack = {this.operandStack.Count}");
            }

            return ans;
        }

        #endregion
    }
}
