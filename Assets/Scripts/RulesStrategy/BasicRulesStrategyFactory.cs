internal class BasicRulesStrategyFactory
{
    internal static RulesStrategy Create(string strategyType)
    {
        return strategyType switch
        {
            "American" => new AmericanRulesStrategy(),
            "Brazilian" => new BrazilianRulesStrategy(),
            "International" => new InternationalRulesStrategy(),
            _ => throw new System.ArgumentException($"Invalid strategy type: {strategyType}"),
        };
    }
}