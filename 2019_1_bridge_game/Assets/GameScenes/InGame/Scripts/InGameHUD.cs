using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class InGameHUD : MonoBehaviourSingleton<InGameHUD>
{
    [SerializeField] private Text redTeamScoreTxt;
    [SerializeField] private Text blueTeamScoreTxt;
    [SerializeField] private Image redTeamScoreGuage;
    [SerializeField] private Image blueTeamScoreGuage;

    //private int redTeamScore;
    //private int blueTeamScore;

    private Dictionary<int, GameObject> playerListEntries;

    private void OnGUI()
    {
        PunTeams.Team teamName = PunTeams.Team.RED;
        int redTeamScore = 0, blueTeamScore = 0;
        GUIStyle guiStyle = new GUIStyle();
        guiStyle.fontSize = 26;
        guiStyle.normal.textColor = Color.red;
        GUILayout.Label("Team: " + teamName.ToString(), guiStyle);
        List<Player> redTeamPlayers = PunTeams.PlayersPerTeam[teamName];
        foreach (Player player in redTeamPlayers)
        {
            //GUILayout.Label("  " + player.ToStringFull() + " Score: " + player.GetScore(), guiStyle);
            GUILayout.Label(player.NickName + " : 점수 : " + player.GetScore() + ", 악보 수 : " + player.GetNumSheetMusic(), guiStyle);
            redTeamScore += player.GetScore();
        }
        guiStyle.normal.textColor = Color.blue;
        teamName = PunTeams.Team.BLUE;
        GUILayout.Label("Team: " + teamName.ToString(), guiStyle);
        List<Player> blueTeamPlayers = PunTeams.PlayersPerTeam[teamName];
        foreach (Player player in blueTeamPlayers)
        {
            //GUILayout.Label("  " + player.ToStringFull() + " Score: " + player.GetScore(), guiStyle);
            GUILayout.Label(player.NickName + " : 점수 : " + player.GetScore() + ", 악보 수 : " + player.GetNumSheetMusic(), guiStyle);
            blueTeamScore += player.GetScore();
        }
        guiStyle.normal.textColor = Color.white;
        //GUILayout.Label("팀 점수 Red : " + redTeamScore + ", Blue : " + blueTeamScore, guiStyle);
        UpdateTeamScoreGauge(redTeamScore, blueTeamScore);
    }

    private void UpdateTeamScoreGauge(int redTeamScore, int blueTeamScore)
    {
        redTeamScoreTxt.text = redTeamScore.ToString();
        blueTeamScoreTxt.text = blueTeamScore.ToString();

        int totalScore = redTeamScore + blueTeamScore;

        if(totalScore == 0)
        {
            redTeamScoreGuage.fillAmount = 0.5f;
            blueTeamScoreGuage.fillAmount = 0.5f;
        }
        else
        {
            redTeamScoreGuage.fillAmount = (float)redTeamScore / totalScore;
            blueTeamScoreGuage.fillAmount = (float)blueTeamScore / totalScore;
        }
    }
}
