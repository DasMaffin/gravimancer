using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class InputTextChecker : MonoBehaviour
{
    public TMP_InputField ipInputField;

    public static bool IsValidIP(string ip)
    {
        string pattern = @"^(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
               + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
               + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
               + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)$";
        return Regex.IsMatch(ip, pattern);
    }

    public void checkIPInputField(string input)
    {
        if (IsValidIP(input))
        {
            ipInputField.textComponent.color = Color.green;
        }
        else
        {
            ipInputField.textComponent.color = new Color(0.4f, 0f, 0.2f);
        }
    }
}
