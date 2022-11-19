using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMessage : MonoBehaviour
{
    [SerializeField] List<ConcreteFactory> factories;//list of factories
    [SerializeField] List<string> messages; // list of messages
    [SerializeField] Text facroryMessage;
    void Awake()
    {
        for (int i = 0; i < factories.Count; i++)
        {
            factories[i].ShowMessage += ChangeMessage; // subscribing to event of showing message
            messages.Add(factories[i].Message); // add message from factory
        }

    }
    // displaying UI text
    public IEnumerator ShowText()
    {
        facroryMessage.text = "";
        for (int i = 0; i < messages.Count; i++)
        {
            if (messages[i] == "") continue;
            facroryMessage.gameObject.SetActive(true);
            facroryMessage.text += factories[i].name  +" "+ messages[i]+"\n";
            facroryMessage.color = Color.red;
        }
        yield return new WaitForSeconds(1f);
        facroryMessage.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
        StartCoroutine(ShowText());
    }

    // renewing message according to factory message needs
    public void ChangeMessage()
    {
        bool check = false; ;
        for (int i = 0; i < factories.Count; i++)
        {
            if (messages[i] == factories[i].Message) check = true;
            else check = false;
            messages[i] = factories[i].Message;
        }
        if (!check) 
        StartCoroutine(ShowText());
    }
}
