using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform destination;
    public Transform departure;
    public Vector3 currentPosition;
    public Vector3 destinationPosition;
    public Vector3 departurePosition;
    public float middleX;


    // Start is called before the first frame update
    void Start()
    {
        destinationPosition = destination.position;
        departurePosition = departure.position;
        currentPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("KeyDown");
            StartCoroutine(Move());
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            CardUIManager.Instance.DrawCard(0, 1);
        }
    }

    IEnumerator Move()
    {
        int breaker = 0;
        Vector3 currentPosition = transform.position;
        float midPointX = (departure.position.x + destination.position.x) / 2;
        while (currentPosition.x > destination.position.x)
        {
            currentPosition.x -= 15;
            currentPosition.y += (currentPosition.x - midPointX)/30;
            transform.position = currentPosition;
            breaker++;
            if (breaker == 10000) break;
            yield return null;
        }
        yield break;
    }
}
