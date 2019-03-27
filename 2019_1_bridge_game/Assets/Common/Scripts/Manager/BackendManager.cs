using UnityEngine;
using LitJson;
using BackEnd;
using System.Collections.Generic;
using System;

public class BackendManager : MonoBehaviourSingleton<BackendManager>
{
    private string id = "";
    private string pw = "";
    private string nick = "";

    private BackendReturnObject bro = new BackendReturnObject();

    private string Indate;     //gameinfo update, gameinfo delete, findOne에 사용
    private string characterTable = "character";

    private List<string> PublicTables = new List<string>();
    private List<string> PrivateTables = new List<string>();

    #region Get / Set
    public void SetSignupData(string id, string pw, string nick)
    {
        this.id = id; this.pw = pw; this.nick = nick;
    }

    public void SetLoginData(string id, string pw)
    {
        this.id = id; this.pw = pw;
    }

    public string ID { get { return id; } set { id = value; } }
    public string PW { get { return pw; } set { pw = value; } }
    public string Nick { get { return nick; } set { nick = value; } }
    public BackendReturnObject BRO { get { return bro; } set { bro = value; } }
    #endregion

    public void GetChartSave()
    {
        Debug.Log("-----------------A Get Chart And Save-----------------");
        PlayerPrefs.DeleteAll();
        Backend.Chart.GetChartAndSave(chartsave =>
        {
            Debug.Log(chartsave.ToString());
            bro = chartsave;
            SuccessUserData();
        });
    }


    // 비동기식 데이터 저장
    private void SuccessUserData()
    {
        Debug.Log("-----------------Update-----------------");

        PlayerPrefs.DeleteAll();
        Backend.Chart.SaveChart(bro);

        if (bro.IsSuccess())
        {
            JsonData rows = bro.GetReturnValuetoJSON()["rows"];
            string ChartName, ChartContents;
            // get chart contents with chartName
            for (int i = 0; i < rows.Count; i++)
            {
                ChartName = rows[i]["chartName"]["S"].ToString();
                ChartContents = PlayerPrefs.GetString(ChartName);
                Debug.Log(string.Format("{0}\n{1}", ChartName, ChartContents));
            }
        }

        bro.Clear();
    }


    public void GameInfoInsert()
    {
        Debug.Log("-----------------A GameInfo Insert-----------------");

        Param param = new Param();
        param.Add("id", id);
        param.Add("nickname", nick);
        param.Add("coin", 0);
        param.Add("rank", 0);
        param.Add("honor", 0);
        param.Add("level", 0);
        param.Add("stage", 0);

        Backend.GameInfo.Insert(characterTable, param, insertComplete =>
        {
            Debug.Log("insert : " + insertComplete.ToString());
            if (insertComplete.IsSuccess())
            {
                Indate = insertComplete.GetInDate();
                Debug.Log("indate : " + Indate);
            }
        });
    }

    public void GetTableList()
    {
        Debug.Log("-----------------A Get Table List-----------------");
        Backend.GameInfo.GetTableList(tablelist =>
        {
            Debug.Log(tablelist);

            if (tablelist.IsSuccess())
            {
                SetTable(tablelist.GetReturnValuetoJSON());
            }
        });
    }

    public void CreateNickname()
    {
        Debug.Log("-------------CreateNickname-------------");

        Backend.BMember.CreateNickname(nick, isComplete =>
        {
            Debug.Log(isComplete.ToString());
        });
    }

    public void UpdateNickname()
    {
        Debug.Log("-------------UpdateNickname-------------");

        Backend.BMember.UpdateNickname(nick, isComplete =>
        {
            Debug.Log(isComplete.ToString());
        });
    }


    // character, stage, item, present, message
    public void GetPrivateContents(string tableName)
    {
        Debug.Log("-----------------AGet Private Contents-----------------");
        Backend.GameInfo.GetPrivateContents(tableName, bro =>
        {
            Debug.Log(bro);
            if (bro.IsSuccess())
            {
                GetGameInfo(bro.GetReturnValuetoJSON());
            }
        });
    }

    public void AGetPublicContents(string tableName)
    {
        Debug.Log("-----------------AGet Public Contents-----------------");

        Backend.GameInfo.GetPublicContents(tableName, bro =>
        {
            Debug.Log(bro);

            if (bro.IsSuccess())
            {
                GetGameInfo(bro.GetReturnValuetoJSON());
            }
        });
    }

    private void SetTable(JsonData data)
    {
        JsonData publics = data["publicTables"];
        foreach (JsonData row in publics)
        {
            PublicTables.Add(row.ToString());
        }

        JsonData privates = data["privateTables"];
        foreach (JsonData row in privates)
        {
            PrivateTables.Add(row.ToString());
        }
    }

    private void GetGameInfo(JsonData returnData)
    {
        // ReturnValue가 존재하고, 데이터가 있는지 확인
        if (returnData != null)
        {
            Debug.Log("returnvalue is not null");
            // for the rows 
            if (returnData.Keys.Contains("rows"))
            {
                Debug.Log("returnvalue contains rows");
                JsonData rows = returnData["rows"];

                for (int i = 0; i < rows.Count; i++)
                {
                    GetData(rows[i]);
                }
            }
            // for an row
            else if (returnData.Keys.Contains("row"))
            {
                Debug.Log("returnvalue contains row");
                JsonData row = returnData["row"];

                GetData(row[0]);
            }
        }
        else
        {
            Debug.Log("contents has no data");
        }
    }

    // json parsing 활용
    private void GetData(JsonData data)
    {
        string scoreKey = "score";
        string lunchKey = "lunch";
        string listKey = "list_string";

        // score 라는 key가 존재하는지 확인
        if (data.Keys.Contains(scoreKey))
        {
            var score = data[scoreKey]["N"];
            Debug.Log("score: " + score);
        }
        else
        {
            Debug.Log("there is no key " + scoreKey);
        }
        //Debug.Log("data.Keys.Contains(scoreKey" + data.Keys.Contains(scoreKey));
        // lunch 라는 key가 존재하는지 확인
        if (data.Keys.Contains(lunchKey))
        {
            JsonData lunch = data[lunchKey]["M"];
            var howmuchKey = "how much";
            var whenKey = "when";
            var whatKey = "what";

            if (lunch.Keys.Contains(howmuchKey) && lunch.Keys.Contains(whenKey) && lunch.Keys.Contains(whatKey))
            {
                var howmuch = lunch[howmuchKey]["N"].ToString();
                var when = lunch[whenKey]["S"].ToString();
                var what = lunch[whatKey]["S"].ToString();

                Debug.Log(when + " " + what + " " + howmuch);
            }
            else
            {
                Debug.Log("there is no key (" + howmuchKey + " || " + whenKey + " || " + whatKey + ")");
            }
        }
        else
        {
            Debug.Log("there is no key " + lunchKey);
        }

        // list_string 라는 key가 존재하는지 확인
        if (data.Keys.Contains(listKey))
        {
            List<string> returnlist = new List<string>();
            JsonData list = data[listKey]["L"];
            var listCount = list.Count;
            if (listCount > 0)
            {
                for (int j = 0; j < listCount; j++)
                {
                    var listdata = list[j]["S"].ToString();
                    returnlist.Add(listdata);
                }
                Debug.Log(JsonMapper.ToJson(returnlist));
            }
            else
            {
                Debug.Log("list has no data");
            }
        }
        else
        {
            Debug.Log("there is no key " + listKey);
        }
    }
}
