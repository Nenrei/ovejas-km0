using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverZone : MonoBehaviour
{

    [SerializeField] GameplayManager manager;
    [SerializeField] UiAboveObject deliverSheep;
    [SerializeField] Transform player;
    [SerializeField] List<Transform> deliverTarget;

    Sheep targetSheep;
    bool keyboardAndMouse;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        float horizontalX = Input.GetAxisRaw("XboxH");
        float verticalX = Input.GetAxisRaw("XboxV");
        if (horizontal != 0 || vertical != 0)
            keyboardAndMouse = true;
        else if (horizontalX != 0 || verticalX != 0)
            keyboardAndMouse = false;

        if (targetSheep != null && (Input.GetButtonDown("Fire1") || Input.GetButtonDown("XboxA")))
        {
            manager.UpdateSheeps();
            targetSheep.DeliverSheep(GetAvailableDeliverPoint());
            deliverSheep.HideText();
            targetSheep = null;
        }
    }

    Transform GetAvailableDeliverPoint()
    {
        Transform deliverPoint;
        do
        {
            deliverPoint = deliverTarget[Random.Range(0, deliverTarget.Count)];
        } while (!deliverPoint.gameObject.activeInHierarchy);

        return deliverPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sheep"))
        {
            deliverSheep.ShowText(player, keyboardAndMouse);
            targetSheep = other.GetComponent<Sheep>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Sheep"))
        {
            deliverSheep.HideText();
            targetSheep = null;
        }
    }

    public void RestartDeliverPoints()
    {
        foreach (Transform deliverPoint in deliverTarget)
        {
            if (!deliverPoint.gameObject.activeInHierarchy)
                deliverPoint.gameObject.SetActive(true);
        }
    }
}
