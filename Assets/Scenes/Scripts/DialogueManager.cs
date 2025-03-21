using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine.XR;
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

    Dictionary<string, Ingredient.Cook> cookMap = new Dictionary<string, Ingredient.Cook>
    {
        { "무조리", Ingredient.Cook.none },
        { "볶음요리", Ingredient.Cook.stirFry },
        { "구운요리", Ingredient.Cook.roast },
    };

    Dictionary<string, Ingredient.Base> baseMap = new Dictionary<string, Ingredient.Base>
    {
        { "밥요리", Ingredient.Base.rice },
        { "빵요리", Ingredient.Base.bread },
        { "면요리", Ingredient.Base.noodle },
    };

    Dictionary<string, Personality> personalityMap = new Dictionary<string, Personality>
    {
        { "generous", Personality.Generous},
        { "normal", Personality.Normal },
        { "picky",Personality.Picky },
        { "strict", Personality.Strict },
    };

    // 현재 랜덤 대사 출력 중?
    private bool isRandom = false;

    // 랜덤 대사 인덱스
    private int indexForRandom = 0;

    // 선택한 랜덤 대사들이 있는 배열
    RandomDialogue[] randomDialogues;

    // 랜덤 대사 다 출력하고 돌아올 ID
    string backID = "";

    // 랜덤 대사 다 출력하고 만약 바로 주문 대사가 올 경우에 사용할 플래그
    bool directOrder = false;

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

        if (dialogueDic.ContainsKey(currentID) && dialogueDic[currentID].nextDialogueID.Contains("EOD"))
        {

            _Day = int.Parse(tmp[0].Substring(1)) + 1;

            // _currentID = "D" + string.Format("{0:D2}", _Day) + "_C" + string.Format("{0:D2}", 1) + "_" + "01" + "_";
            _currentID = dialogueDic[currentID].nextDialogueID.Split(new char[] { '_' })[1].Replace('~', '_');

            if (_currentID.Contains("O"))
            {
                directOrder = true;
            }

            GameManager.instance.nextCustomer();

        }

        else if (dialogueDic.ContainsKey(currentID) && dialogueDic[currentID].nextDialogueID.Contains("EOO"))
        {

            _Customer = int.Parse(tmp[1].Substring(1)) + 1;

            //_currentID = "D" + string.Format("{0:D2}", _Day) + "_C" + string.Format("{0:D2}", _Customer) + "_" + "01" + "_";
            _currentID = dialogueDic[currentID].nextDialogueID.Split(new char[] { '_' })[1].Replace('~', '_');

            if (_currentID.Contains("O"))
            {
                directOrder = true;
            }

            GameManager.instance.nextCustomer();

        }

        else
        {
            if(isRandom == false)
                _currentID = dialogueDic[currentID].nextDialogueID;

            else if (isRandom == true && indexForRandom >= randomDialogues.Length)
            {
                indexForRandom = 0;
                isRandom = false;
                _currentID = backID;

                nextUI.gameObject.SetActive(false);
                skipUI.gameObject.SetActive(true);
            }

            // 랜덤 대사 가져와야 할 때
            if (currentID.Contains("GTR") && isRandom == false)
            {
                isRandom = true;
                int len = int.Parse(_currentID.Split(new char[] { '_' })[1]);
                backID = _currentID.Split(new char[] { '_' })[2].Replace('~', '_');
                randomDialogues = new RandomDialogue[len];
                for (int i = 0; i < len; i++)
                {
                    int randomIndex = UnityEngine.Random.Range(0, randomDialogueDic.Count);
                    Debug.Log(randomDialogueDic[randomIndex].line);
                    randomDialogues[i] = randomDialogueDic[randomIndex];
                }

                Debug.Log("랜덤 대사 가져옴!");
            }

            if (isRandom == false || directOrder == true)
            {
                directOrder = false;

                // 주문 ui 활성화
                if (_currentID.Split(new char[] { '_' })[2] == "O")
                {
                    Debug.Log("!!!!!!!!!!!!!!!");
                    nextUI.gameObject.SetActive(true);

                    string desireMeatfish = dialogueDic[currentID].desireMain.Contains("&&") ? dialogueDic[currentID].desireMain.Split("&&")[0] : "";
                    string desireVege = dialogueDic[currentID].desireMain.Contains("&&") ? dialogueDic[currentID].desireMain.Split("&&")[1] : "";
                    string desireCategory = dialogueDic[currentID].desireCategory;
                    string desirePersonality = dialogueDic[currentID].type;

                    // 성격
                    Personality personality = personalityMap.ContainsKey(desirePersonality) ? personalityMap[desirePersonality] : Personality.Generous; //성격이 지정되어 있지 않으며 기본값으로 Generous
                                                                                                                                                        // 카테고리
                    CategoryData category = categoryMap.ContainsKey(desireCategory) ? categoryMap[desireCategory] : null;

                    // 재료 선택
                    Ingredient.MeatFish meatfish = meatFishMap.ContainsKey(desireMeatfish) ? meatFishMap[desireMeatfish] : Ingredient.MeatFish.noCondition;
                    Ingredient.Vege vege = vegeMap.ContainsKey(desireVege) ? vegeMap[desireVege] : Ingredient.Vege.noCondition;
                    Ingredient.Base baseIngred = baseMap.ContainsKey(desireCategory) ? baseMap[desireCategory] : Ingredient.Base.noCondition;
                    Ingredient.Cook cook = cookMap.ContainsKey(desireCategory) ? cookMap[desireCategory] : Ingredient.Cook.noCondition;
                    Ingredient.Main Main = mainMap.ContainsKey(desireMeatfish) ? mainMap[desireMeatfish] : (mainMap.ContainsKey(desireVege) ? mainMap[desireVege] : Ingredient.Main.noCondition);

                    // 임시
                    bool hateMeatFish = false;
                    bool hateVege = false;
                    bool hateBase = false;
                    bool hateCategory = false;

                    // 임시
                    CustomerManager.instance.GetOrder(personality, true, meatfish, vege, baseIngred, cook, hateMeatFish, hateVege, hateBase);
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
                    if (currentID.Contains("&&"))
                    {
                        Debug.Log("멀티");
                        string[] chooseTmp = _currentID.Split("&&");


                        for (int i = 0; i < chooseTmp.Length; i++)
                        {
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
                    else
                    {
                        Debug.Log("싱글");
                    }
                }
            }
        }

        if (isRandom == false)
        {
            nameUI.text = dialogueDic[currentID].name;
            dialogueUI.text = dialogueDic[currentID].line.Replace('`', ',');
            Debug.Log(dialogueDic[currentID].line);
        }

        else {
            string desireMeatfish = "";
            string desireVege = "";
            string desireCategory = randomDialogues[indexForRandom].desireCategory;
            string desirePersonality = randomDialogues[indexForRandom].type;

            bool hateMeatFish = false;
            bool hateVege = false;
            bool hateBase = false;
            bool hateCategory = false;

            // 성격
            Personality personality = personalityMap.ContainsKey(desirePersonality) ? personalityMap[desirePersonality] : Personality.Generous; //성격이 지정되어 있지 않으며 기본값으로 Generous

            // 받침이 없는 글자 목록
            string[] noBatchimMeatFish = { "육류", "생선류", "돼지고기", "참치", "닭고기", "연어", "소고기" };
            string[] noBatchimVege = { "과채류", "감자", "토마토", "당근" };

            // 받침이 있는 글자 목록
            string[] withBatchimMeatFish = { };
            string[] withBatchimVege = { "버섯" };


            // 받침이 없는 글자 목록
            string[] noBatchimBase = { "밥요리", "빵요리", "면요리" };
            string[] noBatchimCook = { "볶음요리", "구운요리" };
            string[] noBatchimCategory = { "햄버거", "샌드위치", "고로케", "파이", "피자", "국수", "오븐파스타" };

            // 받침이 있는 글자 목록
            string[] withBatchimBase = { };
            string[] withBatchimCook = { };
            string[] withBatchimCategory = { "덮밥", "볶음밥", "구운주먹밥", "볶음면" };

            switch (randomDialogues[indexForRandom].desireMain)
            {
                case "$$":
                case "!$$":


                    int temp = UnityEngine.Random.Range(0, 1);

                    if (temp == 0)
                    {
                        desireMeatfish = noBatchimMeatFish[UnityEngine.Random.Range(0, noBatchimMeatFish.Length)];

                        if (randomDialogues[indexForRandom].line.Contains("밥"))
                        {
                            desireCategory = "밥요리";
                        }
                        else if (randomDialogues[indexForRandom].line.Contains("빵"))
                        {
                            desireCategory = "빵요리";
                        }
                        else if (randomDialogues[indexForRandom].line.Contains("면"))
                        {
                            desireCategory = "면요리";
                        }

                        randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("$$", desireMeatfish);

                        if (randomDialogues[indexForRandom].desireMain[0] == '!') {
                            hateMeatFish = true;
                        }
                    }
                    else {
                        desireVege = noBatchimVege[UnityEngine.Random.Range(0, noBatchimVege.Length)];

                        if (randomDialogues[indexForRandom].line.Contains("밥"))
                        {
                            desireCategory = "밥요리";
                        }
                        else if (randomDialogues[indexForRandom].line.Contains("빵"))
                        {
                            desireCategory = "빵요리";
                        }
                        else if (randomDialogues[indexForRandom].line.Contains("면"))
                        {
                            desireCategory = "면요리";
                        }

                        randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("$$", desireVege);
                        if (randomDialogues[indexForRandom].desireMain[0] == '!')
                        {
                            hateVege = true;
                        }
                    }
                    
                    break;

                case "%%":
                case "!%%":
                    desireVege = withBatchimVege[UnityEngine.Random.Range(0, withBatchimVege.Length)];

                    if (randomDialogues[indexForRandom].line.Contains("밥"))
                    {
                        desireCategory = "밥요리";
                    }
                    else if (randomDialogues[indexForRandom].line.Contains("빵"))
                    {
                        desireCategory = "빵요리";
                    }
                    else if (randomDialogues[indexForRandom].line.Contains("면"))
                    {
                        desireCategory = "면요리";
                    }

                    randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("%%", desireVege);
                    if (randomDialogues[indexForRandom].desireMain[0] == '!')
                    {
                        hateVege = true;
                    }
                    break;

                default:
                    break;
            }


            switch (randomDialogues[indexForRandom].desireCategory)
            {
                case "##":
                case "!##":


                    int temp = UnityEngine.Random.Range(0, 3);

                    if (temp == 0)
                    {
                        desireCategory =  noBatchimBase[UnityEngine.Random.Range(0, noBatchimBase.Length)];

                        if (randomDialogues[indexForRandom].desireCategory[0] == '!')
                        {
                            hateBase = true;
                        }
                    }

                    else if(temp == 1) {
                        desireCategory = noBatchimCook[UnityEngine.Random.Range(0, noBatchimCook.Length)];
                    }

                    else
                    {
                        desireCategory = noBatchimCategory[UnityEngine.Random.Range(0, noBatchimCategory.Length)];
                        if (randomDialogues[indexForRandom].desireCategory[0] == '!')
                        {
                            hateCategory = true;
                        }
                    }

                    randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("##", desireCategory);
                    break;

                case "@@":
                case "!@@":
                    desireCategory = withBatchimCategory[UnityEngine.Random.Range(0, withBatchimCategory.Length)];
                    randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("@@", desireCategory);
                    break;

                default:
                    break;
            }

            if (randomDialogues[indexForRandom].line.Contains("**")) 
            {

                int temp = UnityEngine.Random.Range(0, 7);

                switch (temp)
                {
                    case 0:
                        desireMeatfish = noBatchimMeatFish[UnityEngine.Random.Range(0, noBatchimMeatFish.Length)];
                        randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("**", desireMeatfish);
                        break;

                    case 1:
                        desireVege = noBatchimVege[UnityEngine.Random.Range(0, noBatchimVege.Length)];
                        randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("**", desireVege);
                        break;

                    case 2:
                        desireVege = withBatchimVege[UnityEngine.Random.Range(0, withBatchimVege.Length)];
                        randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("**", desireVege);
                        break;

                    case 3:
                        desireCategory = noBatchimBase[UnityEngine.Random.Range(0, noBatchimBase.Length)];
                        randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("**", desireCategory);
                        break;

                    case 4:
                        desireCategory = noBatchimCook[UnityEngine.Random.Range(0, noBatchimCook.Length)];
                        randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("**", desireCategory);
                        break;

                    case 5:
                        desireCategory = noBatchimCategory[UnityEngine.Random.Range(0, noBatchimCategory.Length)];
                        randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("**", desireCategory);
                        break;

                    case 6:
                        desireCategory = withBatchimCategory[UnityEngine.Random.Range(0, withBatchimCategory.Length)];
                        randomDialogues[indexForRandom].line = randomDialogues[indexForRandom].line.Replace("**", desireCategory);
                        break;

                    default:
                        Debug.Log("Invalid category selected.");
                        break;
                }


            }

            CategoryData category = categoryMap.ContainsKey(desireCategory + "\r") ? categoryMap[desireCategory + "\r"] : null;
            Debug.Log(category);

            // 재료 선택
            Ingredient.MeatFish meatfish = meatFishMap.ContainsKey(desireMeatfish) ? meatFishMap[desireMeatfish] : Ingredient.MeatFish.noCondition;
            Ingredient.Vege vege = vegeMap.ContainsKey(desireVege) ? vegeMap[desireVege] : Ingredient.Vege.noCondition;
            Ingredient.Base baseIngred = baseMap.ContainsKey(desireCategory) ? baseMap[desireCategory] : Ingredient.Base.noCondition;
            Ingredient.Cook cook = cookMap.ContainsKey(desireCategory) ? cookMap[desireCategory] : Ingredient.Cook.noCondition;
            Ingredient.Main Main = mainMap.ContainsKey(desireMeatfish) ? mainMap[desireMeatfish] : (mainMap.ContainsKey(desireVege) ? mainMap[desireVege] : Ingredient.Main.noCondition);


            if (meatfish != Ingredient.MeatFish.noCondition || vege != Ingredient.Vege.noCondition)
            {
                if (baseIngred != Ingredient.Base.noCondition || cook != Ingredient.Cook.noCondition)
                {
                    Debug.Log(1);
                    CustomerManager.instance.GetOrder(personality, false, meatfish, vege, baseIngred, cook, hateMeatFish, hateVege, hateBase);
                }

                else if (category != null)
                {
                    Debug.Log(2);
                    CustomerManager.instance.GetOrder(personality, false, meatfish, vege, category, hateMeatFish, hateVege, hateBase);
                }

                else {
                    Debug.Log(3);
                    CustomerManager.instance.GetOrder(personality, false, meatfish, vege, baseIngred, cook, hateMeatFish, hateVege, hateBase);
                }
            }

            else if (Main != Ingredient.Main.noCondition)
            {
                if (baseIngred != Ingredient.Base.noCondition || cook != Ingredient.Cook.noCondition)
                {
                    Debug.Log(4);
                    CustomerManager.instance.GetOrder(personality, false, Main, baseIngred, cook, hateCategory, hateBase);
                }
                else if (category != null)
                {
                    Debug.Log(5);
                    CustomerManager.instance.GetOrder(personality, false, Main, category, hateCategory, hateBase);
                }

                else {
                    Debug.Log(6);
                    CustomerManager.instance.GetOrder(personality, false, Main, baseIngred, cook, hateCategory, hateBase);
                }
            }

            else
            {
                Debug.Log("오류");
                CustomerManager.instance.GetOrder(personality, false, meatfish, vege, baseIngred, cook, hateMeatFish, hateVege, hateBase);
            }

            nameUI.text = "손님";
            dialogueUI.text = randomDialogues[indexForRandom].line.Replace('`', ',');
            nextUI.gameObject.SetActive(true);
            skipUI.gameObject.SetActive(false);

            indexForRandom++;
            _Customer++;

        }

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
