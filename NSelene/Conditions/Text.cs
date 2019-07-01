using System.Text.RegularExpressions;
using NSelene.Conditions;

namespace NSelene
{
    namespace Conditions
    {
        public class Text : DescribedCondition<SeleneElement>
        {
            protected string expected;
            protected string actual;

            public Text(string expected)
            {
                this.expected = expected;
                this.actual = "";
            }

            public override bool Apply(SeleneElement entity)
            {
                this.actual = entity.ActualWebElement.Text;
                return this.actual.Contains(this.expected);
            }

            public override string DescribeActual()
            {
                return this.actual;
            }

            public override string DescribeExpected()
            {
                return "contains " + this.expected;
            }
        }

        public class ExactText : Text
        {
            public ExactText(string expected) : base(expected)
            {
            }

            public override bool Apply(SeleneElement entity)
            {
                this.actual = entity.ActualWebElement.Text;
                return this.actual.Equals(this.expected);
            }

            public override string DescribeExpected()
            {
                return "is " + this.expected;
            }
        }

        public class TextIgnoringCase : Text
        {
            public TextIgnoringCase(string expected) : base(expected)
            {
            }

            public override bool Apply(SeleneElement entity)
            {
                this.actual = entity.ActualWebElement.Text;
                return Regex.IsMatch(actual, this.expected, RegexOptions.IgnoreCase);
            }
            public override string DescribeExpected()
            {
                return "is " + this.expected +  " ignoring case";
            }
        }
    }

    public static partial class Have
    {
        public static Condition<SeleneElement> Text(string expected)
        {
            return new Text(expected);
        }

        public static Condition<SeleneElement> ExactText(string expected)
        {
            return new ExactText(expected);
        }

        public static Condition<SeleneElement> TextIgnoringCase(string expected)
        {
            return new TextIgnoringCase(expected);
        }
    }

    public static partial class HaveNot
    {
        public static Condition<SeleneElement> Text(string exepected)
        {
            return new Not(new Text(exepected));
        }
    }
}