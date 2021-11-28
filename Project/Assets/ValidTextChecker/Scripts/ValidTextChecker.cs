using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 有効文字のチェック
/// </summary>
public class ValidTextChecker : MonoBehaviour
{
    [SerializeField] private char[] invalidChars = null;

    private InputField inputField_ = null;
    private TextGenerator textGenerator_ = null;
    private TextGenerationSettings textGeneratorSettings_;

    /// <summary>
    /// テキストの有効文字のみ抽出する
    /// </summary>
    /// <param name="text"></param>
    /// <param name="invalids"></param>
    /// <param name="textComponent"></param>
    /// <returns></returns>
    public static string GetValidText(string text, char[] invalids, Text textComponent = null)
    {
        if (text == null) { return text; }

        // TextGenerator取得
        TextGenerator textGenerator = null;
        TextGenerationSettings textGeneratorSettings = new TextGenerationSettings();
        if (textComponent != null)
        {
            textGenerator = textComponent.cachedTextGenerator;
            textGeneratorSettings = textComponent.GetGenerationSettings(new Vector2(1000, 1000));
        }

        // 変換処理
        string resultText = "";
        foreach (char targetChar in text.ToCharArray())
        {
            // サロゲートペアの場合は無視する
            if (char.IsSurrogate(targetChar)) { continue; }

            // 指定無効文字の場合は無視する
            if (invalids != null)
            {
                foreach (char invalidChar in invalids)
                {
                    if (targetChar == invalidChar) { continue; }
                }
            }

            // Textに表示できない文字の場合は無視する
            if (textGenerator != null)
            {
                if (textGenerator.GetPreferredWidth(targetChar.ToString(), textGeneratorSettings) == 0)
                {
                    if (targetChar != '\r' || targetChar != '\n') { continue; }
                }
            }

            // 有効文字の追加
            resultText += targetChar;
        }

        return resultText;
    }

    // Start is called before the first frame update
    void Start()
    {
        inputField_ = this.GetComponent<InputField>();
        if (inputField_ != null)
        {
            // コールバック設定
            inputField_.onValidateInput = OnValidateInput;

            textGenerator_ = inputField_.textComponent.cachedTextGenerator;
            textGeneratorSettings_ = inputField_.textComponent.GetGenerationSettings(new Vector2(1000, 1000));
        }
    }

    /// <summary>
    /// 文字更新コールバック
    /// </summary>
    /// <param name="text"></param>
    /// <param name="charIndex"></param>
    /// <param name="addedChar"></param>
    /// <returns></returns>
    private char OnValidateInput(string text, int charIndex, char addedChar)
    {
        // サロゲートペアの場合は削除する
        if (char.IsSurrogate(addedChar)) { return '\0'; }

        // 指定無効文字の場合は削除する
        if (invalidChars != null)
        {
            foreach (char invalidChar in invalidChars)
            {
                if (addedChar == invalidChar) { return '\0'; }
            }
        }

        // InputFieldに表示できない文字の場合は削除する
        if (textGenerator_.GetPreferredWidth(addedChar.ToString(), textGeneratorSettings_) == 0)
        {
            // 改行文字の場合はそのまま返す
            if (addedChar == '\r' || addedChar == '\n') { return addedChar; }
            else { return '\0'; }
        }

        // 有効文字
        return addedChar;
    }
}
