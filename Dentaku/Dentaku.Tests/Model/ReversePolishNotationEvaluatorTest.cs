namespace Dentaku.Tests.Model
{
    using System;
    using Xunit;
    using Dentaku.Model;

    /// <summary>
    /// ReversePolishNotationEvaluatorの単体テスト
    /// </summary>
    public class ReversePolishNotationEvaluatorTest
    {
        [Theory,
            InlineData("1 0 +", 1),
            InlineData("0 1 +", 1),
            InlineData("1 0 -", 1),
            InlineData("0 1 -", -1),
            InlineData("1 0 *", 0),
            InlineData("0 1 *", 0),
            InlineData("1 2 /", 0.5),
            InlineData("2 1 /", 2),
            InlineData("1 2 %", 1),
            InlineData("2 1 %", 0),
        ]
        public void Test01(string suite, double expect)
        {
            var evalator = new ReversePolishNotationEvaluator();

            var actual = evalator.Calculate(suite);
            Assert.Equal(expect, actual);
        }

        [Theory,
            InlineData("1 10 +", 11),
            InlineData("10 1 +", 11),
            InlineData("1 10 -", -9),
            InlineData("10 1 -", 9),
            InlineData("1 10 *", 10),
            InlineData("10 1 *", 10),
            InlineData("1 10 /", 0.1),
            InlineData("10 1 /", 10),
            InlineData("1 10 %", 1),
            InlineData("10 1 %", 0),
        ]
        public void Test02(string suite, double expect)
        {
            var evalator = new ReversePolishNotationEvaluator();

            var actual = evalator.Calculate(suite);
            Assert.Equal(expect, actual);
        }

        [Theory,
            InlineData("1 0.5 +", 1.5),
            InlineData("0.5 1 +", 1.5),
            InlineData("1 0.5 -", 0.5),
            InlineData("0.5 1 -", -0.5),
            InlineData("1 0.5 *", 0.5),
            InlineData("0.5 1 *", 0.5),
            InlineData("1 0.5 /", 2),
            InlineData("0.5 1 /", 0.5),
        ]
        public void Test03(string suite, double expect)
        {
            var evalator = new ReversePolishNotationEvaluator();

            var actual = evalator.Calculate(suite);
            Assert.Equal(expect, actual);
        }

        [Theory,
            InlineData("1 2 3 + +", 6),
            InlineData("1 2 3 - +", 0),
            InlineData("1 2 - 3 +", 2),
            InlineData("1 2 3 * +", 7),
            InlineData("1 2 * 3 +", 5),
            InlineData("1 2 / 3 +", 3.5),
            InlineData("1 2 3 % +", 3),
            InlineData("1 2 % 3 +", 4),
        ]
        public void Test04(string suite, double expect)
        {
            var evalator = new ReversePolishNotationEvaluator();

            var actual = evalator.Calculate(suite);
            Assert.Equal(expect, actual);
        }

        [Theory,
            InlineData("1 2 + 3 *", 9),
            InlineData("3 1 2 + *", 9),
            InlineData("1 2 - 3 *", -3),
            InlineData("3 1 2 - *", -3),
            InlineData("1 2 + 3 /", 1),
            InlineData("3 1 2 + /", 1),
            InlineData("1 2 + 3 %", 0),
            InlineData("3 1 2 + %", 0),
        ]
        public void Test05(string suite, double expect)
        {
            var evalator = new ReversePolishNotationEvaluator();

            var actual = evalator.Calculate(suite);
            Assert.Equal(expect, actual);
        }

        [Theory,
            InlineData("1 2 + 3 * 4 + 5 *", 65),
            InlineData("5 1 2 + 3 * 4 + *", 65),
            InlineData("1 2 + 3 * 4 - 5 *", 25),
            InlineData("5 1 2 + 3 * 4 - *", 25),
            InlineData("1 2 - 3 * 4 + 5 *", 5),
            InlineData("5 1 2 - 3 * 4 + *", 5),
        ]
        public void Test06(string suite, double expect)
        {
            var evalator = new ReversePolishNotationEvaluator();

            var actual = evalator.Calculate(suite);
            Assert.Equal(expect, actual);
        }
    }
}
