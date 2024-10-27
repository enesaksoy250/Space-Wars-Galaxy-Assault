using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SpaceStation : MonoBehaviour
{

 
    [SerializeField] GameObject spaceShip;
    [SerializeField] Image healthBar;
    [SerializeField] float outputRange;
    [SerializeField] float stationHealth;
    [SerializeField] float destructionScore;
    Animator animator;
    EnemyFire enemyFire;
    private bool isLoop=true;
    private float initialStationHealth;
    
    
    private void Awake()
    {

        initialStationHealth = stationHealth;
        FindObject();      

    }


    private void Start()
    {

        StartCoroutine(CreateSpaceshipCoroutine());

    }

    public void ChangeStationHealth(float power)
    {

        stationHealth -= power;
        healthBar.transform.parent.gameObject.SetActive(true);
        healthBar.fillAmount = (stationHealth / initialStationHealth);

        if(stationHealth <= 0 && isLoop) 
        {

            isLoop = false;

            if(enemyFire!=null)
            {

                enemyFire.isFiringToPlayer = false;

            }


            AudioManager.instance.PlayAudio("explosion");
            DataBaseManager.instance.UpdateFirebaseInfo("totalDestruction", 1);
            PlayerPoints.instance.ChangeScore(destructionScore);
            GameManager.instance.IncreaseNumber();
            //PlayerFire.instance.isFiring = false;
            StopCoroutine(CreateSpaceshipCoroutine());
            animator.Play("ExplosionForStation");
            Destroy(gameObject, .75f);
        
        }

    }

   

    IEnumerator CreateSpaceshipCoroutine()
    {

        while(true)
        {

            if(PlayerHealth.instance.health > 0 && GameManager.instance.isPlaying)
            {

                Instantiate(spaceShip, transform.position, Quaternion.identity);

            }
                        
            yield return new WaitForSeconds(outputRange);

        }

    }

    public float GetStationHealth()
    {

        return stationHealth;

    }


    private void FindObject()
    {

        enemyFire = FindObjectOfType<EnemyFire>();
        animator = GetComponent<Animator>();

    }
}
