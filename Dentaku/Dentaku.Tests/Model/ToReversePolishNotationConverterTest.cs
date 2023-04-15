namespace Dentaku.Tests.Model
{
    using System;
    using Xunit;
    using Dentaku.Model;

    /// <summary>
    /// ToReversePolishNotationConverterの単体テスト
    /// </summary>
    public class ToReversePolishNotationConverterTest
    {
        [Theory,
            InlineData("0 + 1", "0 1 +"),
            InlineData("0 - 1", "0 1 -"),
            InlineData("0 * 1", "0 1 *"),
            InlineData("0 / 1", "0 1 /"),
            InlineData("0 % 1", "0 1 %"),
        ]
        public void Test01(string suite, string expect)
        {
            var toReversePolishNotation = new ToReversePolishNotationConverter();

            var actual = toReversePolishNotation.Convert(suite);
            Assert.Equal(expect, actual);
        }

        [Theory,
            InlineData("1 + 2 + 3", "1 2 3 + +"),
            InlineData("1 + 2 - 3", "1 2 3 - +"),
            InlineData("1 - 2 + 3", "1 2 - 3 +"),
            InlineData("1 + 2 * 3", "1 2 3 * +"),
            InlineData("1 * 2 + 3", "1 2 * 3 +"),
            InlineData("1 + 2 / 3", "1 2 3 / +"),
            InlineData("1 / 2 + 3", "1 2 / 3 +"),
            InlineData("1 + 2 % 3", "1 2 3 % +"),
            InlineData("1 % 2 + 3", "1 2 % 3 +"),
        ]
        public void Test02(string suite, string expect)
        {
            var toReversePolishNotation = new ToReversePolishNotationConverter();

            var actual = toReversePolishNotation.Convert(suite);
            Assert.Equal(expect, actual);
        }

        [Theory,
            InlineData("( 1 + 2 ) * 3", "1 2 + 3 *"),
            InlineData("3 * ( 1 + 2 )", "3 1 2 + *"),
            InlineData("( 1 - 2 ) * 3", "1 2 - 3 *"),
            InlineData("3 * ( 1 - 2 )", "3 1 2 - *"),
            InlineData("( 1 + 2 ) / 3", "1 2 + 3 /"),
            InlineData("3 / ( 1 + 2 )", "3 1 2 + /"),
            InlineData("( 1 - 2 ) / 3", "1 2 - 3 /"),
            InlineData("3 / ( 1 - 2 )", "3 1 2 - /"),
            InlineData("( 1 + 2 ) % 3", "1 2 + 3 %"),
            InlineData("3 % ( 1 + 2 )", "3 1 2 + %"),
            InlineData("( 1 - 2 ) % 3", "1 2 - 3 %"),
            InlineData("3 % ( 1 - 2 )", "3 1 2 - %"),
        ]
        public void Test03(string suite, string expect)
        {
            var toReversePolishNotation = new ToReversePolishNotationConverter();

            var actual = toReversePolishNotation.Convert(suite);
            Assert.Equal(expect, actual);
        }

        [Theory,
            InlineData("( ( 1 + 2 ) * 3 + 4 ) * 5", "1 2 + 3 * 4 + 5 *"),
            InlineData("5 * ( ( 1 + 2 ) * 3 + 4 )", "5 1 2 + 3 * 4 + *"),
            InlineData("( ( 1 + 2 ) * 3 - 4 ) * 5", "1 2 + 3 * 4 - 5 *"),
            InlineData("5 * ( ( 1 + 2 ) * 3 - 4 )", "5 1 2 + 3 * 4 - *"),
            InlineData("( ( 1 - 2 ) * 3 + 4 ) * 5", "1 2 - 3 * 4 + 5 *"),
            InlineData("5 * ( ( 1 - 2 ) * 3 + 4 )", "5 1 2 - 3 * 4 + *"),
        ]
        public void Test04(string suite, string expect)
        {
            var toReversePolishNotation = new ToReversePolishNotationConverter();

            var actual = toReversePolishNotation.Convert(suite);
            Assert.Equal(expect, actual);
        }

        [Theory,
            InlineData("1 + 10", "1 10 +"),
            InlineData("10 + 1", "10 1 +"),
            InlineData("1 - 10", "1 10 -"),
            InlineData("10 - 1", "10 1 -"),
            InlineData("1 * 10", "1 10 *"),
            InlineData("10 * 1", "10 1 *"),
            InlineData("1 / 10", "1 10 /"),
            InlineData("10 / 1", "10 1 /"),
            InlineData("1 % 10", "1 10 %"),
            InlineData("10 % 1", "10 1 %"),
        ]
        public void Test05(string suite, string expect)
        {
            var toReversePolishNotation = new ToReversePolishNotationConverter();

            var actual = toReversePolishNotation.Convert(suite);
            Assert.Equal(expect, actual);
        }

        [Theory,
            InlineData("1 + 0.5", "1 0.5 +"),
            InlineData("0.5 + 1", "0.5 1 +"),
            InlineData("1 - 0.5", "1 0.5 -"),
            InlineData("0.5 - 1", "0.5 1 -"),
            InlineData("1 * 0.5", "1 0.5 *"),
            InlineData("0.5 * 1", "0.5 1 *"),
            InlineData("1 / 0.5", "1 0.5 /"),
            InlineData("0.5 / 1", "0.5 1 /"),
            InlineData("1 % 0.5", "1 0.5 %"),
            InlineData("0.5 % 1", "0.5 1 %"),
        ]
        public void Test06(string suite, string expect)
        {
            var toReversePolishNotation = new ToReversePolishNotationConverter();

            var actual = toReversePolishNotation.Convert(suite);
            Assert.Equal(expect, actual);
        }

        [Theory,
            InlineData("A + B", "A B +"),
            InlineData("A - B", "A B -"),
            InlineData("A * B", "A B *"),
            InlineData("A / B", "A B /"),
            InlineData("A % B", "A B %"),
            InlineData("A + B + C", "A B C + +"),
            InlineData("A - B + C", "A B - C +"),
            InlineData("A * B + C", "A B * C +"),
            InlineData("A + B * C", "A B C * +"),
            InlineData("( A + B ) * C", "A B + C *"),
            InlineData("C * ( A + B )", "C A B + *"),
            InlineData("( A + B ) * C + D", "A B + C * D +"),
            InlineData("D + C * ( A + B )", "D C A B + * +"),
        ]
        public void Test07(string suite, string expect)
        {
            var toReversePolishNotation = new ToReversePolishNotationConverter();

            var actual = toReversePolishNotation.Convert(suite);
            Assert.Equal(expect, actual);
        }
    }
}
