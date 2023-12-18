internal class BaseRulesStrategyFactory
{
    internal static RulesStrategy Create(BaseRulesStrategyType strategyType)
    {
        return strategyType switch
        {
            BaseRulesStrategyType.American => new AmericanRulesStrategy(),
            BaseRulesStrategyType.Brazilian => new BrazilianRulesStrategy(),
            BaseRulesStrategyType.International => new InternationalRulesStrategy(),
            _ => throw new System.ArgumentException($"Invalid strategy type: {strategyType}"),
        };
    }
}