namespace Dentaku.ViewModel
{
    using System;
    using Reactive.Bindings;

    public class SizeProperty
    {
        #region コンストラクタ

        /// <summary>
        /// ctor
        /// </summary>
        public SizeProperty()
        {

        }

        #endregion

        #region データ

        /// <summary>
        /// 横幅
        /// </summary>
        public ReactiveProperty<double> Width { get { return this._Width; } }
        private ReactiveProperty<double> _Width = new ReactiveProperty<double>(0);

        /// <summary>
        /// 縦幅
        /// </summary>
        public ReactiveProperty<double> Height { get { return this._Height; } }
        private ReactiveProperty<double> _Height = new ReactiveProperty<double>(0);

        #endregion

        #region 処理

        /// <summary>
        /// サイズを設定する
        /// </summary>
        /// <param name="width">横幅</param>
        /// <param name="height">縦幅</param>
        public void SetSize(double width, double height)
        {
            this._Width.Value = width;
            this._Height.Value = height;
        }

        #endregion
    }
}
