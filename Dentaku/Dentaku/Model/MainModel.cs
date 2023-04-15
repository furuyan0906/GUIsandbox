namespace Dentaku.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// DentakuのModel
    /// </summary>
    public class MainModel
    {
        #region コンストラクタ

        /// <summary>
        /// ctor
        /// </summary>
        public MainModel()
        {
            // do nothing
        }

        #endregion

        #region 内部データ

        /// <summary>
        /// 中置記法の数式から逆ポーランド記法の数式へ変換するコンバーター
        /// </summary>
        private ToReversePolishNotationConverter infixToRpnConverter = new ToReversePolishNotationConverter();

        /// <summary>
        /// 逆ポーランド記法の数式を計算する評価者
        /// </summary>
        private ReversePolishNotationEvaluator rpnEvaluator = new ReversePolishNotationEvaluator();

        #endregion

        #region 処理

        /// <summary>
        /// 中置記法の数式を計算する
        /// </summary>
        /// <param name="inifixNotation">中置記法の数式の文字列</param>
        /// <returns></returns>
        public double Calculate(string infix)
        {
            var rpn = this.infixToRpnConverter.Convert(infix);
            var ans = this.rpnEvaluator.Calculate(rpn);

            return ans;
        }

        #endregion
    }
}
