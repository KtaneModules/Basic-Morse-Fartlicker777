using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicMorse : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMAudio Audio;
   public KMSelectable[] Letters;
   public KMSelectable[] ArrowsAndSubmit;
   public TextMesh AidsNumbers;

   string[] WordsOfAids = { "ACID", "BUST", "CODE", "DAZE", "ECHO", "FILM", "GOLF", "HUNT", "ITCH", "JURY", "KING", "LIME", "MONK", "NUMB", "ONLY", "PREY", "QUIT", "RAVE", "SIZE", "TOWN", "URGE", "VERY", "WAXY", "XYLO", "YARD", "ZERO", "ABORT", "BLEND", "CRYPT", "DWARF", "EQUIP", "FANCY", "GIZMO", "HELIX", "IMPLY", "JOWLS", "KNIFE", "LEMON", "MAJOR", "NIGHT", "OVERT", "POWER", "QUILT", "RUSTY", "STOMP", "TRASH", "UNTIL", "VIRUS", "WHISK", "XERIC", "YACHT", "ZEBRA", "ADVICE", "BUTLER", "CAVITY", "DIGEST", "ELBOWS", "FIXURE", "GOBLET", "HANDLE", "INDUCT", "JOKING", "KNEADS", "LENGTH", "MOVIES", "NIMBLE", "OBTAIN", "PERSON", "QUIVER", "RACHET", "SAILOR", "TRANCE", "UPHELD", "VANISH", "WALNUT", "XYLOSE", "YANKED", "ZODIAC", "ALREADY", "BROWSED", "CAPITOL", "DESTROY", "ERASING", "FLASHED", "GRIMACE", "HIDEOUT", "INFUSED", "JOYRIDE", "KETCHUP", "LOCKING", "MAILBOX", "NUMBERS", "OBSCURE", "PHANTOM", "QUIETLY", "REFUSAL", "SUBJECT", "TRAGEDY", "UNKEMPT", "VENISON", "WARSHIP", "XANTHIC", "YOUNGER", "ZEPHYRS", "ADVOCATE", "BACKFLIP", "CHIMNEYS", "DISTANCE", "EXPLOITS", "FOCALIZE", "GIFTWRAP", "HOVERING", "INVENTOR", "JEALOUSY", "KINSFOLK", "LOCKABLE", "MERCIFUL", "NOTECARD", "OVERCAST", "PERILOUS", "QUESTION", "RAINCOAT", "STEALING", "TREASURY", "UPDATING", "VERTICAL", "WISHBONE", "XENOLITH", "YEARLONG", "ZEALOTRY", "ABHORRENT", "BACCARATS", "CULTIVATE", "DAMNINGLY", "EFFLUXION", "FUTURISTS", "GYROSCOPE", "HAZARDOUS", "ILLOGICAL", "JUXTAPOSE", "KILOBYTES", "LANTHANUM", "MATERIALS", "NIHILISTS", "OBSCENITY", "PAINFULLY", "QUEERNESS", "RESTROOMS", "SABOTAGED", "TYRANNOUS", "UMPTEENTH", "VEXILLATE", "WAYLAYERS", "XENOBLAST", "YTTERBIUM", "ZIGZAGGER" };
   string QWERTYPhabet = "QWERTYUIOPASDFGHJKLZXCVBNM";
   string StandardAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
   readonly int[] WordsButBaseTen = { 7, 27, 30, 9, 2, 57, 12, 54, 6, 67, 10, 63, 4, 3, 13, 66, 37, 21, 18, 1, 19, 55, 22, 28, 31, 36 };
   int WordPicker = 0;
   string SaintKildaFan = "";
   int BaseIndex = 2;
   private List<int> Starting = new List<int> { };
   private List<int> Offset = new List<int> { };
   private List<int> Result = new List<int> { };
   string CasualBattlesOfTheCongo = "";
   static int moduleIdCounter = 1;
   int moduleId;
   private bool moduleSolved;

   void Awake() {
      moduleId = moduleIdCounter++;

      foreach (KMSelectable Letter in Letters) {
         Letter.OnInteract += delegate () { LetterPress(Letter); return false; };
      }
      foreach (KMSelectable Arrow in ArrowsAndSubmit) {
         Arrow.OnInteract += delegate () { ArrowPress(Arrow); return false; };
      }
   }

   void Start() {
      WordPicker = UnityEngine.Random.Range(0, WordsOfAids.Length);
      SaintKildaFan = WordsOfAids[WordPicker];
      for (int i = 0; i < SaintKildaFan.Length; i++) {
         for (int j = 0; j < 26; j++) {
            if (SaintKildaFan[i] == StandardAlphabet[j]) {
               Starting.Add(WordsButBaseTen[j]);
               Offset.Add(UnityEngine.Random.Range(0, WordsButBaseTen[j]));
               Result.Add(Starting.Last() - Offset.Last());
            }
         }
      }
      Debug.LogFormat("[Basic Morse #{0}] The word chosen is {1}.", moduleId, SaintKildaFan);
      AidsNumbers.text = Larger();
      StartCoroutine(Logger());
   }

   IEnumerator Logger () { //It's 2:30 am and I can't think of a better way to do this.
      for (int i = 0; i < SaintKildaFan.Length; i++) {
         Debug.LogFormat("[Basic Morse #{0}] The page says {1} + {2} in base {3}. This shows up as {4}.", moduleId, Result[i], Offset[i], i + 2, Larger().Replace('\n', ' '));
         ArrowsAndSubmit[0].OnInteract();
         yield return new WaitForSecondsRealtime(.01f);
      }
   }

   void LetterPress (KMSelectable Letter) {
      Letter.AddInteractionPunch();
      Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Letter.transform);
      if (moduleSolved || CasualBattlesOfTheCongo.Length == 9) {
         return;
      }
      for (int i = 0; i < 26; i++) {
         if (Letter == Letters[i]) {
            CasualBattlesOfTheCongo += QWERTYPhabet[i];
            AidsNumbers.text = CasualBattlesOfTheCongo;
         }
         switch (CasualBattlesOfTheCongo.Length) {
            case 8:
               AidsNumbers.fontSize = 265;
               break;
            case 9:
               AidsNumbers.fontSize = 230;
               break;
            default:
               AidsNumbers.fontSize = 300;
               break;
         }
      }
   }

   void ArrowPress(KMSelectable Arrow) {
      Arrow.AddInteractionPunch();
      Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Arrow.transform);
      if (moduleSolved) {
         return;
      }
      if (Arrow == ArrowsAndSubmit[0]) {
         BaseIndex++;
         if (BaseIndex - 1 > SaintKildaFan.Length) {
            BaseIndex = 2;
         }
         CasualBattlesOfTheCongo = String.Empty;
         AidsNumbers.text = Larger();
      }
      else if (Arrow == ArrowsAndSubmit[1]) {
         BaseIndex--;
         if (BaseIndex < 2) {
            BaseIndex = SaintKildaFan.Length + 1;
         }
         CasualBattlesOfTheCongo = String.Empty;
         AidsNumbers.text = Larger();
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

   string BaseConverter (int num, int thebase) {
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

   string Larger() {
      string First = BaseConverter(Result[BaseIndex - 2], BaseIndex);
      string Second = BaseConverter(Offset[BaseIndex - 2], BaseIndex);
      if (First.Length > Second.Length) {
         do {
            Second = "0" + Second;
         } while (Second.Length < First.Length);
      }
      else if (First.Length < Second.Length) {
         do {
            First = "0" + First;
         } while (Second.Length > First.Length);
      }
      AidsNumbers.fontSize = 300;
      return First + "\n+\n" + Second;
   }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} up/down to press the corresponding arrow, !{0} submit XXXX to submit a word.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string command) {
      int WeedCheck = 0;
      string[] parameters = command.ToUpper().Trim().Split(' ');
      yield return null;
      if (parameters.Length > 2 || parameters.Length < 1) {
         yield return "sendtochaterror I don't understand!";
      }
      else if (parameters[0] == "UP") {
         ArrowsAndSubmit[0].OnInteract();
      }
      else if (parameters[0] == "DOWN") {
         ArrowsAndSubmit[1].OnInteract();
      }
      else if (parameters[0] == "SUBMIT") {
         for (int i = 0; i < parameters[1].Length; i++) {
            for (int j = 0; j < 26; j++) {
               if (parameters[1][i].ToString().ToUpper() == QWERTYPhabet[j].ToString().ToUpper()) {
                  WeedCheck++;
               }
            }
         }
         if (parameters[1].Length == WeedCheck) {
            for (int i = 0; i < parameters[1].Length; i++) {
               for (int j = 0; j < 26; j++) {
                  if (parameters[1][i].ToString().ToUpper() == QWERTYPhabet[j].ToString().ToUpper()) {
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

   IEnumerator TwitchHandleForcedSolve () {
      yield return ProcessTwitchCommand("submit " + SaintKildaFan);
   }
}
