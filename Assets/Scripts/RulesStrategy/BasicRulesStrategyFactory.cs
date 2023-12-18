internal class BasicRulesStrategyFactory
{
    internal static RulesStrategy Create(RulesStrategyType strategyType)
    {
        return strategyType switch
        {
            RulesStrategyType.American => new AmericanRulesStrategy(),
            RulesStrategyType.Brazilian => new BrazilianRulesStrategy(),
            RulesStrategyType.International => new InternationalRulesStrategy(),
            _ => throw new System.ArgumentException($"Invalid strategy type: {strategyType}"),
        };
    }
}