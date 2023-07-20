using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    #region LEVEL

    const string KEY_LEVEL = "levels";

    public static void IncreaseLevel() => PlayerPrefs.SetInt(KEY_LEVEL, GetLevel() + 1);
    public static int GetLevel() => PlayerPrefs.GetInt(KEY_LEVEL, 0);

    #endregion

    #region COIN

    const string KEY_COIN = "coins";

    public static void AddCoin(int add)
    {
        PlayerPrefs.SetInt(KEY_COIN, GetCoin() + add);
        UIManager.I.UpdateCoinText();
    }
    public static int GetCoin() => PlayerPrefs.GetInt(KEY_COIN, 80);

    public static void ReduceCoin(int reduce)
    {
        PlayerPrefs.SetInt(KEY_COIN, GetCoin() - reduce);
        UIManager.I.UpdateCoinText();
    }

    #endregion

    #region VIBRATION

    const string KEY_VIBRATION = "vibrator";

    public static bool HasVibration() => PlayerPrefs.GetInt(KEY_VIBRATION, 1) == 1;

    public static void ChangeVibrationStatus() { if (HasVibration()) SetVibrationStatus(false); else SetVibrationStatus(true); }

    public static void SetVibrationStatus(bool isEnabled) { PlayerPrefs.SetInt(KEY_VIBRATION, isEnabled ? 1 : 0); UIManager.I.UpdateHapticStatus(); }

    #endregion

    #region PRIZES

    const string KEY_PRIZES = "priozes_";

    public static bool HasPrizeTaken(int id) => PlayerPrefs.GetInt(KEY_PRIZES + id, 0) == 1;

    public static void SetPrizeTaken(int id) => PlayerPrefs.SetInt(KEY_PRIZES + id, 1);

    #endregion
   
    const string INITIAL_POGS = "0,6,0,6";

    const string KEY_DECK = "_DECK";

    public static int[] GetDeck()
    {
        string s_decks = PlayerPrefs.GetString(KEY_DECK, INITIAL_POGS);

        string[] s_elements = s_decks.Split(',');
       
        int[] deck = new int[s_elements.Length];

        for (int i = 0; i < s_elements.Length; i++)
        {

            int _i = -1;

            if (int.TryParse(s_elements[i], out _i))
            {
                deck[i] = _i;
            }
        }

        return deck;
    }
    public static void AddToDeck(int id)
    {
        string s = PlayerPrefs.GetString(KEY_DECK, INITIAL_POGS);

        s += "," + id;
        PlayerPrefs.SetString(KEY_DECK, s);

    }

    //Extra pogs deck
    const string EXTRA_POGS = "0,3,6,15,12,9,18,21,0,3,6";
    const string KEY_EXTRA_DECK = "Extra_DECK";

    public static int[] GetExtraPogs()
    {
        string s_decks = PlayerPrefs.GetString(KEY_EXTRA_DECK, EXTRA_POGS);

        string[] s_elements = s_decks.Split(',');

        int[] deck = new int[s_elements.Length];

        for (int i = 0; i < s_elements.Length; i++)
        {
            int _i = -1;

            if (int.TryParse(s_elements[i], out _i))
            {
                deck[i] = _i;
            }
        }

        return deck;
    }
    public static void AddToExtraPogs(int id)
    {
        string s = PlayerPrefs.GetString(KEY_EXTRA_DECK, EXTRA_POGS);

        s += "," + id;
        PlayerPrefs.SetString(KEY_DECK, s);

        DeckManager.I.GetDeck();
    }
}
