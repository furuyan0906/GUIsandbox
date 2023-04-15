namespace Dentaku.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Reactive.Bindings;
    using Dentaku.Common;
    using Dentaku.Model;

    /// <summary>
    /// DentakuのViewModel
    /// </summary>
    public class MainViewModel
    {
        #region コンストラクタ

        /// <summary>
        /// ctor
        /// </summary>
        public MainViewModel()
        {
            this._AllClearCommand.Subscribe(_ => this.OnAllClearCommandPropertyChanged());
            this._CalculateCommnad.Subscribe(_ => this.OnCalculateCommandPropertyChanged());
            this._OperatorCommand.Subscribe(o => this.OnOperatorCommandPropertyChanged((Operators)o));
            this._ValueCommand.Subscribe(x => this.OnValueCommandPropertyChanged(x.ToString()));
        }

        #endregion

        #region データ

        /// <summary>
        /// Model
        /// </summary>
        private MainModel mainModel = new MainModel();

        /// <summary>
        /// Modelに送る演算子の文字列
        /// </summary>
        private IReadOnlyDictionary<Operators, string> toOperatorStrings = new Dictionary<Operators, string>()
        {
            {
                Operators.Addition, "+"
            },
            {
                Operators.Substraction, "-"
            },
            {
                Operators.Multiplication, "*"
            },
            {
                Operators.Division, "/"
            },
            {
                Operators.Residue, "%"
            },
            {
                Operators.LeftBracket, "("
            },
            {
                Operators.RightBracket, ")"
            },
        };

        /// <summary>
        /// 中置記法の数式
        /// </summary>
        private string infix = "";


        /// <summary>
        /// 数式全消去要求発行コマンド
        /// </summary>
        public ReactiveCommand AllClearCommand => this._AllClearCommand;
        private ReactiveCommand _AllClearCommand = new ReactiveCommand();

        /// <summary>
        /// 計算要求発行コマンド
        /// </summary>
        public ReactiveCommand CalculateCommnad => this._CalculateCommnad;
        private ReactiveCommand _CalculateCommnad = new ReactiveCommand();

        /// <summary>
        /// 演算子追加要求発行コマンド
        /// </summary>
        public ReactiveCommand OperatorCommand => this._OperatorCommand;
        private ReactiveCommand _OperatorCommand = new ReactiveCommand();

        /// <summary>
        /// 数値要求発行コマンド
        /// </summary>
        public ReactiveCommand ValueCommand => this._ValueCommand;
        private ReactiveCommand _ValueCommand = new ReactiveCommand();

        /// <summary>
        /// 各演算子のビュー
        /// </summary>
        public IReadOnlyDictionary<Operators, string> OperatorViews => this._OperatorViews;
        private IReadOnlyDictionary<Operators, string> _OperatorViews = new Dictionary<Operators, string>()
        {
            {
                Operators.Addition, "+"
            },
            {
                Operators.Substraction, "-"
            },
            {
                Operators.Multiplication, "×"
            },
            {
                Operators.Division, "÷"
            },
            {
                Operators.Residue, "%"
            },
            {
                Operators.LeftBracket, "("
            },
            {
                Operators.RightBracket, ")"
            },
        };

        /// <summary>
        /// ACボタンのビュー
        /// </summary>
        public string AllClearCommandView => this._AllClearCommandView;
        private string _AllClearCommandView = "AC";

        /// <summary>
        /// 計算要求ボタンのビュー
        /// </summary>
        public string CalculationCommandView => this._CalculationCommandView;
        private string _CalculationCommandView = "=";

        /// <summary>
        /// 中置記法の数式のビュー
        /// </summary>
        public IReadOnlyReactiveProperty<string> InfixView => this._InfixView.ToReadOnlyReactiveProperty();
        private ReactiveProperty<string> _InfixView = new ReactiveProperty<string>("");

        /// <summary>
        /// 数式クリアフラグ
        /// </summary>
        private bool isInfixClear = false;

        /// <summary>
        /// 数式のコントロールのサイズ
        /// </summary>
        public SizeProperty FormulaControlSize => this.formulaControlSize;
        private SizeProperty formulaControlSize = new SizeProperty();

        #endregion

        #region 処理

        #region イベントハンドラ

        /// <summary>
        /// AC要求発行に対するイベントハンドラ
        /// </summary>
        private void OnAllClearCommandPropertyChanged()
        {
            this.clearInfix();
        }

        /// <summary>
        /// 計算要求に対するイベント
        /// </summary>
        /// <param name="op"></param>
        private void OnCalculateCommandPropertyChanged()
        {
            // イコール複数回押しへの対策
            // フラグがオフになっていないなら, 既に計算が実行されていて, 複数回"="を押された場合である
            if (this.isInfixClear)
            {
                return ; 
            }

            // 次の入力で数式をクリアする
            this.isInfixClear = true;

            var s = " = ";
            try
            {
                var ans = this.calculate();
                s += ans.ToString() + " ";
            }
            catch (DivideByZeroException)
            {
                s += "±INF";
            }
            catch (ArgumentException)
            {
                s += "INVALID";
            }
            catch (InvalidOperationException)
            {
                s += "INVALID";
            }

            this.ChangeInfix("", s);
        }

        /// <summary>
        /// 演算子追加要求に対するイベントハンドラ
        /// </summary>
        /// <param name="op"></param>
        private void OnOperatorCommandPropertyChanged(Operators op)
        {
            if (this.isInfixClear)
            {
                this.clearInfix();
            }

            var header = "";
            var tail = "";
            switch (op)
            {
                case Operators.LeftBracket:
                case Operators.RightBracket:
                    header = "";
                    tail = "";
                    break;
                default:
                    header = " ";
                    tail = " ";
                    break;
            }

            // Modelに送る数式はスペース区切り
            this.ChangeInfix(" " + this.toOperatorStrings[op] + " ", header + this._OperatorViews[op] + tail);
        }

        /// <summary>
        /// 数値追加要求に対するイベントハンドラ
        /// </summary>
        /// <param name="x">数字</param>
        /// <remarks>引数は"0"~"9"のはず</remarks>
        private void OnValueCommandPropertyChanged(string x)
        {
            if (this.isInfixClear)
            {
                this.clearInfix();
            }

            // Modelに送る数式はスペース区切り
            this.ChangeInfix(" " + x + " ", x);
        }

        #endregion

        #region 内部処理

        /// <summary>
        /// 数式のビューを変更する
        /// </summary>
        /// <param name="s"></param>
        private void ChangeInfix(string s, string sview)
        {
            this.infix += s;
            this._InfixView.Value += sview;
        }

        /// <summary>
        /// 計算を行う
        /// </summary>
        /// <returns></returns>
        private double calculate()
        {
            var ans = this.mainModel.Calculate(this.infix);
            return ans;
        }

        /// <summary>
        /// 数式をクリアする
        /// </summary>
        private void clearInfix()
        {
            this.isInfixClear = false;

            this.infix = "";
            this._InfixView.Value = "";
        }

        #endregion

        /// <summary>
        /// 数式用コントロールのサイズを設定する
        /// </summary>
        /// <param name="ctrl">親コントロールのサイズ</param>
        public void SetFormulaControlSize((double width, double height) ctrl)
        {
            this.formulaControlSize.SetSize(ctrl.width, ctrl.height);
        }

        #endregion
    }
}
