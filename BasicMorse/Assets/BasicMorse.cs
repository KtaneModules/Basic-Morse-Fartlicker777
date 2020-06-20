using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class BasicMorse : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMSelectable[] Letters;
    public KMSelectable[] ArrowsAndSubmit;
    public TextMesh AidsNumbers;

    string[] WordsOfAids = {
"ACID","BUST","CODE","DAZE","ECHO","FILM","GOLF","HUNT","ITCH","JURY","KING","LIME","MONK","NUMB","ONLY","PREY","QUIT","RAVE","SIZE","TOWN","URGE","VERY","WAXY","XYLO","YARD","ZERO","ABORT","BLEND","CRYPT","DWARF","EQUIP","FANCY","GIZMO","HELIX","IMPLY","JOWLS","KNIFE","LEMON","MAJOR","NIGHT","OVERT","POWER","QUILT","RUSTY","STOMP","TRASH","UNTIL","VIRUS","WHISK","XERIC","YACHT","ZEBRA","ADVICE","BUTLER","CAVITY","DIGEST","ELBOWS","FIXURE","GOBLET","HANDLE","INDUCT","JOKING","KNEADS","LENGTH","MOVIES","NIMBLE","OBTAIN","PERSON","QUIVER","RACHET","SAILOR","TRANCE","UPHELD","VANISH","WALNUT","XYLOSE","YANKED","ZODIAC","ALREADY","BROWSED","CAPITOL","DESTROY","ERASING","FLASHED","GRIMACE","HIDEOUT","INFUSED","JOYRIDE","KETCHUP","LOCKING","MAILBOX","NUMBERS","OBSCURE","PHANTOM","QUIETLY","REFUSAL","SUBJECT","TRAGEDY","UNKEMPT","VENISON","WARSHIP","XANTHIC","YOUNGER","ZEPHYRS","ADVOCATE","BACKFLIP","CHIMNEYS","DISTANCE","EXPLOITS","FOCALIZE","GIFTWRAP","HOVERING","INVENTOR","JEALOUSY","KINSFOLK","LOCKABLE","MERCIFUL","NOTECARD","OVERCAST","PERILOUS","QUESTION","RAINCOAT","STEALING","TREASURY","UPDATING","VERTICAL","WISHBONE","XENOLITH","YEARLONG","ZEALOTRY","ABHORRENT","BACCARATS","CULTIVATE","DAMNINGLY","EFFLUXION","FUTURISTS","GYROSCOPE","HAZARDOUS","ILLOGICAL","JUXTAPOSE","KILOBYTES","LANTHANUM","MATERIALS","NIHILISTS","OBSCENITY","PAINFULLY","QUEERNESS","RESTROOMS","SABOTAGED","TYRANNOUS","UMPTEENTH","VEXILLATE","WAYLAYERS","XENOBLAST","YTTERBIUM","ZIGZAGGER"
};
    string STDphabet = "QWERTYUIOPASDFGHJKLZXCVBNM";
    string StandardAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    int[] WordsButBaseTen = {7,27,30,9,2,57,12,54,6,67,10,63,4,3,13,66,37,21,18,1,19,55,22,28,31,36};
    int RandomAidsPicker = 0;
    string SaintKildaFan = "";
    int Emotiguy = 2;
    private List<int> Starting = new List<int>{};
    private List<int> Offset = new List<int>{};
    private List<int> Result = new List<int>{};
    string CasualBattlesOfTheCongo = "";
    bool[] Fuckyou = {false,false,false,false,false,false,false,false,false,false,false,false};
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake () {
        moduleId = moduleIdCounter++;

        foreach (KMSelectable Letter in Letters) {
            Letter.OnInteract += delegate () { LetterPress(Letter); return false; };
        }
        foreach (KMSelectable Arrow in ArrowsAndSubmit) {
            Arrow.OnInteract += delegate () { ArrowPress(Arrow); return false; };
        }
    }

    void Start () {
      RandomAidsPicker = UnityEngine.Random.Range(0,WordsOfAids.Length);
      SaintKildaFan = WordsOfAids[RandomAidsPicker];
      for (int i = 0; i < SaintKildaFan.Length; i++) {
        for (int j = 0; j < 26; j++) {
          if (SaintKildaFan[i] == StandardAlphabet[j]) {
            Starting.Add(WordsButBaseTen[j]);
            Offset.Add(UnityEngine.Random.Range(0,WordsButBaseTen[j]));
            Result.Add(Starting.Last()-Offset.Last());
          }
        }
      }
      Debug.LogFormat("[Basic Morse #{0}] The word chosen is {1}.", moduleId, SaintKildaFan);
      NumberDisplay();
    }

    void LetterPress(KMSelectable Letter) {
      Letter.AddInteractionPunch();
      Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Letter.transform);
      if (moduleSolved == true) {
        return;
      }
      for (int i = 0; i < 26; i++) {
        if (Letter == Letters[i]) {
          CasualBattlesOfTheCongo += STDphabet[i];
          AidsNumbers.text = CasualBattlesOfTheCongo;
        }
      }
    }
    void ArrowPress(KMSelectable Arrow){
      Arrow.AddInteractionPunch();
      Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Arrow.transform);
      if (moduleSolved == true) {
        return;
      }
        if (Arrow == ArrowsAndSubmit[0]) {
          Emotiguy += 1;
          if (Emotiguy - 1 > SaintKildaFan.Length) {
            Emotiguy = 2;
          }
          CasualBattlesOfTheCongo = "";
          NumberDisplay();
        }
        else if (Arrow == ArrowsAndSubmit[1]) {
          Emotiguy -= 1;
          if (Emotiguy < 2) {
            Emotiguy = SaintKildaFan.Length + 1;
          }
          CasualBattlesOfTheCongo = "";
          NumberDisplay();
        }
        else {
          if (CasualBattlesOfTheCongo == SaintKildaFan) {
            GetComponent<KMBombModule>().HandlePass();
            moduleSolved = true;
          }
          else {
            GetComponent<KMBombModule>().HandleStrike();
          }
        }
    }
    void NumberDisplay() {
      switch (Emotiguy) {
        case 2:
        if (Fuckyou[Emotiguy - 2] == false) {
          Debug.LogFormat("[Basic Morse #{0}] The page says {1} + {2} in base {3}. This shows up as {4} + {5}.", moduleId, Result[Emotiguy - 2], Offset[Emotiguy - 2], Emotiguy,PrimeChecker(Result[Emotiguy - 2], Emotiguy), PrimeChecker(Offset[Emotiguy - 2], Emotiguy));
        }
        Fuckyou[Emotiguy - 2] = true;
        break;
        case 3:
        if (Fuckyou[Emotiguy - 2] == false) {
          Debug.LogFormat("[Basic Morse #{0}] The page says {1} + {2} in base {3}. This shows up as {4} + {5}.", moduleId, Result[Emotiguy - 2], Offset[Emotiguy - 2], Emotiguy,PrimeChecker(Result[Emotiguy - 2], Emotiguy), PrimeChecker(Offset[Emotiguy - 2], Emotiguy));
        }
        Fuckyou[Emotiguy - 2] = true;
        break;
        case 4:
        if (Fuckyou[Emotiguy - 2] == false) {
          Debug.LogFormat("[Basic Morse #{0}] The page says {1} + {2} in base {3}. This shows up as {4} + {5}.", moduleId, Result[Emotiguy - 2], Offset[Emotiguy - 2], Emotiguy,PrimeChecker(Result[Emotiguy - 2], Emotiguy), PrimeChecker(Offset[Emotiguy - 2], Emotiguy));
        }
        Fuckyou[Emotiguy - 2] = true;
        break;
        case 5:
        if (Fuckyou[Emotiguy - 2] == false) {
          Debug.LogFormat("[Basic Morse #{0}] The page says {1} + {2} in base {3}. This shows up as {4} + {5}.", moduleId, Result[Emotiguy - 2], Offset[Emotiguy - 2], Emotiguy,PrimeChecker(Result[Emotiguy - 2], Emotiguy), PrimeChecker(Offset[Emotiguy - 2], Emotiguy));
        }
        Fuckyou[Emotiguy - 2] = true;
        break;
        case 6:
        if (Fuckyou[Emotiguy - 2] == false) {
          Debug.LogFormat("[Basic Morse #{0}] The page says {1} + {2} in base {3}. This shows up as {4} + {5}.", moduleId, Result[Emotiguy - 2], Offset[Emotiguy - 2], Emotiguy,PrimeChecker(Result[Emotiguy - 2], Emotiguy), PrimeChecker(Offset[Emotiguy - 2], Emotiguy));
        }
        Fuckyou[Emotiguy - 2] = true;
        break;
        case 7:
        if (Fuckyou[Emotiguy - 2] == false) {
          Debug.LogFormat("[Basic Morse #{0}] The page says {1} + {2} in base {3}. This shows up as {4} + {5}.", moduleId, Result[Emotiguy - 2], Offset[Emotiguy - 2], Emotiguy,PrimeChecker(Result[Emotiguy - 2], Emotiguy), PrimeChecker(Offset[Emotiguy - 2], Emotiguy));
        }
        Fuckyou[Emotiguy - 2] = true;
        break;
        case 8:
        if (Fuckyou[Emotiguy - 2] == false) {
          Debug.LogFormat("[Basic Morse #{0}] The page says {1} + {2} in base {3}. This shows up as {4} + {5}.", moduleId, Result[Emotiguy - 2], Offset[Emotiguy - 2], Emotiguy,PrimeChecker(Result[Emotiguy - 2], Emotiguy), PrimeChecker(Offset[Emotiguy - 2], Emotiguy));
        }
        Fuckyou[Emotiguy - 2] = true;
        break;
        case 9:
        if (Fuckyou[Emotiguy - 2] == false) {
          Debug.LogFormat("[Basic Morse #{0}] The page says {1} + {2} in base {3}. This shows up as {4} + {5}.", moduleId, Result[Emotiguy - 2], Offset[Emotiguy - 2], Emotiguy,PrimeChecker(Result[Emotiguy - 2], Emotiguy), PrimeChecker(Offset[Emotiguy - 2], Emotiguy));
        }
        Fuckyou[Emotiguy - 2] = true;
        break;
        case 10:
        if (Fuckyou[Emotiguy - 2] == false) {
          Debug.LogFormat("[Basic Morse #{0}] The page says {1} + {2} in base {3}. This shows up as {4} + {5}.", moduleId, Result[Emotiguy - 2], Offset[Emotiguy - 2], Emotiguy,PrimeChecker(Result[Emotiguy - 2], Emotiguy), PrimeChecker(Offset[Emotiguy - 2], Emotiguy));
        }
        Fuckyou[Emotiguy - 2] = true;
        break;
      }
      AidsNumbers.text = PrimeChecker(Result[Emotiguy - 2], Emotiguy) + "\n+\n" + PrimeChecker(Offset[Emotiguy - 2], Emotiguy);
    }
    string PrimeChecker(int num, int thebase){
      if (num == 0) {
        return "0";
      }
      else {
      string thingys = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
      string newnum = "";
      int current = 0;
      while (num != 0) {
       current = num % thebase;
       newnum = thingys[current] + newnum;
       num = num / thebase;
      }
      return newnum;
    }
    }
    //I add the Twich Play
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use !{0} up/down to press the corresponding arrow, !{0} submit XXXX to submit a word.";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command){
      int WeedCheck = 0;
      command = command.Trim();
      string[] parameters = command.Split(' ');
      if (parameters.Length > 2) {
        yield return "sendtochaterror Too many words!";
      }
      else if (parameters.Length < 1) {
        yield return "sendtochaterror Too little words!";
      }
      else if (parameters[0].ToUpper() == "UP") {
        yield return null;
        ArrowsAndSubmit[0].OnInteract();
        yield break;
      }
      else if (parameters[0].ToUpper() == "DOWN") {
        yield return null;
        ArrowsAndSubmit[1].OnInteract();
        yield break;
      }
      else if (parameters[0].ToUpper() == "SUBMIT") {
        yield return null;
        for (int i = 0; i < parameters[1].Length; i++) {
          for (int j = 0; j < 26; j++) {
            if (parameters[1][i].ToString().ToUpper() == STDphabet[j].ToString().ToUpper()) {
              WeedCheck += 1;
            }
          }
        }
        if (parameters[1].Length == WeedCheck) {
          for (int i = 0; i < parameters[1].Length; i++) {
            for (int j = 0; j < 26; j++) {
              if (parameters[1][i].ToString().ToUpper() == STDphabet[j].ToString().ToUpper()) {
                Letters[j].OnInteract();
                yield return new WaitForSeconds(.1f);
              }
            }
          }
          ArrowsAndSubmit[2].OnInteract();
          WeedCheck = 0;
          ArrowsAndSubmit[0].OnInteract();
          ArrowsAndSubmit[1].OnInteract();
          yield break;
        }
        WeedCheck = 0;
      }
    }
}
