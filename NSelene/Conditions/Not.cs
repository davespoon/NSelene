namespace NSelene
{
    namespace Conditions
    {
        public class Not : DescribedCondition<SeleneElement>
        {
            private DescribedCondition<SeleneElement> condition;


            public Not(DescribedCondition<SeleneElement> originalCondition)
            {
                condition = originalCondition;
            }

            public override bool Apply(SeleneElement element)
            {
                return !condition.Apply(element);
            }

            public override string Explain()
            {
                return "Returns opposite to " + condition;
            }
        }
    }
}