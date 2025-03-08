using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Ingredient;
using static UnityEngine.Rendering.DebugUI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] TextAsset StoryCSV;
    [SerializeField] TextAsset RandomCSV;
    //string csv_FileName;

    [SerializeField] TextMeshProUGUI nameUI;
    [SerializeField] TextMeshProUGUI dialogueUI;
    [SerializeField] RectTransform nextUI;
    [SerializeField] RectTransform skipUI;
    [SerializeField] RectTransform multipleskipUI;


    Dictionary<string, Dialogue> dialogueDic = new Dictionary<string, Dialogue>();
    Dictionary<int, RandomDialogue> randomDialogueDic = new Dictionary<int, RandomDialogue>();

    public static bool isFinish = false;

    [SerializeField]
    private int _Day;
    public int Day { get { return _Day; } }

    [SerializeField]
    private int _Customer;
    public int Customer { get { return _Customer; } }

    [SerializeField]
    private string _Sequence;
    public string Sequence { get { return _Sequence; } }

    [SerializeField]
    private string _Multiple;
    public string Multiple { get { return _Multiple; } }

    [SerializeField] 
    private string _currentID;
    public string currentID { get { return _currentID; } }

    [SerializeField]
    private string _currentDialogue;
    public string currentDialogue {  get { return _currentDialogue; } }

    Dictionary<string, Ingredient.MeatFish> meatFishMap = new Dictionary<string, Ingredient.MeatFish>
    {
        { "돼지고기", Ingredient.MeatFish.pork },
        { "참치", Ingredient.MeatFish.tuna },
        { "닭고기", Ingredient.MeatFish.chicken },
        { "연어", Ingredient.MeatFish.tuna },
        { "소고기", Ingredient.MeatFish.beef },
        { "없음", Ingredient.MeatFish.none }
    };

    Dictionary<string, Ingredient.Vege> vegeMap = new Dictionary<string, Ingredient.Vege>
    {
        { "감자", Ingredient.Vege.potato },
        { "토마토", Ingredient.Vege.tomato },
        { "버섯", Ingredient.Vege.mushroom },
        { "당근", Ingredient.Vege.carrot },
        { "없음", Ingredient.Vege.none }
    };

    Dictionary<string, CategoryData> categoryMap;

    Dictionary<string, Ingredient.Main> mainMap = new Dictionary<string, Ingredient.Main>
    {
        { "육류", Ingredient.Main.meat },
        { "생선류", Ingredient.Main.fish },
        { "과채류", Ingredient.Main.vege },
    };

    Dictionary<string, Personality> personalityMap = new Dictionary<string, Personality>
    {
        { "generous", Personality.Generous},
        { "normal", Personality.Normal },
        { "picky",Personality.Picky },
        { "strict", Personality.Strict },
    };

    private void Awake()
    {
        Instance = this;

        categoryMap = new Dictionary<string, CategoryData>
        {
            {"덮밥\r", RecipeManager.instance.getCategoryListDatas()[0].categoryData },
            {"볶음밥\r", RecipeManager.instance.getCategoryListDatas()[1].categoryData },
            {"구운주먹밥\r", RecipeManager.instance.getCategoryListDatas()[2].categoryData },
            {"햄버거\r", RecipeManager.instance.getCategoryListDatas()[3].categoryData },
            {"샌드위치\r", RecipeManager.instance.getCategoryListDatas()[3].categoryData },
            {"고로케\r", RecipeManager.instance.getCategoryListDatas()[4].categoryData },
            {"파이\r", RecipeManager.instance.getCategoryListDatas()[4].categoryData },
            {"피자\r", RecipeManager.instance.getCategoryListDatas()[5].categoryData },
            {"국수\r", RecipeManager.instance.getCategoryListDatas()[6].categoryData },
            {"볶음면\r", RecipeManager.instance.getCategoryListDatas()[7].categoryData },
            {"오븐파스타\r", RecipeManager.instance.getCategoryListDatas()[8].categoryData },
        };

        _Day = 0;
        _Customer = 1;
        _Sequence = string.Format("{0:D2}", 1);

        _currentID = "D" + string.Format("{0:D2}", _Day) + "_C" + string.Format("{0:D2}", _Customer) + "_" + _Sequence + "_";

        DialogueParser theParser = gameObject.GetComponent<DialogueParser>();
        
        Dialogue[] dialogues = theParser.Parse(StoryCSV);

        for (int i = 0; i < dialogues.Length; i++)
        {
            dialogueDic.Add(dialogues[i].dialogueID, dialogues[i]);
        }
        isFinish = true;

        nameUI.text = dialogueDic[currentID].name;
        dialogueUI.text = dialogueDic[currentID].line.Replace('`', ','); ;
        Debug.Log(dialogueDic[currentID].line);



        RandomDialogue[] randomDialogues = theParser.RandomParse(RandomCSV);

        for (int i = 0; i < randomDialogues.Length; i++)
        {
            randomDialogueDic.Add(i, randomDialogues[i]);
        }

    }

    public void GetNextDialogue()
    {
        string[] tmp = _currentID.Split(new char[] { '_' });

        if (dialogueDic[currentID].nextDialogueID == "EOD")
        {

            _Day = int.Parse(tmp[0].Substring(1)) + 1;

            _currentID = "D" + string.Format("{0:D2}", _Day) + "_C" + string.Format("{0:D2}", 1) + "_" + "01" + "_";

            GameManager.instance.nextCustomer();

        }

        else if (dialogueDic[currentID].nextDialogueID == "EOO")
        {

            _Customer = int.Parse(tmp[1].Substring(1)) + 1;

            _currentID = "D" + string.Format("{0:D2}", _Day) + "_C" + string.Format("{0:D2}", _Customer) + "_" + "01" + "_";

            GameManager.instance.nextCustomer();
        }

        else
        {
            _currentID = dialogueDic[currentID].nextDialogueID;

            // 주문 ui 활성화
            if (_currentID.Split(new char[] { '_' })[2] == "O")
            {
                nextUI.gameObject.SetActive(true);

                string desireMeatfish = dialogueDic[currentID].desireMain.Contains("&&") ? dialogueDic[currentID].desireMain.Split("&&")[0] : "";
                string desireVege = dialogueDic[currentID].desireMain.Contains("&&") ? dialogueDic[currentID].desireMain.Split("&&")[1] : "";
                string desireCategory = dialogueDic[currentID].desireCategory;
                string desirePersonality = dialogueDic[currentID].type;

                // 성격
                Personality personality = personalityMap.ContainsKey(desirePersonality) ? personalityMap[desirePersonality] : Personality.Generous; //t성격이 지정되어 있지 않으며 기본값으로 Generous
                // 카테고리
                CategoryData category = categoryMap.ContainsKey(desireCategory) ? categoryMap[desireCategory] : null;

                // 재료 선택
                Ingredient.MeatFish meatfish = meatFishMap.ContainsKey(desireMeatfish) ? meatFishMap[desireMeatfish] : Ingredient.MeatFish.noCondition;
                Ingredient.Vege vege = vegeMap.ContainsKey(desireVege) ? vegeMap[desireVege] : Ingredient.Vege.noCondition;
                Ingredient.Base baseIngred =  category != null ? category.baseIngred : Ingredient.Base.noCondition;
                Ingredient.Cook cook = category != null ? category.cook : Ingredient.Cook.noCondition;
                Ingredient.Main Main = mainMap.ContainsKey(desireMeatfish) ?  mainMap[desireMeatfish] : (mainMap.ContainsKey(desireVege) ? mainMap[desireVege] : Ingredient.Main.noCondition);

                // 임시
                bool hateMeatFish = false;
                bool hateVege = false;
                bool hateBase = false;
                bool hateCategory = false;

                // 임시
                CustomerManager.instance.GetOrder(personality, meatfish, vege, baseIngred, cook, hateMeatFish, hateVege, hateBase);
                /*
                if (meatfish != Ingredient.MeatFish.noCondition )
                {
                    CustomerManager.instance.GetOrder(personality, meatfish, vege, baseIngred, cook, hateMeatFish, hateVege, hateBase);
                }
                else
                {
                    CustomerManager.instance.GetOrder(personality, Main, baseIngred, cook, hateCategory, hateBase);
                }
                */

                skipUI.gameObject.SetActive(false);
            }

            // 다중 선택 ui 활성화
            if (currentID.Contains("&&") && !currentID.Contains("R"))
            {
                string[] chooseTmp = _currentID.Split("&&");

                multipleskipUI.gameObject.SetActive(true);
                skipUI.gameObject.SetActive(false);

                GameObject firstChoose = multipleskipUI.GetChild(0).gameObject.GetComponent<RectTransform>().GetChild(0).gameObject;
                firstChoose.GetComponent<TextMeshProUGUI>().text = dialogueDic[chooseTmp[0]].line;
                GameObject secondChoose = multipleskipUI.GetChild(1).gameObject.GetComponent<RectTransform>().GetChild(0).gameObject;
                secondChoose.GetComponent<TextMeshProUGUI>().text = dialogueDic[chooseTmp[1]].line;
                return;
            }


            if (currentID.Contains("R"))
            {
                string temp = currentID;
                // 다중 반응 처리
                if (currentID.Contains("&&")) {
                    Debug.Log("멀티");
                    string[] chooseTmp = _currentID.Split("&&");


                    for (int i = 0; i < chooseTmp.Length; i++) {
                        if (CookManager.instance.satisfiedType == CookManager.Result.positive && dialogueDic[chooseTmp[i]].type == "positive")
                        {
                            _currentID = chooseTmp[i];
                            break;
                        }
                        else if (CookManager.instance.satisfiedType == CookManager.Result.neutral && dialogueDic[chooseTmp[i]].type == "neutral")
                        {
                            _currentID = chooseTmp[i];
                            break;
                        }
                        else if (CookManager.instance.satisfiedType == CookManager.Result.negative && (dialogueDic[chooseTmp[i]].type == "negative" || dialogueDic[chooseTmp[i]].type == "s-negative"))
                        {
                            _currentID = chooseTmp[i];
                            break;
                        }
                    }

                    if (temp == _currentID)
                        Debug.LogWarning("해당되는 반응 대사가 없습니다.");

                    nameUI.text = dialogueDic[currentID].name;
                    dialogueUI.text = dialogueDic[currentID].line.Replace('`', ',');
                    return;
                }
                // 단일 반응 처리
                else {
                    Debug.Log("싱글");
                }
            }
        }

        nameUI.text = dialogueDic[currentID].name;
        dialogueUI.text = dialogueDic[currentID].line.Replace('`', ',');
        Debug.Log(dialogueDic[currentID].line);

    }

    public void GetNextDialogueMultiple(string index)
    {
        string[] chooseTmp = _currentID.Split("&&");

        if (index == "a")
            _currentID = chooseTmp[0];
        if (index == "b")
            _currentID = chooseTmp[1];

        multipleskipUI.gameObject.SetActive(false);
        skipUI.gameObject.SetActive(true);
    }
}
