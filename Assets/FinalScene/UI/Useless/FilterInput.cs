using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FilterInput : MonoBehaviour
{
    public TMP_InputField inputField; // Assignez ceci depuis l'inspecteur

    void Start()
    {
        // Ajoutez un listener pour vérifier les changements de texte
        inputField.onValueChanged.AddListener(OnInputValueChanged);
    }

    void OnInputValueChanged(string input)
    {
        // Filtrer les caractères non numériques
        string filteredInput = FilterNonNumeric(input);

        // Si le texte a été modifié par le filtrage, mettez à jour le champ de texte
        if (input != filteredInput)
        {
            inputField.text = filteredInput;
        }
    }

    string FilterNonNumeric(string input)
    {
        // Utiliser System.Text pour construire une chaîne de caractères filtrés
        System.Text.StringBuilder numericStringBuilder = new System.Text.StringBuilder();
        bool hasDecimalPoint = false;

        foreach (char c in input)
        {
            if (char.IsDigit(c))
            {
                numericStringBuilder.Append(c);
            }
            else if (c == '.' && !hasDecimalPoint)
            {
                numericStringBuilder.Append(c);
                hasDecimalPoint = true;
            }
        }

        return numericStringBuilder.ToString();
    }
}
