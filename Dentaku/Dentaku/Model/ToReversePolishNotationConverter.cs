namespace Dentaku.Model
{
    using System;
    using System.Collections.Generic;


    public class ToReversePolishNotationConverter
    {
        #region コンストラクタ

        /// <summary>
        /// ctor
        /// </summary>
        public ToReversePolishNotationConverter()
        {

        }

        #endregion

        #region データ

        /// <summary>
        /// 各項目の優先度
        /// </summary>
        /// <remarks>値が大きいほど優先度が高い</remarks>
        private static readonly IReadOnlyDictionary<string, int> toPriority = new Dictionary<string, int>()
        {
            // 優先度0 : 足し算, 引き算
            { "+", 0 },

            // 優先度1 : 引き算
            { "-", 1 },

            // 優先度2 : 掛け算, 割り算, 余り
            { "*", 2 },
            { "/", 2 },
            { "%", 2 },

            // 優先度3 : 左括弧, 右括弧
            { "(", 3 },
            { ")", 3 },

            // 優先度4 : 数値, 文字式
            // 限定できないため定義しない
        };

        /// <summary>
        /// 優先度の最大値
        /// </summary>
        private static readonly int MaxPriority = ToReversePolishNotationConverter.toPriority["("] + 1;

        /// <summary>
        /// 括弧が現れた回数
        /// </summary>
        /// <remarks>左括弧で+1, 右括弧で-1</remarks>
        private int cntBracket = 0;

        /// <summary>
        /// 一時キャッシュ用キュー
        /// </summary>
        /// <remarks>メモリ確保に時間を取らせないため, メンバ変数として持つ</remarks>
        private Queue<string> queue = new Queue<string>() { };

        /// <summary>
        /// 一時キャッシュ用スタック
        /// </summary>
        /// <remarks>メモリ確保に時間を取らせないため, メンバ変数として持つ</remarks>
        private Stack<(string Operator, int Priority)> stack = new Stack<(string Operator, int Priority)>() { };

        /// <summary>
        /// 中置記法の数式 デバッグ用
        /// </summary>
        private string infix_ = "";

        /// <summary>
        /// 逆ポーランド記法 デバッグ用
        /// </summary>
        private string rpn_ = "";

        #endregion

        #region 処理

        #region 内部処理

        /// <summary>
        /// pri1がpri2より優先度が高いか判定する
        /// </summary>
        /// <param name="pri1">評価対象の優先度</param>
        /// <param name="pri2">比較対象の優先度</param>
        /// <returns></returns>
        private bool isPriorityGreater(int priority1, int priority2)
        {
            return priority1 > priority2;
        }

        /// <summary>
        /// 数値or文字式か否かを判定する
        /// </summary>
        /// <param name="s">文字列</param>
        /// <returns></returns>
        private bool isValue(string s)
        {
            int priority;
            if (ToReversePolishNotationConverter.toPriority.TryGetValue(s, out priority))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 無視対象の演算子か判定する
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool isIgnore(string s)
        {
            return (s == "(") || (s == ")");
        }

        /// <summary>
        /// 優先度を取得する
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private int getPriority(string s)
        {
            int priority;
            if (!ToReversePolishNotationConverter.toPriority.TryGetValue(s, out priority))
            {
                // 文字式にも対応する
                priority = ToReversePolishNotationConverter.MaxPriority;
            }

            return priority + this.cntBracket * ToReversePolishNotationConverter.MaxPriority;
        }

        #endregion

        /// <summary>
        /// 中置記法の数式を逆ポーランド記法に変換する
        /// </summary>
        /// <param name="infix">中置記法の数式</param>
        /// <returns>逆ポーランド記法の数式</returns>
        /// <remarks>入力, 出力ともにスペース区切りである</remarks>
        public string Convert(string infix)
        {
            int end = 0;
            int ninfix = infix.Length;

            // 優先度に基づいて要素を並び変える
            while (end < infix.Length)
            {
                // 次の先頭まで移動する
                while ((end < ninfix) && (infix[end] == ' ')) { end++; }

                var start = end;

                // 最後に空白が続いていた場合の保護
                if (start >= ninfix) { break; }

                // 空白に当たるまでインデックスを進める
                while ((end < ninfix) && (infix[end] != ' ')){ end++; }
                
                var op = infix.Substring(start, end - start);

                // 数字, 文字式ならそのままキューに入れる
                if (this.isValue(op))
                {
                    this.queue.Enqueue(op);
                }
                else
                {
                    (string Operator, int Priority) cur = (op, this.getPriority(op));

                    // 計算の優先順位を考慮するため, 括弧の数を記録する
                    this.cntBracket += System.Convert.ToInt32(cur.Operator == "(") - System.Convert.ToInt32(cur.Operator == ")");

                    // 空なら入れるだけ
                    if (this.stack.Count == 0)
                    {
                        this.stack.Push(cur);
                        continue;
                    }

                    var prev = this.stack.Pop();

                    // 前の演算子の方が優先度が高ければ, 計算の優先順位を保つためにStackにあるアイテムを全てキューに入れる
                    // 括弧の数に応じた下駄を履かせているため, 括弧がいくつ登場しても計算の優先順位は保たれる
                    if (this.isPriorityGreater(prev.Priority, cur.Priority))
                    {
                        // 下の処理のために一度スタックに戻す
                        this.stack.Push(prev);
                        
                        // 現在の演算子以下の優先度の演算子が見つかるまで, キューに移し続ける
                        while (this.stack.Count > 0)
                        {
                            var x = this.stack.Pop();

                            if (this.isPriorityGreater(cur.Priority, x.Priority))
                            {
                                this.stack.Push(x);
                                break;
                            }

                            if (this.isIgnore(x.Operator))
                            {
                                continue;
                            }
                            this.queue.Enqueue(x.Operator);
                        }
                        
                        this.stack.Push(cur);
                    }
                    else
                    {
                        // 今回の演算子の方が優先度が高いもしくは同じならば, 順番を保ってStackへ入れる
                        this.stack.Push(prev);
                        this.stack.Push(cur);
                    }
                }
            }

            // 括弧を除いて, 残っているアイテムをStackからQueueに移す
            while (this.stack.Count > 0)
            {
                var x = this.stack.Pop();
                if (this.isIgnore(x.Operator))
                {
                    continue;
                }

                this.queue.Enqueue(x.Operator);
            }

            var rpn = "";
            while (this.queue.Count > 0)
            {
                var x = this.queue.Dequeue();
                rpn += x;
                rpn += this.queue.Count != 0 ? " " : "";
            }

            // デバッグ用
            this.infix_ = infix;
            this.rpn_ = rpn;

            return rpn;
        }

        #endregion
    }
}
