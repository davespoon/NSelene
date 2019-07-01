namespace NSelene
{
    namespace Conditions
    {
        public class Not : DescribedCondition<SeleneElement>
        {
            private DescribedCondition<SeleneElement> _condition;


            public Not(DescribedCondition<SeleneElement> condition)
            {
                _condition = condition;
            }

            public override bool Apply(SeleneElement element)
            {
                return !_condition.Apply(element);
            }

            public override string Explain()
            {
                return "Returns opposite to " + _condition;
            }
        }
    }
}