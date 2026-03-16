using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine.XR;
using static Ingredient;
using static UnityEngine.Rendering.DebugUI;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public event Action<string> DialogueSetEvent;
    public event Action<string, string> MultipleTextSetEvent;
    public Action SpriteSetEvent;
    // 조리 결과에 따른 플레이어 반응 이벤트
    public Action<string> CustomerReactionSetEvent;
    // 날짜 변경시의 이벤트
    public event Action<int> DayPassEvent;
    // 페이드 이벤트
    public event Action<int> FadeEvent;

    [Header("CSV")]
    [SerializeField] TextAsset StoryCSV;
    [SerializeField] TextAsset RandomCSV;
    [SerializeField] TextAsset RandomReactionCSV;
    [SerializeField] TextAsset HappyEndingCSV;
    [SerializeField] TextAsset NormalEndingCSV;
    [SerializeField] TextAsset BadEndingCSV;
    //string csv_FileName;

    [Header("UI")]
    [SerializeField] RectTransform portraitUI;
    [SerializeField] TextMeshProUGUI nameUI;
    [SerializeField] TextMeshProUGUI dialogueUI;
    [SerializeField] RectTransform nextUI;
    [SerializeField] RectTransform skipUI;
    [SerializeField] RectTransform multipleskipUI;
    [SerializeField] RectTransform reactionUI;
    [SerializeField] RectTransform letterUI;


    public TextMeshProUGUI getDialogueUI { get { return dialogueUI; } }
    public RectTransform getReactionUI { get { return reactionUI; } }

    Dictionary<string, Dialogue> dialogueDic = new Dictionary<string, Dialogue>();
    Dictionary<int, RandomDialogue> randomDialogueDic = new Dictionary<int, RandomDialogue>();
    Dictionary<int, RandomReactionDialogue[]> randomReactionDialogueDic = new Dictionary<int, RandomReactionDialogue[]>();
    Dictionary<string, EndingDialogue> endingDialogueDic = new Dictionary<string, EndingDialogue>();


    [SerializeField] private bool _isFinish = false;
    public bool isFinish { get { return _isFinish; } }

    [Header("Dialogue Data")]
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
    private string _pastID;
    public string pastID { get { return _pastID; } }

    [SerializeField]
    private string _currentID;
    public string currentID { get { return _currentID; } }

    [SerializeField]
    private string _currentDialogue;
    public string currentDialogue { get { return _currentDialogue; } }

    Dictionary<string, Ingredient.MeatFish> meatFishMap = new Dictionary<string, Ingredient.MeatFish>
    {
        { "돼지고기", Ingredient.MeatFish.pork },
        { "참치", Ingredient.MeatFish.tuna },
        { "닭고기", Ingredient.MeatFish.chicken },
        { "연어", Ingredient.MeatFish.salmon },
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
    [SerializeField]
    private bool _isRandom = false;
    public bool IsRandom { get => _isRandom; }

    // 랜덤 대사 인덱스
    private int indexForRandom = 0;

    // 선택한 랜덤 대사들이 있는 배열
    RandomDialogue[] randomDialogues;

    // 랜덤 대사 다 출력하고 돌아올 ID
    string _backID = "";
    public string backID { get { return _backID; } }

    // 랜덤 대사 다 출력하고 만약 바로 주문 대사가 올 경우에 사용할 플래그
    bool directOrder = false;


    // 받침이 없는 글자 목록
    string[] noBatchimMeatFish = { "육류", "생선류", "돼지고기", "참치", "닭고기", "연어", "소고기" };
    string[] noBatchimVege = { "과채류", "감자", "토마토", "당근" };

    // 받침이 있는 글자 목록
    string[] withBatchimMeatFish = { };
    string[] withBatchimVege = { "버섯" }; // 버섯이 해금되기 전에는 접근하면 안됨


    // 받침이 없는 글자 목록
    string[] noBatchimBase = { "밥요리", "빵요리", "면요리" };
    string[] noBatchimCook = { "볶음요리", "구운요리" };
    string[] noBatchimCategory = { "햄버거", "샌드위치", "고로케", "파이", "피자", "국수", "오븐파스타" };

    // 받침이 있는 글자 목록
    string[] withBatchimBase = { };
    string[] withBatchimCook = { };
    string[] withBatchimCategory = { "덮밥", "볶음밥", "구운주먹밥", "볶음면" };


    EndingDialogue[] endingDialogues;
    DialogueParser theParser;

    // 재생시킬 엔딩 트랙명
    string endingTrack;

    private void Awake()
    {
        Instance = this;

        MultipleTextSetEvent += multipleskipUI.GetComponent<MultipleSkipController>().SetText;
        CustomerReactionSetEvent += reactionUI.GetComponent<CustomerReactionChangeController>().setReactionImage;

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

        theParser = gameObject.GetComponent<DialogueParser>();

        Dialogue[] dialogues = theParser.Parse(StoryCSV);

        for (int i = 0; i < dialogues.Length; i++)
        {
            dialogueDic.Add(dialogues[i].dialogueID, dialogues[i]);
        }
        //isFinish = true;

        //nameUI.text = dialogueDic[currentID].name;
        //dialogueUI.text = dialogueDic[currentID].line.Replace('`', ','); ;
        //Debug.Log(dialogueDic[currentID].line);



        RandomDialogue[] randomDialogues = theParser.RandomParse(RandomCSV);

        for (int i = 0; i < randomDialogues.Length; i++)
        {
            randomDialogueDic.Add(i, randomDialogues[i]);
        }


        RandomReactionDialogue[][] randomReactionDialogues = theParser.RandomReactionParse(RandomReactionCSV);


        for (int i = 0; i < randomReactionDialogues.Length; i++)
        {

            randomReactionDialogueDic.Add(i, randomReactionDialogues[i]);
            Debug.Log(randomReactionDialogues[i].Length);

        }

    }

    private void Start()
    {
        nameUI.text = dialogueDic[currentID].name;
        dialogueSet(dialogueDic[currentID].line.Replace('`', ','));

        GameManager.instance.Order_Canvas.Find("Background").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/New/" + "Main_restaurant");

    }

    private void GetStoryDialogueID()
    {
        // 다음 Dialogue id 가져오기(두 개의 if문은 첫번째 대사에서 바로 주문을 하는 경우를 위함)
        if (dialogueDic.ContainsKey(currentID) && dialogueDic[currentID].nextDialogueID.Contains("EOD"))
        {
            GameManager.instance.Fade_Panel.gameObject.SetActive(true);
            DayPassEvent(1);

            _currentID = dialogueDic[currentID].nextDialogueID.Split(new char[] { '_' })[1].Replace('~', '_');

            if (_currentID.Contains("O"))
            {
                directOrder = true;
            }

            if (_pastID.Split(new char[] { '_' })[1] != "C00")
                GameManager.instance.nextCustomer();
        }

        else if (dialogueDic.ContainsKey(currentID) && dialogueDic[currentID].nextDialogueID.Contains("EOO"))
        {
            _currentID = dialogueDic[currentID].nextDialogueID.Split(new char[] { '_' })[1].Replace('~', '_');

            if (_currentID.Contains("O"))
            {
                directOrder = true;
            }

            if (_pastID.Split(new char[] { '_' })[1] != "C00")
                GameManager.instance.nextCustomer();
        }
        else
            _currentID = dialogueDic[currentID].nextDialogueID;
    }

    private void GetRandomDialogueID()
    {
        if (_isRandom == true)
        {
            // 랜덤 대사 출력 완료 됬을 때의 처리
            if (indexForRandom >= randomDialogues.Length)
            {
                indexForRandom = 0;
                _isRandom = false;
                //SpriteSetEvent();
                _currentID = backID;

                GameManager.instance.Fade_Panel.gameObject.SetActive(true);
                DayPassEvent(1);

                if (_pastID.Split(new char[] { '_' })[1] != "C00")
                    GameManager.instance.nextCustomer();

                // 첫번째 대사에서 바로 주문을 하는 경우
                if (_currentID.Contains("O"))
                {
                    directOrder = true;
                }
                else
                {
                    nextUI.gameObject.SetActive(false);
                    skipUI.gameObject.SetActive(true);
                }
            }
        }

        // 랜덤 대사를 처음으로 가져와야 할 때
        if (currentID.Contains("GTR") && _isRandom == false)
        {
            CustomerManager.instance.ResetSpriteDuplicationPool();

            _isRandom = true;
            SpriteSetEvent();
            _backID = _currentID.Split(new char[] { '_' })[1].Replace('~', '_');

            int len;

            string tmp1 = pastID.Split(new char[] { '_' })[1];
            string tmp2 = backID.Split(new char[] { '_' })[1];
            if (int.Parse(tmp1.Substring(1, tmp1.Length - 1)) < int.Parse(tmp2.Substring(1, tmp2.Length - 1)))
            {
                len = int.Parse(tmp2.Substring(1, tmp2.Length - 1)) - int.Parse(tmp1.Substring(1, tmp1.Length - 1)) - 1;
            }
            else
                len = GameManager.instance.GetCustomerNum(GameManager.instance.day) - GameManager.instance.customerNum;
            Debug.Log(len + "명 랜덤 가져옴");


            randomDialogues = new RandomDialogue[len];

            for (int i = 0; i < len; i++)
            {

                String selectedDialogue;
                int randomIndex;
                do
                {
                    randomIndex = UnityEngine.Random.Range(0, randomDialogueDic.Count);
                    selectedDialogue = randomDialogueDic[randomIndex].line;

                } while ((selectedDialogue.Contains("%%") && GameManager.instance.day < 4) ||
                          (selectedDialogue.Contains("면") && GameManager.instance.day < 5)
                          );
                //  버섯 해금 전에 string[] withBatchimVege = { "버섯" }; 접근하는 것을 막기 위해 필요
                //  면 해금 전에 "면이랑 ~ 가 잘 어울린대요." 나오는 것을 막기 위해 필요

                Debug.Log(randomDialogueDic[randomIndex].line + " | " + System.Guid.NewGuid());
                randomDialogues[i] = randomDialogueDic[randomIndex];
            }
            // 첫번째 대사에서 바로 주문을 하는 경우
            if (_currentID.Contains("O"))
            {
                directOrder = true;
            }
            Debug.Log(directOrder);
            Debug.Log("랜덤 대사 가져옴!");
        }

    }

    private void GetStoryOrder()
    {
        directOrder = false;
        Debug.Log(_currentID);
        // 주문이 필요한 경우면 주문 과정 처리
        if (_currentID.Split(new char[] { '_' })[2] == "O")
        {
            nextUI.gameObject.SetActive(true);

            string desireMeatfish = dialogueDic[currentID].desireMain.Contains("&&") ? dialogueDic[currentID].desireMain.Split("&&")[0] : "";
            string desireVege = dialogueDic[currentID].desireMain.Contains("&&") ? dialogueDic[currentID].desireMain.Split("&&")[1] : "";
            string desireCategory = dialogueDic[currentID].desireCategory;
            string desirePersonality = dialogueDic[currentID].type;

            // 성격
            Personality personality = personalityMap.ContainsKey(desirePersonality) ? personalityMap[desirePersonality] : Personality.Generous; //성격이 지정되어 있지 않으며 기본값으로 Generous
                                                                                                                                                // 카테고
            CategoryData category = categoryMap.ContainsKey(desireCategory + '\r') ? categoryMap[desireCategory + '\r'] : null;

            if (desireCategory.Contains("||"))
            {
                Debug.Log(category);
            }

            //Debug.Log(category);

            // 재료 선택
            Ingredient.MeatFish meatfish = meatFishMap.ContainsKey(desireMeatfish) ? meatFishMap[desireMeatfish] : Ingredient.MeatFish.noCondition;
            Ingredient.Vege vege = vegeMap.ContainsKey(desireVege) ? vegeMap[desireVege] : Ingredient.Vege.noCondition;
            Ingredient.Base baseIngred = baseMap.ContainsKey(desireCategory) ? baseMap[desireCategory] : Ingredient.Base.noCondition;
            Ingredient.Cook cook = cookMap.ContainsKey(desireCategory) ? cookMap[desireCategory] : Ingredient.Cook.noCondition;
            Ingredient.Main Main = mainMap.ContainsKey(desireMeatfish) ? mainMap[desireMeatfish] : (mainMap.ContainsKey(desireVege) ? mainMap[desireVege] : Ingredient.Main.noCondition);

            // 불호 여부
            bool hateMeatFish = false;
            bool hateVege = false;
            bool hateBase = false;
            bool hateCategory = false;

            // 
            if (meatfish != Ingredient.MeatFish.noCondition || vege != Ingredient.Vege.noCondition)
            {
                if (baseIngred != Ingredient.Base.noCondition || cook != Ingredient.Cook.noCondition)
                {
                    Debug.Log("01");
                    CustomerManager.instance.GetOrder(personality, true, meatfish, vege, baseIngred, cook, hateMeatFish, hateVege, hateBase);
                }

                else if (category != null)
                {
                    Debug.Log("02");
                    CustomerManager.instance.GetOrder(personality, true, meatfish, vege, category, hateMeatFish, hateVege, hateBase);
                }

                else if (desireCategory.Contains("||"))
                {
                    string[] categories = desireCategory.Split("||");
                    List<CategoryData> desireCategories = new List<CategoryData>();
                    foreach (string categoryData in categories)
                    {
                        if (categoryMap.ContainsKey(categoryData + '\r'))
                        {
                            desireCategories.Add(categoryMap[categoryData + '\r']);
                        }
                    }

                    Debug.Log("03");
                    CustomerManager.instance.GetOrder(personality, true, meatfish, vege, desireCategories, hateMeatFish, hateVege, hateBase);
                }

                else
                {
                    Debug.Log("04");
                    CustomerManager.instance.GetOrder(personality, true, meatfish, vege, baseIngred, cook, hateMeatFish, hateVege, hateBase);
                }
            }

            else if (Main != Ingredient.Main.noCondition)
            {
                if (baseIngred != Ingredient.Base.noCondition || cook != Ingredient.Cook.noCondition)
                {
                    Debug.Log("05");
                    CustomerManager.instance.GetOrder(personality, true, Main, baseIngred, cook, hateCategory, hateBase);
                }
                else if (category != null)
                {
                    Debug.Log("06");
                    CustomerManager.instance.GetOrder(personality, true, Main, category, hateCategory, hateBase);
                }

                else if (desireCategory.Contains("||"))
                {
                    string[] categories = desireCategory.Split("||");
                    List<CategoryData> desireCategories = new List<CategoryData>();
                    foreach (string categoryData in categories)
                    {
                        if (categoryMap.ContainsKey(categoryData + '\r'))
                        {
                            desireCategories.Add(categoryMap[categoryData + '\r']);
                        }
                    }

                    Debug.Log("07");
                    CustomerManager.instance.GetOrder(personality, true, Main, desireCategories, hateCategory, hateBase);
                }

                else
                {
                    Debug.Log("08");
                    CustomerManager.instance.GetOrder(personality, true, Main, baseIngred, cook, hateCategory, hateBase);
                }
            }

            else if (category != null)
            {
                Debug.Log("07");
                CustomerManager.instance.GetOrder(personality, true, Main, category, hateCategory, hateBase);
            }


            // 주재료 없이 || 카테고리인 형식인 경우에는 03의 경우와 동일하게 처리
            else if (desireCategory.Contains("||"))
            {
                string[] categories = desireCategory.Split("||");
                List<CategoryData> desireCategories = new List<CategoryData>();
                foreach (string categoryData in categories)
                {
                    if (categoryMap.ContainsKey(categoryData + "\r"))
                    {
                        desireCategories.Add(categoryMap[categoryData + '\r']);
                    }
                }

                Debug.Log("08");
                CustomerManager.instance.GetOrder(personality, true, meatfish, vege, desireCategories, hateMeatFish, hateVege, hateBase);
            }

            // 지정된 주재료, 카테고리가 없는 경우에는 01의 경우와 동일하게 처리
            else
            {
                Debug.Log("09");
                CustomerManager.instance.GetOrder(personality, true, meatfish, vege, baseIngred, cook, hateMeatFish, hateVege, hateBase);
            }

            skipUI.gameObject.SetActive(false);
        }
    }

    private string GetStoryResult()
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

            //nameUI.text = dialogueDic[currentID].name;
            var text2 = dialogueDic[currentID].line.Replace('`', ',');
            dialogueSet(text2);

            return text2;
        }
        // 단일 반응 처리
        else
        {
            Debug.Log("싱글");
            return null;
        }
    }

    private string GetRandomOrder()
    {
        string currentDialogue = randomDialogues[indexForRandom].line;

        CustomerManager.instance.currentCustomer.InitializeOrder();
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

        switch (randomDialogues[indexForRandom].desireMain)
        {
            case "$$":
            case "!$$":


                int temp = UnityEngine.Random.Range(0, 1);

                if (temp == 0)
                {
                    //desireMeatfish = noBatchimMeatFish[UnityEngine.Random.Range(0, noBatchimMeatFish.Length)];
                    desireMeatfish = getUnlockedMeatFish(noBatchimMeatFish);
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

                    currentDialogue = currentDialogue.Replace("$$", desireMeatfish);

                    if ((desireMeatfish == "육류" || desireMeatfish == "생선류") && randomDialogues[indexForRandom].desireMain[0].Equals('!'))
                    {
                        hateCategory = true;
                    }
                    else if (randomDialogues[indexForRandom].desireMain[0].Equals('!'))
                    {
                        hateMeatFish = true;
                    }

                }
                else
                {
                    //desireVege = noBatchimVege[UnityEngine.Random.Range(0, noBatchimVege.Length)];
                    desireVege = getUnlockedVege(noBatchimVege);
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

                    currentDialogue = currentDialogue.Replace("$$", desireVege);
                    if ((desireVege == "과채류") && randomDialogues[indexForRandom].desireMain[0].Equals('!'))
                    {
                        hateCategory = true;
                    }
                    else if (randomDialogues[indexForRandom].desireMain[0].Equals('!'))
                    {
                        hateVege = true;
                    }
                }

                break;

            case "%%":
            case "!%%":
                //desireVege = withBatchimVege[UnityEngine.Random.Range(0, withBatchimVege.Length)];
                desireVege = getUnlockedVege(withBatchimVege); // 버섯이 해금되지 않았을 때는 무한루프
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

                currentDialogue = currentDialogue.Replace("%%", desireVege);
                if (randomDialogues[indexForRandom].desireMain[0].Equals('!'))
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
                    //desireCategory =  noBatchimBase[UnityEngine.Random.Range(0, noBatchimBase.Length)];
                    desireCategory = getUnlockedBase(noBatchimBase);
                    if (randomDialogues[indexForRandom].desireCategory[0].Equals('!'))
                    {
                        hateBase = true;
                    }
                }

                else if (temp == 1)
                {
                    //desireCategory = noBatchimCook[UnityEngine.Random.Range(0, noBatchimCook.Length)];
                    desireCategory = getUnlockedCook(noBatchimCook);
                }

                else
                {
                    //desireCategory = noBatchimCategory[UnityEngine.Random.Range(0, noBatchimCategory.Length)];
                    desireCategory = getUnlockedCategory(noBatchimCategory);

                    /*
                    if (randomDialogues[indexForRandom].desireCategory[0].Equals('!'))
                    {
                        hateCategory = true;
                    }
                    */

                    desireCategory = desireCategory.TrimEnd();
                }

                currentDialogue = currentDialogue.Replace("##", desireCategory);
                break;

            case "@@":
            case "!@@":
                //desireCategory = withBatchimCategory[UnityEngine.Random.Range(0, withBatchimCategory.Length)];
                desireCategory = getUnlockedCategory(withBatchimCategory);
                desireCategory = desireCategory.TrimEnd();
                currentDialogue = currentDialogue.Replace("@@", desireCategory);
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
                    //desireMeatfish = noBatchimMeatFish[UnityEngine.Random.Range(0, noBatchimMeatFish.Length)];
                    desireMeatfish = getUnlockedMeatFish(noBatchimMeatFish);
                    currentDialogue = currentDialogue.Replace("**", desireMeatfish);
                    break;

                case 1:
                    //desireVege = noBatchimVege[UnityEngine.Random.Range(0, noBatchimVege.Length)];
                    desireVege = getUnlockedVege(noBatchimVege);
                    currentDialogue = currentDialogue.Replace("**", desireVege);
                    break;

                case 2:
                    //desireVege = withBatchimVege[UnityEngine.Random.Range(0, withBatchimVege.Length)];

                    // 임시
                    if (GameManager.instance.day < 4)
                        desireVege = getUnlockedVege(noBatchimVege);
                    else
                        desireVege = getUnlockedVege(withBatchimVege);

                    currentDialogue = currentDialogue.Replace("**", desireVege);
                    break;

                case 3:
                    //desireCategory = noBatchimBase[UnityEngine.Random.Range(0, noBatchimBase.Length)];
                    desireCategory = getUnlockedBase(noBatchimBase);
                    currentDialogue = currentDialogue.Replace("**", desireCategory);
                    break;

                case 4:
                    //desireCategory = noBatchimCook[UnityEngine.Random.Range(0, noBatchimCook.Length)];
                    desireCategory = getUnlockedCook(noBatchimCook);
                    currentDialogue = currentDialogue.Replace("**", desireCategory);
                    break;

                case 5:
                    //desireCategory = noBatchimCategory[UnityEngine.Random.Range(0, noBatchimCategory.Length)];
                    desireCategory = getUnlockedCategory(noBatchimCategory);
                    desireCategory = desireCategory.TrimEnd();
                    currentDialogue = currentDialogue.Replace("**", desireCategory);
                    break;

                case 6:
                    //desireCategory = withBatchimCategory[UnityEngine.Random.Range(0, withBatchimCategory.Length)];
                    desireCategory = getUnlockedCategory(withBatchimCategory);
                    desireCategory = desireCategory.TrimEnd();
                    currentDialogue = currentDialogue.Replace("**", desireCategory);
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

            else
            {
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

            else
            {
                Debug.Log(6);
                CustomerManager.instance.GetOrder(personality, false, Main, baseIngred, cook, hateCategory, hateBase);
            }
        }

        else if (category != null)
        {
            Debug.Log(7);
            CustomerManager.instance.GetOrder(personality, false, Main, category, hateCategory, hateBase);
        }

        else
        {
            //Debug.Log("오류");
            CustomerManager.instance.GetOrder(personality, false, meatfish, vege, baseIngred, cook, hateMeatFish, hateVege, hateBase);
        }

        nameUI.text = "손님";
        var text4 = currentDialogue.Replace('`', ',');
        return text4;
    }


    public void GetNextDialogue()
    {
        /*
        if (isFinish == true) {

            if (endingDialogueDic[currentID].directing == "fade_in") {
                GameManager.instance.Fade_Panel.gameObject.SetActive(true);
                FadeEvent(0);
            }

            nameUI.text = endingDialogueDic[currentID].name;
            var text = endingDialogueDic[currentID].line.Replace('`', ',');
            dialogueSet(text);
            _pastID = _currentID;
            _currentID = endingDialogueDic[currentID].nextDialogueID;
            return;
        }
        */
        if (GameManager.instance.Fade_Panel.GetComponent<FadeController>().fadingDone == false) // 페이드 연출이 완료된 후(페이드 아웃 후)에 또 ID를 가져오면 안됨
        {
            _pastID = _currentID;
            string[] tmp = _currentID.Split(new char[] { '_' });

            if (!isFinish)
            {
                // 스토리 대사 ID가 필요한 경우
                if (_isRandom == false)
                {
                    GetStoryDialogueID();

                    if (_currentID == "D05_C01_R_positive")
                    {
                        GameManager.instance.Order_Canvas.Find("Background").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/New/" + "Main_restaurant_Day5");
                    }

                }
                // 랜덤 대사 ID가 필요한 경우
                if (_isRandom == true || (currentID.Contains("GTR") && _isRandom == false))
                {
                    GetRandomDialogueID();
                }
            }

            // 엔딩 대사 ID가 필요한 경우
            else
            {
                _pastID = _currentID;
                _currentID = endingDialogueDic[currentID].nextDialogueID;

                if (endingDialogueDic[currentID].directing == "fade")
                {
                    // 엔딩 크레딧 필요할 때
                    if (endingDialogueDic[currentID].line == "The End")
                    {
                        GameManager.instance.Fade_Panel.gameObject.SetActive(true);
                        DayPassEvent(2);
                    }

                    else
                    {
                        GameManager.instance.Fade_Panel.gameObject.SetActive(true);
                        FadeEvent(0);
                    }
                }

            }

        }

        // 스토리 CSV 끝났을 때, 엔딩 CSV 가져오기
        if (_currentID == "" && _isFinish == false)
        {
            _isFinish = true;
            int score = GameManager.instance.reputation;
            if (score >= GameManager.instance.GoodEndingCriteria)
            {
                endingDialogues = theParser.EndingParse(HappyEndingCSV);
                endingTrack = "good_ending";
                for (int i = 0; i < endingDialogues.Length; i++)
                {
                    endingDialogueDic.Add(endingDialogues[i].dialogueID, endingDialogues[i]);
                }
            }
            else if (score >= GameManager.instance.NormalCriteria)
            {
                endingDialogues = theParser.EndingParse(NormalEndingCSV);
                endingTrack = "normal_ending";
                for (int i = 0; i < endingDialogues.Length; i++)
                {
                    endingDialogueDic.Add(endingDialogues[i].dialogueID, endingDialogues[i]);
                }
            }
            else
            {
                endingDialogues = theParser.EndingParse(BadEndingCSV);
                endingTrack = "bad_ending";
                for (int i = 0; i < endingDialogues.Length; i++)
                {
                    endingDialogueDic.Add(endingDialogues[i].dialogueID, endingDialogues[i]);
                }
            }
            letterUI.gameObject.SetActive(true);
            skipUI.gameObject.SetActive(false);
            _currentID = endingDialogues[0].dialogueID;
            return;
        }

        // 페이드 연출 중에는 대사가 더 이상 진행되지 않도록 강제 리턴
        if (GameManager.instance.Fade_Panel.GetComponent<FadeController>().fadingInProgress == true)
        {
            return;
        }

        // 페이드가 끝났으면 fadingDone = false
        if (GameManager.instance.Fade_Panel.GetComponent<FadeController>().fadingDone == true)
        {
            GameManager.instance.Fade_Panel.GetComponent<FadeController>().fadeDoneStatusChange();

        }

        // 엔딩 대사 출력
        if (isFinish == true)
        {

            if (_currentID != "")
            {
                skipUI.gameObject.SetActive(true);

                if (endingTrack != "")
                {
                    AudioManager.Instance.BGMTrackChange(endingTrack);
                    endingTrack = "";
                }

            }

            nameUI.text = endingDialogueDic[currentID].name;
            var text = endingDialogueDic[currentID].line.Replace('`', ',');
            dialogueSet(text);


            //Debug.Log(endingDialogueDic[currentID].spriteID);

            if (nameUI.text.CompareTo("나") == 0 && portraitUI.gameObject.activeSelf == true)
            {
                togglePortraitUI();
            }
            else if (nameUI.text.CompareTo("") != 0 && nameUI.text.CompareTo("나") != 0 && portraitUI.gameObject.activeSelf == false)
            {
                togglePortraitUI();
            }


            StartCoroutine(SpriteManager.Instance.SpriteChangeCoroutine(endingDialogueDic[currentID].spriteID));
            GameManager.instance.Order_Canvas.Find("Background").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/New/Ending/" + endingDialogueDic[currentID].background.Trim());

            return;
        }

        // 스토리 주문 * 결과 처리
        if ((_isRandom == false || directOrder == true) && !(_currentID.Contains("GTR")))
        {

            // 스토리 주문 처리
            GetStoryOrder();

            // 다중 선택 ui 활성화
            if (currentID.Contains("&&") && !currentID.Contains("R"))
            {
                string[] chooseTmp = _currentID.Split("&&");

                multipleskipUI.gameObject.SetActive(true);
                skipUI.gameObject.SetActive(false);

                var firstChoose = default(string);
                if (dialogueDic[chooseTmp[0]].line[0] == '@')
                {
                    firstChoose = dialogueDic[chooseTmp[0]].line.Substring(1, dialogueDic[chooseTmp[0]].line.Length - 1);
                }
                else
                    firstChoose = dialogueDic[chooseTmp[0]].line;

                var secondChoose = default(string);
                if (dialogueDic[chooseTmp[1]].line[0] == '%')
                {
                    secondChoose = dialogueDic[chooseTmp[1]].line.Substring(1, dialogueDic[chooseTmp[1]].line.Length - 1);
                }
                else
                    secondChoose = dialogueDic[chooseTmp[1]].line;

                MultipleTextSetEvent(firstChoose, secondChoose);

                return;
            }

            // 스토리 결과 처리
            if (currentID.Contains("_R_"))
            {
                string result = GetStoryResult();
                if (result != null)
                {
                    nameUI.text = dialogueDic[currentID].name;
                    dialogueSet(result);
                    return;
                }
            }
        }

        // 스토리 주문과 관련 없는 일반 대사 처리
        if (_isRandom == false)
        {
            // 뒷광고 여부 플래그 처리가 필요할 때면 플래그를 처리
            if (_currentID.Contains("@@"))
            {
                string[] chooseTmp = _currentID.Split("@@");

                if (GameManager.instance.sneakyAdsFlag == true)
                {
                    _currentID = chooseTmp[0];
                }
                if (GameManager.instance.sneakyAdsFlag == false)
                {
                    _currentID = chooseTmp[1];
                }

            }


            Debug.Log(dialogueDic[currentID].spriteID);

            if (dialogueDic[currentID].name.CompareTo("나") == 0 && portraitUI.gameObject.activeSelf == true)
            {
                togglePortraitUI();
            }
            else if (dialogueDic[currentID].name.CompareTo("나") != 0 && portraitUI.gameObject.activeSelf == false)
            {
                togglePortraitUI();
            }

            StartCoroutine(SpriteManager.Instance.SpriteChangeCoroutine(dialogueDic[currentID].spriteID));


            nameUI.text = dialogueDic[currentID].name;
            var text3 = dialogueDic[currentID].line.Replace('`', ',');
            dialogueSet(text3);

        }
        // 랜덤 주문 대사 처리
        else
        {

            dialogueSet(GetRandomOrder());

            nextUI.gameObject.SetActive(true);
            skipUI.gameObject.SetActive(false);
            SpriteManager.Instance.GetRandomSprite();

            indexForRandom++;
            _Customer++;
            if (_pastID.Split(new char[] { '_' })[1] != "C00")
                GameManager.instance.nextCustomer();


        }
    }

    public void GetNextDialogueMultiple(string index)
    {

        string[] chooseTmp = _currentID.Split("&&");

        if (index == "a")
            _currentID = chooseTmp[0];
        if (index == "b")
            _currentID = chooseTmp[1];

        // 플래그를 선택하는 선택지에 따라 플래그를 설정
        if (dialogueDic[_currentID].line.Contains('@'))
            GameManager.instance.sneakyAdsFlag = true;

        if (dialogueDic[_currentID].line.Contains('%'))
            GameManager.instance.sneakyAdsFlag = false;


        multipleskipUI.gameObject.SetActive(false);
        skipUI.gameObject.SetActive(true);

    }

    private string getUnlockedBase(string[] strings)
    {

        while (true)
        {
            string unlocked;

            while (true)
            {
                unlocked = strings[UnityEngine.Random.Range(0, strings.Length)];
                Debug.Log(unlocked);
                if (UnlockManager.instance.IsUnlocked(baseMap[unlocked]))
                {

                    break;
                }
            }
            return unlocked;
        }
    }
    private string getUnlockedCook(string[] strings)
    {
        string unlocked;

        while (true)
        {
            unlocked = strings[UnityEngine.Random.Range(0, strings.Length)];
            Debug.Log(unlocked);
            if (UnlockManager.instance.IsUnlocked(cookMap[unlocked]))
            {
                break;
            }
        }
        return unlocked;
    }

    private string getUnlockedMeatFish(string[] strings)
    {
        string unlocked;

        while (true)
        {
            unlocked = strings[UnityEngine.Random.Range(0, strings.Length)];
            Debug.Log(unlocked);
            if (unlocked == "육류" || unlocked == "생선류" || UnlockManager.instance.IsUnlocked(meatFishMap[unlocked]))
            {
                break;
            }
        }
        return unlocked;
    }

    private string getUnlockedVege(string[] strings)
    {
        string unlocked;

        while (true)
        {
            unlocked = strings[UnityEngine.Random.Range(0, strings.Length)];
            Debug.Log(unlocked);
            if (unlocked == "과채류" || UnlockManager.instance.IsUnlocked(vegeMap[unlocked]))
            {
                break;
            }
        }
        return unlocked;
    }

    private string getUnlockedCategory(string[] strings)
    {
        string unlocked;

        while (true)
        {
            unlocked = strings[UnityEngine.Random.Range(0, strings.Length)] + "\r";
            Debug.Log(unlocked);
            if (UnlockManager.instance.IsUnlocked(categoryMap[unlocked].baseIngred) && UnlockManager.instance.IsUnlocked(categoryMap[unlocked].cook))
            {
                break;
            }
        }
        return unlocked;
    }

    public string getRandomReaction(int index)
    {

        int randomIndex = UnityEngine.Random.Range(0, randomReactionDialogueDic[index].Length);

        string line = randomReactionDialogueDic[index][randomIndex].line;


        if (line.Contains("$$") || line.Contains("%%"))
        {


            if (CustomerManager.instance.currentCustomer.meatfish != MeatFish.noCondition || CustomerManager.instance.currentCustomer.vege != Vege.noCondition || CustomerManager.instance.currentCustomer.mainIngredCategory != Main.noCondition)
            {
                if (line.Contains("$$"))
                {
                    string[] tem = noBatchimMeatFish.Concat(noBatchimVege).ToArray();
                    for (int i = 0; i < tem.Length; i++)
                    {
                        if (dialogueUI.text.Contains(tem[i]))
                        {
                            line = line.Replace("$$", tem[i]);
                            break;
                        }
                    }
                }

                else if (line.Contains("%%"))
                {
                    string[] tem = withBatchimMeatFish.Concat(withBatchimVege).ToArray();
                    for (int i = 0; i < tem.Length; i++)
                    {
                        if (dialogueUI.text.Contains(tem[i]))
                        {
                            line = line.Replace("%%", tem[i]);
                            break;
                        }
                    }
                }

                // 치환한 단어가 tem에 없으면 다시 출력할 반응 대사 선택
                while (line.Contains("$$") || line.Contains("%%"))
                {
                    randomIndex = UnityEngine.Random.Range(0, randomReactionDialogueDic[index].Length);
                    line = randomReactionDialogueDic[index][randomIndex].line;

                }
            }
            //CustomerManager.instance.orderText.text

            else
            {
                while (line.Contains("$$") || line.Contains("%%"))
                {
                    randomIndex = UnityEngine.Random.Range(0, randomReactionDialogueDic[index].Length);
                    line = randomReactionDialogueDic[index][randomIndex].line;

                }
            }
        }

        if (line.Contains('`'))
        {
            line = line.Replace('`', ',');
        }

        return line;

    }

    public void dialogueReset()
    {
        nameUI.text = "";
        dialogueSet("");
    }

    private void dialogueSet(string dlg)
    {
        DialogueSetEvent!.Invoke(dlg);
    }

    public void togglePortraitUI()
    {
        portraitUI.gameObject.SetActive(!portraitUI.gameObject.activeSelf);
    }

    public void endingBackground() {
        GameManager.instance.Order_Canvas.Find("Background").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/New/Ending/" + endingDialogueDic[currentID].background.Trim());
    }
}
