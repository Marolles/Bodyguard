using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanelScript : MonoBehaviour
{
    public Text paparazziCount;
    public Text interviewerCount;
    public Text moneyCount;
    public Text totalCount;

    public GameObject billPrefab;
    public Transform billPosition;

    public void AnimatePanel()
    {
        StartCoroutine(AnimatePanel_C());
    }
    public IEnumerator AnimatePanel_C()
    {
        paparazziCount.text = "0";
        interviewerCount.text = "0";
        moneyCount.text = "0";
        totalCount.text = "0";
        yield return new WaitForSeconds(1f);
        StartCoroutine(IncrementScoreCount_C(moneyCount, GameManager.i.paparazziKilled * 100 + GameManager.i.interviewerKilled * 50, 0.005f, 50));
        yield return StartCoroutine(IncrementCount_C(paparazziCount, GameManager.i.paparazziKilled, 0.1f));
        yield return StartCoroutine(IncrementCount_C(interviewerCount, GameManager.i.interviewerKilled, 0.1f));
    }

    public void IncrementCount(Text text, int amount, float incrementationSpeed)
    {
        StartCoroutine(IncrementCount_C(text, amount, incrementationSpeed));
    }

    public IEnumerator IncrementCount_C(Text text, int amount, float incrementationSpeed)
    {
        text.text = "0";
        for (int i = 0; i < amount; i++)
        {
            Debug.Log("Increment");
            text.text = i.ToString();
            yield return new WaitForSeconds(incrementationSpeed);
        }
        yield return null;
    }

    public IEnumerator IncrementScoreCount_C(Text text, int amount, float incrementationSpeed, int billFrequency)
    {
        text.text = "0";
        int billCount = 0;
        for (int i = 0; i < amount; i+=Random.Range(3,6))
        {
            text.text = i.ToString();
            if (billCount >= billFrequency)
            {
                //Spawn bill
                GameObject bill = Instantiate(billPrefab);
                bill.transform.localPosition = billPosition.transform.position;
                billCount = 0;
            }
            yield return new WaitForSeconds(incrementationSpeed);
            billCount++;
        }
        text.text = amount.ToString();
        yield return null;
    }
}
