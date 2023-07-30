using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float speedIncreaseAmount = 0.1f;
    [SerializeField] private float turnspeed = 200f;
    [SerializeField] private float boostSpeedMultiplier = 2f;
    [SerializeField] private float boostSpeedDuration = 2f;

    private bool hasBoost = false;
    private float boostDuration;
    private int steerValue;

    // Update is called once per frame
    void Update()
    {
        speed += speedIncreaseAmount * Time.deltaTime;
        if (hasBoost)
        {
            boostDuration -= Time.deltaTime;
            transform.Translate(Vector3.forward * speed * boostSpeedMultiplier*  Time.deltaTime);
            transform.Rotate(0f, steerValue * turnspeed * boostSpeedMultiplier * Time.deltaTime, 0f);
            if (boostDuration <= 0)
            {
                hasBoost = false;
            }
        }
        else
        {
            transform.Rotate(0f, steerValue * turnspeed * Time.deltaTime, 0f);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (other.CompareTag("Boost"))
        {
            StartCoroutine(EnableBoostObject(other.gameObject));
            hasBoost = true;
            boostDuration += boostSpeedDuration;
        }
    }

    private IEnumerator EnableBoostObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(true);
    }

    public void Steer(int value)
    {
        steerValue = value;
    }
}
