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

    bool preventNextSelect = false;
    List<UnityAction<Vector2Int>> actions = new List<UnityAction<Vector2Int>>();

    FightPerson me_f;
    FightPerson enemy_f;

    bool curStepIsMe = true;
    public FightPerson Me => this.curStepIsMe ? this.me_f : this.enemy_f;
    public PersonInfoVisualizer Me_InfoVisualizer => this.curStepIsMe ? this.MyInfo : this.EnemyInfo;
    public FightPerson Enemy => this.curStepIsMe ? this.enemy_f : this.me_f;
    public PersonInfoVisualizer Enemy_InfoVisualizer => this.curStepIsMe ? this.EnemyInfo : this.MyInfo;

    [Header("AI")]
    public TableElement Skull;
    public TableElement[] Mana;

    [Header("Events")]
    public UnityEvent<FightPerson> OnFinish = new UnityEvent<FightPerson>();
    public UnityEvent OnMyStep = new UnityEvent();

    FightAI ai;

    public void CreateFight(Person enemy)
    {
        this.InitTable();
        this.MyInfo.Set(this.me_f = new FightPerson(this.me), this);
        this.EnemyInfo.Set(this.enemy_f = new FightPerson(enemy), this);
        this.MyInfo.SetCurrentStep(this.curStepIsMe);
        this.EnemyInfo.SetCurrentStep(!this.curStepIsMe);

        AIDifficulty difficulty;

        if (enemy is AIPerson ai_person)
            difficulty = ai_person.Difficulty;
        else
            difficulty = AIDifficulty.Medium;

        int w = TABLE_SIZE_WIDTH, h = TABLE_SIZE_HEIGHT;

        TableElement[,] tablecache = new TableElement[w, h];
        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
                tablecache[x, y] = this.Get(x, y);

        this.ai = new FightAI(tablecache, this.Skull, this.Mana, difficulty);

        this.OnSelectComplete?.RemoveListener(this.OnStepEnded);
        this.OnSelectComplete?.AddListener(this.OnStepEnded);

        this.me_f.OnDeath?.AddListener(() => this.InvokeOnFinish(this.enemy_f));
        this.enemy_f.OnDeath?.AddListener(() => this.InvokeOnFinish(this.me_f));
    }

    private void Awake()
    {
        this.OnStartSelect?.AddListener(this.Preventor);
    }


    void Preventor(Vector2Int pos)
    {
        if (this.preventNextSelect)
        {
            this.TableEndSelection();
            this.preventNextSelect = false;
            foreach (var a in this.actions)
                a?.Invoke(pos);
            this.actions.Clear();
        }
    }

    public void AddSpellCastEvent(UnityAction<Vector2Int> @do)
    {
        this.preventNextSelect = true;
        this.actions.Add((x) =>
        {
            @do(x);
            this.TableEndSelection();
        });
    }

    void OnStepEnded(int count)
    {
        int xp = (int)(count * count * 0.25f);

        if (this.curStepIsMe)
            this.MyStepEnded();
        else
            this.EnemyStepEnded();

        this.MyInfo.SetCurrentStep(this.curStepIsMe);
        this.EnemyInfo.SetCurrentStep(!this.curStepIsMe);

        if (this.curStepIsMe)
            this.OnMyStep?.Invoke();
    }

    void MyStepEnded()
    {
        this.curStepIsMe = false;
        if (!this.enemy_f.isDeadth)
        {
            this.EnemyInfo.SpellsCooldownTick();
            this.SelectionColor = this.EnemySelectionColor;
            this.LineRenderer.color = this.EnemySelectionColor;
            this.StartCoroutine(this.EnemyAIStep());
        }
    }
    void EnemyStepEnded()
    {
        this.curStepIsMe = true;
        this.MyInfo.SpellsCooldownTick();
        this.SelectionColor = this.MeSelectionColor;
        this.LineRenderer.color = this.MeSelectionColor;
    }

    IEnumerator EnemyAIStep()
    {
        this.curStepIsMe = false;
        yield return this.StartCoroutine(this.AIStep(true));
    }

    void InvokeOnFinish(FightPerson finished)
    {
        this.OnFinish?.Invoke(finished);
    }

    bool can_UseAIStep = true;
    public void _UseAIStep()
    {
        if (this.curStepIsMe && this.can_UseAIStep)
        {
            this.can_UseAIStep = false;
            this.StartCoroutine(this.MeAIStep());
        }
    }
    IEnumerator MeAIStep()
    {
        var t = this.ai.difficulty;
        this.ai.difficulty = AIDifficulty.Hard;
        this.curStepIsMe = true;
        yield return this.StartCoroutine(this.AIStep());
        this.ai.difficulty = t;
        this.can_UseAIStep = true;
    }
    public IEnumerator AIStep(bool castSpells = false)
    {
        const float StartDelay = .5f;
        const float DelayBetweedSteps = .1f;
        const float DelayAfterSteps = .5f;
        const float EndDelay = .5f;
        const float NotEnoghtStepsDelay = 2f;

        this.LockInput = true;
        yield return new WaitForSecondsRealtime(StartDelay);
        this.ai.UpdateTable(this);

        bool step = true;
        if (castSpells)
        {
            const int maxIterations = 32;
            for (int iterations = 0; iterations < maxIterations; iterations++)
            {
                SpellVisualizer spell = this.ai.GetNeededSpellToCast(this.Me, this.Me_InfoVisualizer.Spells);
                if (spell == null) break;
                spell._OnClick();
                if (spell.spell.NeedTarget)
                {
                    var steps = this.ai.GetAIStep();
                    while (steps.Count() == 0)
                    {
                        this.InitTable();
                        yield return new WaitForSecondsRealtime(NotEnoghtStepsDelay);
                        steps = this.ai.GetAIStep();
                    }
                    var p = steps.First();
                    this.LockInput = false;
                    this.AddToSelected(this.table[p.x, p.y].visualizer);
                    this.TableEndSelection();
                    this.LockInput = true;
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
            var steps = this.ai.GetAIStep();
            while (steps.Count() == 0)
            {
                this.InitTable();
                yield return new WaitForSecondsRealtime(NotEnoghtStepsDelay);
                steps = this.ai.GetAIStep();
            }
            foreach (var p in steps)
            {
                this.LockInput = false;
                this.AddToSelected(this.table[p.x, p.y].visualizer);
                this.LockInput = true;
                yield return new WaitForSecondsRealtime(DelayBetweedSteps);
            }
            this.LockInput = true;
            yield return new WaitForSecondsRealtime(DelayAfterSteps);
            this.LockInput = false;
            this.TableEndSelection();
        }
        else
        {
            this.LockInput = false;
            this.EndStep();
        }
        this.LockInput = true;
        yield return new WaitForSecondsRealtime(EndDelay);
        this.LockInput = false;

        yield break;
    }
}