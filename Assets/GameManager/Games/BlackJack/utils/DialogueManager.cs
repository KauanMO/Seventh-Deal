using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueManager
{
    private readonly DialogueRunner dialogueRunner;
    private readonly Queue<(string dialogueName, Dictionary<string, object> variables, Action next)> dialogueQueue = new();
    private bool isPlaying = false;

    public DialogueManager(DialogueRunner dialogueRunner)
    {
        this.dialogueRunner = dialogueRunner;
    }

    public void TriggerDialogue(string dialogueName, Dictionary<string, object> variables = null, Action next = null)
    {
        QueueDialogue(dialogueName, variables, next);
    }

    public void QueueDialogue(string dialogueName, Dictionary<string, object> variables = null, Action next = null)
    {
        Debug.Log(dialogueName);

        dialogueQueue.Enqueue((dialogueName, variables, next));

        Debug.Log(isPlaying);

        if (!isPlaying)
        {
            dialogueRunner.StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator ProcessQueue()
    {
        isPlaying = true;

        while (dialogueQueue.Count > 0)
        {
            var (dialogueName, variables, next) = dialogueQueue.Dequeue();

            if (variables != null)
            {
                SetVariables(variables);
            }

            yield return RunDialogue(dialogueName);

            next?.Invoke();
        }

        isPlaying = false;
    }

    private IEnumerator RunDialogue(string dialogueName)
    {
        bool completed = false;
        bool failed = false;

        void OnComplete() => completed = true;

        dialogueRunner.onDialogueComplete.AddListener(OnComplete);

        try
        {
            _ = dialogueRunner.StartDialogue(dialogueName);
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao iniciar diálogo {dialogueName}: {e}");
            failed = true;
        }

        if (!failed)
        {
            yield return new WaitUntil(() => completed);
        }

        dialogueRunner.onDialogueComplete.RemoveListener(OnComplete);
    }

    private void SetVariables(Dictionary<string, object> variables)
    {
        var storage = dialogueRunner.VariableStorage;

        foreach (var kvp in variables)
        {
            string key = kvp.Key.StartsWith("$") ? kvp.Key : "$" + kvp.Key;

            switch (kvp.Value)
            {
                case string s:
                    storage.SetValue(key, s);
                    break;
                case bool b:
                    storage.SetValue(key, b);
                    break;
                case float f:
                    storage.SetValue(key, f);
                    break;
                case int i:
                    storage.SetValue(key, (float)i);
                    break;
                case double d:
                    storage.SetValue(key, (float)d);
                    break;
                default:
                    throw new ArgumentException($"Type not supported '{key}': {kvp.Value.GetType()}");
            }
        }
    }
}