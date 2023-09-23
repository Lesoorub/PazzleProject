using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static FightAI;

public class FightController : TableController
{
    [Header("Settings")]
    public Person me;

    public PersonInfoVisualizer MyInfo;
    public PersonInfoVisualizer EnemyInfo;

    public Color32 MeSelectionColor;
    public Color32 EnemySelectionColor;

    FightPerson me_f;
    FightPerson enemy_f;


    bool curStepIsMe = true;
    public FightPerson Me => curStepIsMe ? me_f : enemy_f;
    public PersonInfoVisualizer Me_InfoVisualizer => curStepIsMe ? MyInfo : EnemyInfo;
    public FightPerson Enemy => curStepIsMe ? enemy_f : me_f;
    public PersonInfoVisualizer Enemy_InfoVisualizer => curStepIsMe ? EnemyInfo : MyInfo;

    [Header("AI")]
    public TableElement Skull;
    public TableElement[] Mana;

    [Header("Events")]
    public UnityEvent<FightPerson> OnFinish = new UnityEvent<FightPerson>();
    public UnityEvent OnMyStep = new UnityEvent();

    FightAI ai;

    public void CreateFight(Person enemy)
    {
        InitTable();
        MyInfo.Set(me_f = new FightPerson(me), this);
        EnemyInfo.Set(enemy_f = new FightPerson(enemy), this);
        MyInfo.SetCurrentStep(curStepIsMe);
        EnemyInfo.SetCurrentStep(!curStepIsMe);

        AIDifficulty difficulty;

        if (enemy is AIPerson ai_person)
            difficulty = ai_person.Difficulty;
        else
            difficulty = AIDifficulty.Medium;

        int w = TableSizeWidth, h = TableSizeHeight;

        TableElement[,] tablecache = new TableElement[w, h];
        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
                tablecache[x, y] = Get(x, y);

        ai = new FightAI(tablecache, Skull, Mana, difficulty);

        OnSelectComplete?.RemoveListener(OnStepEnded);
        OnSelectComplete?.AddListener(OnStepEnded);

        me_f.OnDeath?.AddListener(() => InvokeOnFinish(enemy_f));
        enemy_f.OnDeath?.AddListener(() => InvokeOnFinish(me_f));
    }

    bool preventNextSelect = false;
    List<UnityAction<Vector2Int>> actions = new List<UnityAction<Vector2Int>>();
    private void Awake()
    {
        OnStartSelect?.AddListener(Preventor);
    }


    void Preventor(Vector2Int pos)
    {
        if (preventNextSelect)
        {
            TableEndSelection();
            preventNextSelect = false;
            foreach (var a in actions)
                a?.Invoke(pos);
            actions.Clear();
        }
    }

    public void AddSpellCastEvent(UnityAction<Vector2Int> @do)
    {
        preventNextSelect = true;
        actions.Add((x) =>
        {
            @do(x);
            TableEndSelection();
        });
    }

    void OnStepEnded(int xp)
    {
        if (curStepIsMe)
            MyStepEnded();
        else
            EnemyStepEnded();

        MyInfo.SetCurrentStep(curStepIsMe);
        EnemyInfo.SetCurrentStep(!curStepIsMe);

        if (curStepIsMe)
            OnMyStep?.Invoke();
    }

    void MyStepEnded()
    {
        curStepIsMe = false;
        if (!enemy_f.isDeadth)
        {
            EnemyInfo.SpellsCooldownTick();
            SelectionColor = EnemySelectionColor;
            LineRenderer.color = EnemySelectionColor;
            StartCoroutine(EnemyAIStep());
        }
    }
    void EnemyStepEnded()
    {
        curStepIsMe = true;
        MyInfo.SpellsCooldownTick();
        SelectionColor = MeSelectionColor;
        LineRenderer.color = MeSelectionColor;
    }

    IEnumerator EnemyAIStep()
    {
        curStepIsMe = false;
        yield return StartCoroutine(AIStep(true));
    }

    void InvokeOnFinish(FightPerson finished)
    {
        OnFinish?.Invoke(finished);
    }

    bool can_UseAIStep = true;
    public void _UseAIStep()
    {
        if (curStepIsMe && can_UseAIStep)
        {
            can_UseAIStep = false;
            StartCoroutine(MeAIStep());
        }
    }
    IEnumerator MeAIStep()
    {
        var t = ai.difficulty;
        ai.difficulty = AIDifficulty.Hard;
        curStepIsMe = true;
        yield return StartCoroutine(AIStep());
        ai.difficulty = t;
        can_UseAIStep = true;
    }
    public IEnumerator AIStep(bool castSpells = false)
    {
        const float StartDelay = .5f;
        const float DelayBetweedSteps = .1f;
        const float DelayAfterSteps = .5f;
        const float EndDelay = .5f;
        const float NotEnoghtStepsDelay = 2f;

        LockInput = true;
        yield return new WaitForSecondsRealtime(StartDelay);
        ai.UpdateTable(this);

        bool step = true;
        if (castSpells)
        {
            const int maxIterations = 32;
            for (int iterations = 0; iterations < maxIterations; iterations++)
            {
                SpellVisualizer spell = ai.GetNeededSpellToCast(Me, Me_InfoVisualizer.Spells);
                if (spell == null) break;
                spell._OnClick();
                if (spell.spell.NeedTarget)
                {
                    var steps = ai.GetAIStep();
                    while (steps.Count() == 0)
                    {
                        InitTable();
                        yield return new WaitForSecondsRealtime(NotEnoghtStepsDelay);
                        steps = ai.GetAIStep();
                    }
                    var p = steps.First();
                    LockInput = false;
                    AddToSelected(table[p.x, p.y].visualizer);
                    TableEndSelection();
                    LockInput = true;
                }
                if (spell.spell.CostStep)
                {
                    step = false;
                    break;
                }
                yield return new WaitForSecondsRealtime(0.2f);
            }
        }

        if (step)
        {
            var steps = ai.GetAIStep();
            while (steps.Count() == 0)
            {
                InitTable();
                yield return new WaitForSecondsRealtime(NotEnoghtStepsDelay);
                steps = ai.GetAIStep();
            }
            foreach (var p in steps)
            {
                LockInput = false;
                AddToSelected(table[p.x, p.y].visualizer);
                LockInput = true;
                yield return new WaitForSecondsRealtime(DelayBetweedSteps);
            }
            LockInput = true;
            yield return new WaitForSecondsRealtime(DelayAfterSteps);
            LockInput = false;
            TableEndSelection();
        }
        else
        {
            LockInput = false;
            EndStep();
        }
        LockInput = true;
        yield return new WaitForSecondsRealtime(EndDelay);
        LockInput = false;

        yield break;
    }
}