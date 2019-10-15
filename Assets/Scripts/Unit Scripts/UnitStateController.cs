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
    int weaponRange;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        unitData = gameObject.GetComponent<UnitData>();
        unitSelectable = gameObject.GetComponent<Selectable>();
        unitParameters = gameObject.GetComponent<Parameters>();
        turnManager = TurnManager.Instance;
        battleNavigate = gameObject.GetComponentInParent<BattleNavigate>();
        hpBar = gameObject.GetComponentInChildren<HPScript>(); // Rocky HP bar stuff


        if (unitData.UnitWeapon != null)
            weaponRange = GetComponent<UnitData>().UnitWeapon.range;
        else
            weaponRange = 1;

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
                HPChange(10);
                Debug.Log(unitParameters.CHP);
                if (unitParameters.CHP <= 0)
                {
                    KillUnit();
                }
            }

            //Hit Z to simulated a selected unit to be in attacking phase
            if (Input.GetKeyDown(KeyCode.Z) && !actionIsDone)
            {
                Attacking();
            }

            if (Input.GetKeyDown(KeyCode.B) && moveIsDone && !actionIsDone)
            {
                battleNavigate.Return();
                moveIsDone = false;
            }

        }
    }


    public void Attacking()
    {
        Debug.Log(unitData.UnitName + " is atacc");
        isAttacking = true;

    }

    public void Moving()
    {

    }

    public void Items()
    {

    }

    public void Skills()
    {

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

        if (Int2.Distance(aPos, dPos) > weaponRange)
        {
            Debug.Log("Cannot attack a unit that is out of range!");
            return;
        }

        Debug.Log("Unit is attacking this unit for " + Attack.CalculateProjectedDamage(attacker, defender) + " damage but will take " +
            Attack.CalculateProjectedDamage(defender, attacker) + " damage.");

        Attack.CommenceBattle(gameObject, defender);
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

    private void HPChange(int hpDelta)
    {
        DamagePopup.Create(gameObject.transform.position, hpDelta, false);
    }

    internal void KillUnit()
    {
        FinishUnit();
        battleNavigate.ResetNavigator();
        if (unitData.UnitTeam == Team.HERO)
        {
            TurnManager.Instance.playerUnitCount--;
            BattleState.playerUnitCount--;
        }
        else if (unitData.UnitTeam == Team.ENEMY)
        {
            TurnManager.Instance.enemyUnitCount--;
            BattleState.enemyUnitCount--;

        }
        //TurnManager.Instance.CheckWinConditions();
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

    // Cannot move but can attack
    internal void DoneMoving()
    {
        if (!moveIsDone)
        {
            moveIsDone = true;
            unitSelectable.ChangeColor(1);
            //Debug.Log("Unit finished moving");
        }
    }



    IEnumerator DeathAnimation()
    {

        SpriteRenderer render = gameObject.GetComponent<SpriteRenderer>();

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
