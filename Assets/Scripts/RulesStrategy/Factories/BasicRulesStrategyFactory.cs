internal class BasicRulesStrategyFactory
{
    internal static RulesStrategy Create(BasicRulesStrategyType strategyType)
    {
        return strategyType switch
        {
            BasicRulesStrategyType.American => new AmericanRulesStrategy(),
            BasicRulesStrategyType.Brazilian => new BrazilianRulesStrategy(),
            BasicRulesStrategyType.International => new InternationalRulesStrategy(),
            _ => throw new System.ArgumentException($"Invalid strategy type: {strategyType}"),
        };
    }
}