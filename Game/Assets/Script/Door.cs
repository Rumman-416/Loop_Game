using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public GameObject intText;
    public Button interactButton; // Reference to your shared UI button
    public Animator doorAnim;

    private bool interactable;
    private bool toggle;

    // Reference to the currently interactable door
    private static Door currentInteractableDoor;

    private void SetButtonVisibility(bool visible)
    {
        intText.SetActive(visible);
        interactButton.gameObject.SetActive(visible);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            SetButtonVisibility(true);
            interactable = true; // Set interactable to true when the player is in the trigger zone

            // Store a reference to this door as the current interactable door
            currentInteractableDoor = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            SetButtonVisibility(false);
            interactable = false; // Set interactable to false when the player leaves the trigger zone

            // Clear the current interactable door reference
            if (currentInteractableDoor == this)
            {
                currentInteractableDoor = null;
            }
        }
    }

    private void Update()
    {
        // Check for interaction using the specified key/button
        if (interactable && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.E)))
        {
            TryToggleDoor();
        }
    }

    private void TryToggleDoor()
    {
        if (currentInteractableDoor == this) // Check if this is the currently interactable door
        {
            if (!toggle)
            {
                doorAnim.SetTrigger("open");
            }
            else
            {
                doorAnim.SetTrigger("close");
            }

            doorAnim.ResetTrigger(toggle ? "open" : "close");
            toggle = !toggle;
            intText.SetActive(false);
            interactable = false;
        }
    }

    // Method to handle UI button click
    public void OnInteractButtonClick()
    {
        TryToggleDoor();
    }
}
