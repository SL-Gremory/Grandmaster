using System.Collections;
using UnityEngine;

public class UnitStateController : MonoBehaviour
{


    #region Declarations

    private TurnManager turnManager;
    private BattleNavigate battleNavigate;

    // Booleans
    private bool moveIsDone;
    private bool actionIsDone;
    private bool isAttacking;

    // Rocky Hp bar code stuff
    private HPScript hpBar;

    // References
    public UnitData unitData;
    public Selectable unitSelectable;
    public Parameters unitParameters;


    #endregion



    // Start is called before the first frame update
    void Start()
    {
        unitData = gameObject.GetComponent<UnitData>();
        unitSelectable = gameObject.GetComponent<Selectable>();
        unitParameters = gameObject.GetComponent<Parameters>();
        hpBar = gameObject.GetComponent<HPScript>(); // Rocky HP bar stuff
        turnManager = TurnManager.Instance;
        battleNavigate = gameObject.GetComponentInParent<BattleNavigate>();
        StartUnit();
        hpBar.Start();
    }

    void Update()
    {



        // HP bar UI functions
        if (hpBar._localScale.x > (float)unitParameters.CHP / (float)unitParameters.MHP)
        {
            hpBar.ChangeLocalScale((float)unitParameters.CHP / (float)unitParameters.MHP);
        }

        // Unit is in attacking phase
        if (isAttacking)
        {
            GameObject foundUnitObject = null;

            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000f))
                {
                    foundUnitObject = hit.transform.gameObject;


                    // If attack is impossible, deselect the found unit
                    // and put selector back to this unit
                    if (foundUnitObject != null)
                    {
                        PrepareAttackOn(foundUnitObject);
                        foundUnitObject.GetComponent<UnitStateController>().DeselectUnit();
                        unitSelectable.SelectThis(true);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("Deselected " + unitData.UnitName + " via attack cancel");

                if (foundUnitObject != null)
                {
                    foundUnitObject.GetComponent<UnitStateController>().DeselectUnit();
                }

                isAttacking = false;
            }

        }
        else if (unitSelectable.GetSelectStatus())
        {







            //unitInfo.DisplayStats(this);









            if (!moveIsDone)
            {
                if (turnManager.isPlayerTurn && unitData.UnitTeam == Team.HERO || !turnManager.isPlayerTurn && unitData.UnitTeam == Team.ENEMY)
                {
                    battleNavigate.Move();
                }
            }

            //ESCAPE to deselect unit
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                unitSelectable.SelectThis(false);
                Debug.Log("Deselected a selectable via esc");
            }

            //SPACE to simulate unit performing an action
            if (Input.GetKeyDown(KeyCode.Space) && !actionIsDone)
            {
                DoneActing();
            }

            //Simulate a unit getting damaged
            if (Input.GetKeyDown(KeyCode.F))
            {
                unitParameters.CHP -= 10;
                Debug.Log(unitParameters.CHP);
                if (unitParameters.CHP <= 0)
                {
                    KillUnit();
                }
            }

            //Hit Z to simulated a selected unit to be in attacking phase
            if (Input.GetKeyDown(KeyCode.Z) && !actionIsDone)
            {
                Debug.Log(unitData.UnitName + " is atacc");
                isAttacking = true;
            }

            if (Input.GetKeyDown(KeyCode.B) && moveIsDone && !actionIsDone)
            {
                battleNavigate.Return();
                moveIsDone = false;
            }

        }
    }


    void StartUnit()
    {
        if (unitData.UnitTeam == Team.HERO)
        {
            TurnManager.Instance.playerUnitCount++;
            if (TurnManager.Instance.isPlayerTurn)
            {
                ReadyUnit();
            }
            else
            {
                ExhaustUnit();
            }
        }
        else if (unitData.UnitTeam == Team.ENEMY)
        {
            TurnManager.Instance.enemyUnitCount++;
            if (!TurnManager.Instance.isPlayerTurn)
            {
                ReadyUnit();
            }
            else
            {
                ExhaustUnit();
            }
        }
    }


    private void PrepareAttackOn(GameObject defender)
    {
        GameObject attacker = gameObject;
        UnitData defenderData = defender.GetComponent<UnitData>();
        Parameters defenderParameters = defender.GetComponent<Parameters>();



        Int2 aPos = new Int2(
                    (int)Mathf.Floor(this.transform.position.x),
                    (int)Mathf.Floor(this.transform.position.z));

        Int2 dPos = new Int2(
                    (int)Mathf.Floor(defender.transform.position.x),
                    (int)Mathf.Floor(defender.transform.position.z));

        if (unitData.UnitTeam == defender.GetComponent<UnitData>().UnitTeam)
        {
            Debug.Log("Cannot attack an ally!");
            return;
        }

        if (Int2.Distance(aPos, dPos) > 1)
        {
            Debug.Log("Cannot attack a unit that is out of range!");
            return;
        }

        Debug.Log("Unit is attacking this unit for " + Attack2.CalculateProjectedDamage(attacker, defender) + " damage but will take " +
            Attack2.CalculateProjectedDamage(defender, attacker) + " damage.");

        Attack2.CommenceBattle(gameObject, defender);
        Debug.Log(unitData.UnitName + " is now at " + unitParameters.CHP + " HP and " + defenderData.UnitName + " now has " + defenderParameters.CHP);

        if (defenderParameters.CHP == 0)
        {
            // Ded
            UnitStateController defenderController = defender.GetComponent<UnitStateController>();
            defenderController.KillUnit();
        }
        else if (gameObject.GetComponent<Parameters>().CHP == 0)
        {
            // This ded
            this.KillUnit();
        }
    }



    internal void KillUnit()
    {
        FinishUnit();
        battleNavigate.ResetNavigator();
        if (unitData.UnitTeam == Team.HERO)
        {
            TurnManager.Instance.playerUnitCount--;
        }
        else if (unitData.UnitTeam == Team.ENEMY)
        {
            TurnManager.Instance.enemyUnitCount--;
        }
        TurnManager.Instance.CheckWinConditions();
        Debug.Log(string.Format("{0} has died to death", unitData.UnitName));
        StartCoroutine("DeathAnimation");
    }


    // Unit cannot move or attack but selector is still active (used for death animation)
    internal void FinishUnit()
    {
        moveIsDone = true;
        actionIsDone = true;
        unitSelectable.ChangeColor(2);
    }


    // Can move and attack
    internal void ReadyUnit()
    {
        moveIsDone = false;
        actionIsDone = false;
        unitSelectable.ChangeColor(0);
    }

    // Cannot move or attack
    internal void ExhaustUnit()
    {
        unitSelectable.SelectThis(false);
        moveIsDone = true;
        actionIsDone = true;
        unitSelectable.ChangeColor(2);
    }


    // Called when cancelling a unit's selection via attack toggle, etc
    internal void DeselectUnit()
    {
        unitSelectable.SetSelectStatus(false);
        unitSelectable.SelectThis(false);
        isAttacking = false;
    }


    // Cannot move and attack
    internal void DoneActing()
    {
        unitSelectable.SelectThis(false);
        battleNavigate.ResetNavigator();
        if (!actionIsDone)
        {
            isAttacking = false;
            ExhaustUnit();
            TurnManager.Instance.unitsDone++;
            //Debug.Log("Unit finished acting");
        }
    }


    IEnumerator DeathAnimation()
    {

        SpriteRenderer render = gameObject.GetComponent<UnitData>().UnitRender;

        yield return new WaitForSeconds(0.5f);
        for (float f = 1f; f >= -0.05; f -= 0.10f)
        {
            Color c = render.material.color;
            c.a = f;
            render.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        unitSelectable.SelectThis(false);
        Destroy(gameObject);
    }

}
