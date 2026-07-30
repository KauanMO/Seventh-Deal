using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextShake : MonoBehaviour
{
    [SerializeField] private float shakeAmount = 2f;

    private TMP_Text textMesh;
    private HashSet<int> shakeIndices = new HashSet<int>();
    private string lastRawText = null;

    private void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (textMesh.text != lastRawText)
            ParseShakeTags();

        textMesh.ForceMeshUpdate();

        TMP_TextInfo textInfo = textMesh.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!shakeIndices.Contains(i))
                continue;

            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
                continue;

            int vertexIndex = charInfo.vertexIndex;
            int materialIndex = charInfo.materialReferenceIndex;

            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
            Vector3 offset = Random.insideUnitCircle * shakeAmount;

            vertices[vertexIndex + 0] += offset;
            vertices[vertexIndex + 1] += offset;
            vertices[vertexIndex + 2] += offset;
            vertices[vertexIndex + 3] += offset;
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }

    private void ParseShakeTags()
    {
        lastRawText = textMesh.text;
        shakeIndices.Clear();

        string raw = lastRawText;
        string cleanText = "";
        int charIndex = 0;
        bool inShake = false;

        var tokenRegex = new Regex(@"<shake>|</shake>|[\s\S]");
        var matches = tokenRegex.Matches(raw);

        foreach (Match match in matches)
        {
            string token = match.Value;

            if (token == "<shake>")
            {
                inShake = true;
            }
            else if (token == "</shake>")
            {
                inShake = false;
            }
            else
            {
                cleanText += token;
                if (inShake)
                    shakeIndices.Add(charIndex);
                charIndex++;
            }
        }

        if (cleanText != raw)
        {
            lastRawText = cleanText;
            textMesh.text = cleanText;
        }
    }
}