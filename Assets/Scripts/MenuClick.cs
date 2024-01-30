using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuClick : MonoBehaviour
{
    [SerializeField]
    RulesContext _rulesContext;

    public void Brazilian()
    {
        _rulesContext.Rules = BaseRulesStrategyFactory.Create(BaseRulesStrategyType.Brazilian);
        Run();
    }
    public void American()
    {
        _rulesContext.Rules = BaseRulesStrategyFactory.Create(BaseRulesStrategyType.American);
        Run();
    }
    public void International()
    {
        _rulesContext.Rules = BaseRulesStrategyFactory.Create(BaseRulesStrategyType.International);
        Run();
    }
    public void Custom()
    {
        _rulesContext.Rules = CustomRulesStrategyFactory.Create(
            boardSize: 5,
            playableTileColor: Color.blue,
            rowsPerTeam: 1,
            darkPieceColor: Color.black,
            startingPieceColor: GameColor.Light,
            flyingKing: true,
            pawnCanCaptureBackwards: false
        );
        Run();
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void Run()
    {
        SceneManager.LoadScene(1);
    }
}
