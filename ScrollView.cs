using PlayFab;
using PlayFab.AdminModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollView : MonoBehaviour
{
    public ScrollView scrollView;
    public Button newbutton;
    public InputField input;
    List<ContentInfo> contentInfos=new List<ContentInfo>();
    bool OnActive = false;
    Transform Content;
    void Start()
    {
        Content = scrollView.transform.GetChild(0).GetChild(0);
        for (int i = 1; i < 20; i++)
        {
            RectTransform recttr = Content.GetChild(0).GetComponent<RectTransform>();

            Vector3 vector3 = recttr.localPosition;
            Button instancBut = Instantiate<Button>(newbutton);
            instancBut.transform.SetParent(Content, false);
            
            instancBut.transform.localScale = Vector3.one;
            vector3.y -= 40*i;
            instancBut.transform.localPosition = vector3;
            
        }
        for (int i = 1; i < Content.childCount; i++)
        {
            Content.GetChild(i).gameObject.SetActive(false);
        }
        scrollView.gameObject.SetActive(false);
    }
    public void DownloadButtonSet()
    {
        if (!OnActive)
        {
            input.transform.Find("SaveBut").gameObject.SetActive(false);
            input.transform.Find("LoadBut").gameObject.SetActive(true);
            scrollView.gameObject.SetActive(true);
            input.gameObject.SetActive(true);
            StartCoroutine(enumerator(2f));
            OnActive = true;
        }
        else
        {
            scrollView.gameObject.SetActive(false);
            input.gameObject.SetActive(false);
            OnActive = false;
            for (int i = 1; i < Content.childCount; i++)
            {
                Content.GetChild(i).gameObject.SetActive(false);
            }

        }
    }

    IEnumerator enumerator(float time)
    {
        Dictionary<string, string> keyValues = new Dictionary<string, string>();
        GetContentListRequest getContentListRequest = new GetContentListRequest { Prefix = "" };
        PlayFabAdminAPI.GetContentList(getContentListRequest, result =>
        {
            contentInfos = result.Contents;
        },
            error => { Debug.LogError(error.GenerateErrorReport()); }, keyValues);
        yield return new WaitForSeconds(time);

        for (int i = 0; i < contentInfos.Count; i++)
        {
            Content.GetChild(i + 1).gameObject.SetActive(true); 
            Content.GetChild(i+1).GetChild(0).GetComponent<Text>().text = contentInfos[i].Key;
        }

    }
    
}
